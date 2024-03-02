using Android.App;
using Android.Content;
using Android.Webkit;
using Companion.Android;
using Companion.Android.Services;
using Companion.Common;
using Companion.Enum;
using Companion.Events;
using Companion.Helpers;
using Companion.Services;
using Companion.ViewModels;
using Prism.Events;
using Prism.Ioc;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(ExtendedWebViewModel), typeof(ExtendedWebViewRenderer))]
namespace Companion.Android
{
    /// <summary>
    /// Class to set height of web view dynamically
    /// </summary>
    public class ExtendedWebViewRenderer : WebViewRenderer
    {
        static ExtendedWebViewModel _xwebView = null;
        WebView _webView;

        public ExtendedWebViewRenderer(Context context) : base(context)
        {
        }

        /// <summary>
        /// Class for ExtendedWebViewClient
        /// </summary>
        class ExtendedWebViewClient : WebViewClient
        {
            string strAndroidFile = "file://";

            /// <summary>
            /// Method to set height of webview as per content
            /// </summary>
            /// <param name="view"></param>
            /// <param name="url"></param>
            public override async void OnPageFinished(WebView view, string url)
            {
                try
                {
                    if (_xwebView != null)
                    {
                        _xwebView.IsLoading = true;
                        int i = 10;
                        while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
                            await System.Threading.Tasks.Task.Delay(100);

                        _xwebView.HeightRequest = view.ContentHeight;
                        _xwebView.InvokeCompleted();
                    }
                    base.OnPageFinished(view, url);
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
                finally
                {
                    if (_xwebView != null)
                    {
                        _xwebView.IsLoading = false;
                        _xwebView.InvokeCompleted();

                    }
                }
            }

            /// <summary>
            /// Method to redirect url to another browser
            /// </summary>
            /// <param name="view"></param>
            /// <param name="request"></param>
            /// <returns></returns>
            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                bool value = true;
                string binaryId = "";
                try
                {
                    var url = request.Url.ToString();
                    if (url.Contains("$"))
                    {
                        request.Dispose();
                        //var mainUrl = url.Replace("$$$", "$");
                        var urlArray = url.Split('$');
                        var mainUrl = urlArray.FirstOrDefault();
                        binaryId = urlArray.LastOrDefault();

                        if (!Path.HasExtension(mainUrl))
                        {
                            IAppPermissionService appPermissionManager = new AppPermissionService();
                            var hasStoragePermissionGranted = appPermissionManager.CheckPermission(AppPermission.WriteExternalStorage);
                            if (hasStoragePermissionGranted)
                            {
                                SaveAndOpenDocument(binaryId);
                            }
                            else
                            {
                                appPermissionManager.AskPermission(new System.Collections.Generic.List<AppPermission> { AppPermission.WriteExternalStorage });
                            }
                        }
                        else if (mainUrl.Contains(strAndroidFile) && !string.IsNullOrEmpty(binaryId))
                        {
                            var documentUrl = GetFilePathWithProtocol(mainUrl, binaryId);
                            OpenDocumentInRelatedControlOrApp(documentUrl, binaryId, true);
                        }
                        else
                        {
                            OpenDocumentInRelatedControlOrApp(mainUrl, binaryId);
                        }
                        value = true;
                    }
                    else if (Convert.ToString(url).StartsWith("http") && !url.Contains("#dd"))
                    {
                        var uri = new Uri(Convert.ToString(url));
                        Launcher.OpenAsync(uri);
                        value = true;
                    }
                    else
                    {
                        if (url.ToLower().Contains("blocked"))
                        {
                            ShowToastMessage();
                            return value;
                        }

                        var urlArray = url.Split('#');
                        if (urlArray.Length > 0 && urlArray[1] == "dd")
                        {
                            IFileManagerService appFileManager = new FileManagerService();
                            var directoryPath = appFileManager.GetDirectoryPath(AppLocalDiretory.DocumentDirectory);
                            var fileExtension = Path.GetExtension(urlArray[0]).Split('.')[1];
                            if (System.Enum.GetNames(typeof(AppFileExtension)).Any(x => x.ToLower() == fileExtension.ToLower()))
                            {
                                var fileName = Path.GetFileName(urlArray[0]);
                                if (!string.IsNullOrEmpty(directoryPath) && !string.IsNullOrEmpty(fileName))
                                {
                                    var filePath = Path.Combine(directoryPath, fileName);
                                    var isFileExist = File.Exists(filePath);
                                    if (!isFileExist)
                                        ShowToastMessage();
                                }
                            }
                            else
                            {
                                ShowToastMessage();
                                return value;
                            }
                        }
                    }
                }
                catch (ActivityNotFoundException)
                {
                    ShowAlertDialogForNotRelatedAppInstalled(binaryId);
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
                return value;
            }

            private async void SaveAndOpenDocument(string binaryId)
            {
                try
                {
                    var _newsHelper = App.Instance.Container.Resolve<INewsHelper>();
                    var mainUrl = await _newsHelper.SaveDocument(binaryId);
                    OpenDocumentInRelatedControlOrApp(mainUrl, binaryId, true);
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
            }

            /// <summary>
            /// To open file in related app ot control
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="binaryID"></param>
            private async void OpenDocumentInRelatedControlOrApp(string filePath, string binaryID, bool isNeedToUpdateNewsDescription = false)
            {
                try
                {
                    if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(binaryID))
                    {
                        var _newsHelper = App.Instance.Container.Resolve<INewsHelper>();
                        if (filePath.Contains(CommonFields.DotPDFExtension))
                        {
                            var _event = App.Instance.Container.Resolve<IEventAggregator>();
                            _event?.GetEvent<ShowPdfToPdfViewerEvent>().Publish(binaryID);
                        }
                        else
                        {
                            await Xamarin.Essentials.Launcher.OpenAsync(filePath);
                        }
                        if (isNeedToUpdateNewsDescription)
                        {
                            await _newsHelper.UpdateNewsDocumentLinks(binaryID);
                        }
                    }
                }
                catch (ActivityNotFoundException)
                {
                    ShowAlertDialogForNotRelatedAppInstalled(binaryID);
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
            }

            /// <summary>
            /// Show the alert dialog for not related apps found
            /// </summary>
            /// <param name="binaryId"></param>
            private void ShowAlertDialogForNotRelatedAppInstalled(string binaryId)
            {
                try
                {
                    string alertMessage, downloadOfficeApp, alertTitleText, alertOkButtonText, alertCancelButtonText;
                    alertMessage = downloadOfficeApp = alertTitleText = alertOkButtonText = alertCancelButtonText = string.Empty;

                    if (App.Labels?.Count > 0)
                    {
                        var _resourceHelper = App.Instance.Container.Resolve<IResourceHelper>();

                        downloadOfficeApp = _resourceHelper.GetLabelTextByLabelName(LabelKey.Download);
                        alertMessage = App.CurrentUser.UserLoginType == LogInType.NonAD
                                       ? _resourceHelper.GetLabelTextByLabelName(LabelKey.NonADUserNoRelatedAppInstalled)
                                       : _resourceHelper.GetLabelTextByLabelName(LabelKey.ADUserNoRelatedAppInstalled);

                        alertTitleText = _resourceHelper.GetLabelTextByLabelName(LabelKey.NewsDocumentAlertTitle);
                        alertOkButtonText = _resourceHelper.GetLabelTextByLabelName(LabelKey.Ok);
                        alertCancelButtonText = _resourceHelper.GetLabelTextByLabelName(LabelKey.Cancel);

                        if (!string.IsNullOrEmpty(alertMessage))
                        {
                            var context = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                            AlertDialog.Builder alertDiag = new AlertDialog.Builder(context);
                            alertDiag.SetTitle(_resourceHelper.GetLabelTextByLabelName(LabelKey.NewsDocumentAlertTitle));
                            alertDiag.SetMessage(alertMessage);

                            //Add ok button for AD/AAD uyser
                            if (App.CurrentUser.UserLoginType != LogInType.NonAD)
                            {
                                alertDiag.SetNeutralButton(_resourceHelper.GetLabelTextByLabelName(LabelKey.Ok), async (senderAlert, args) =>
                                {
                                    if (!string.IsNullOrEmpty(binaryId))
                                    {
                                        var _newsHelper = App.Instance.Container.Resolve<INewsHelper>();
                                        var newsBinary = await _newsHelper.GetNewsBinary(binaryId);
                                        if (newsBinary != null && !string.IsNullOrEmpty(newsBinary.ActualLink))
                                        {
                                            await Launcher.TryOpenAsync(new Uri(newsBinary.ActualLink));
                                        }
                                    }
                                });
                            }

                            alertDiag.SetPositiveButton(downloadOfficeApp, (senderAlert, args) =>
                            {
                                string OfficeAppPkgName = "com.microsoft.office.officehubrow";
                                string url = string.Format("market://details?id={0}", OfficeAppPkgName);
                                Xamarin.Essentials.Launcher.TryOpenAsync(new Uri(url));
                            });

                            alertDiag.SetNegativeButton(_resourceHelper.GetLabelTextByLabelName(LabelKey.Cancel), (senderAlert, args) =>
                            {
                                alertDiag.Dispose();
                            });
                            Dialog diag = alertDiag.Create();
                            if (diag != null)
                            {
                                diag.SetCanceledOnTouchOutside(true);
                                diag.Show();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
            }

            /// <summary>
            /// //get path with ms protocol from simple file storage path
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="binaryId"></param>
            /// <returns></returns>
            public string GetFilePathWithProtocol(string filePath, string binaryId)
            {
                try
                {
                    if (filePath.Contains(strAndroidFile))
                    {
                        string updatePath = filePath.Replace(strAndroidFile, string.Empty);
                        if (!string.IsNullOrEmpty(binaryId))
                        {
                            string mobileStorageFilePath = string.Empty;
                            var _newsHelper = App.Instance.Container.Resolve<INewsHelper>();
                            var task = Task.Run(() => _newsHelper.GetNewsBinary(binaryId)).ContinueWith(i =>
                            {
                                var newsBinary = i.Result;
                                if (newsBinary != null)
                                {
                                    var protocol = newsBinary.Protocol;
                                    updatePath = string.Format("{0}{1}", protocol, updatePath);
                                }
                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
                            task.Wait();
                            return updatePath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
                return default;
            }

            private void ShowToastMessage()
            {
                try
                {
                    var _resourceHelper = App.Instance.Container.Resolve<IResourceHelper>();
                    var message = _resourceHelper.GetLabelTextByLabelName(LabelKey.LinkRemovedMessage);
                    if (!string.IsNullOrEmpty(message))
                        new ToastService().Show(message);
                }
                catch (Exception ex)
                {
                    Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// On element change method of WebViewRenderer 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            try
            {
                base.OnElementChanged(e);
                _xwebView = e.NewElement as ExtendedWebViewModel;
                _webView = Control;

                if (e.OldElement == null)
                {
                    _webView.SetWebViewClient(new ExtendedWebViewClient());
                }

                this.Control.SetBackgroundColor(Color.FromHex(App.DesignSettings.AppBackgroundColor).AddLuminosity(0).ToAndroid());
            }
            catch (Exception ex)
            {
                Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

    }
}