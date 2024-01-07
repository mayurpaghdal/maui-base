namespace MauiAppDemo.ViewModels;

public class NewsViewModel : ChildBaseViewModel
{
    private const int SUMMARY_LINKS_COUNT = 12;

    #region Data Members

    private string _emptyViewMessage;
    private bool _isEmptyViewVisible = false;
    private DataTemplate _dataTemplate;
    private bool _isRefreshing = false;
    private bool _showTopNewsSlider = false;
    private ImageSource _viewImgSourceWhite;
    private ImageSource _likeImgSource;
    private ImageSource _unlikeImgSourceWhite;
    private ImageSource _commentImgSourceWhite;
    private bool _canNavigate = true;
    private bool _showLinksViewAll;
    private bool _showGetStartedSection = false;
    private bool _showLatestNewsCount;
    private int _latestNewsCount;
    #endregion

    #region Properties

    public bool ShowGetStartedSection
    {
        get => _showGetStartedSection;
        set => SetProperty(ref _showGetStartedSection, value);
    }
    public bool ShowTopNewsSlider
    {
        get => _showTopNewsSlider;
        set => SetProperty(ref _showTopNewsSlider, value);
    }

    //public ObservableCollection<NewsModel> SliderNews
    //{
    //    get => _sliderNews;
    //    set
    //    {
    //        ShowTopNewsSlider = value?.Count > 0;
    //        SetProperty(ref _sliderNews, value);
    //    }
    //}

    public int LatestNewsCount
    {
        get => _latestNewsCount;
        set
        {
            ShowLatestNewsCount = LatestNewsCount > 0;
            SetProperty(ref _latestNewsCount, value);
        }
    }

    public bool ShowLatestNewsCount
    {
        get => _showLatestNewsCount;
        set => SetProperty(ref _showLatestNewsCount, value);
    }

    //public bool ShowLinksViewAll => allLinks?.Count > SUMMARY_LINKS_COUNT;
    //{
    //    get => _showLinksViewAll;
    //    set
    //    {
    //        SetProperty(ref _showLinksViewAll, value);
    //    }
    //}

    //public ObservableCollection<ExternalLinkModel> Links
    //{
    //    get => _links;
    //    set
    //    {
    //        SetProperty(ref _links, value);
    //        RaisePropertyChanged(nameof(ShowLinksViewAll));
    //    }
    //}

    public string EmptyViewMessage
    {
        get => _emptyViewMessage;
        set => SetProperty(ref _emptyViewMessage, value);
    }

    public bool IsEmptyViewVisible
    {
        get => _isEmptyViewVisible;
        set => SetProperty(ref _isEmptyViewVisible, value);
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public ImageSource ViewImgSourceWhite { get => _viewImgSourceWhite; set => SetProperty(ref _viewImgSourceWhite, value); }
    public ImageSource LikeImgSource { get => _likeImgSource; set => SetProperty(ref _likeImgSource, value); }
    public ImageSource UnlikeImgSourceWhite { get => _unlikeImgSourceWhite; set => SetProperty(ref _unlikeImgSourceWhite, value); }
    public ImageSource CommentImgSourceWhite { get => _commentImgSourceWhite; set => SetProperty(ref _commentImgSourceWhite, value); }
    public bool CanNavigate
    {
        get { return _canNavigate; }
        set { SetProperty(ref _canNavigate, value); }
    }
    #endregion

    #region Services
    private readonly IBlobCache _cache;
    #endregion

    #region Ctor
    public NewsViewModel(IBlobCache cache)
        : base()
    {
        
        _cache = cache;

        Title = "Home"; // _resourceHelper.GetLabelTextByLabelName(homePage.Label);

        InitCommands();
        SetResources();
    }
    #endregion

    #region Overridden Methods
    public override void InitCommands()
    {
        
    }

    public override void SetResources()
    {
        base.SetResources();
        
    }

    public override async Task OnNavigatedTo(NavigationParameters parameters)
    {
        IsLoading = true;
        try
        {
            ShowFilter = false;
            ShowSearch = false;
            ShowSyncIcon = false;
        }
        catch (Exception ex)
        {
            //Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public override async Task OnRecurringNavigatedTo(NavigationParameters parameters)
    {
        try
        {
            ShowFilter = false;
            ShowSearch = false;
            ShowSyncIcon = false;
        }
        catch (Exception ex)
        {
            //Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
        }
        finally
        {
            //IsEmptyViewVisible = SliderNews is null || SliderNews.Count == 0;
        }
    }
    #endregion
}
