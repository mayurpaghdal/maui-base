namespace MauiBase.ViewModels
{
    public class ChildBaseViewModel : RootBaseViewModel, IChildViewBase
    {
        #region Events
        public event EventHandler<int> NotificationCountUpdated;
        public event EventHandler<string> ChildViewTitleUpdated;
        public event EventHandler<bool> ShowSearchUpdated;
        public event EventHandler<bool> ShowSyncIconUpdated;
        public event EventHandler<bool> ShowFilterUpdated;
        public event EventHandler<string> FilterIconUpdated;
        public event Action SearchClicked;
        public Action FilterClicked;
        public Action SyncClicked;
        #endregion

        #region Child Base Props

        private bool _showSearch;
        public bool ShowSearch
        {
            get => _showSearch;
            set
            {
                SetProperty(ref _showSearch, value);
                ShowSearchUpdated?.Invoke(this, value);
            }
        }

        private bool _showFilter;
        public bool ShowFilter
        {
            get => _showFilter;
            set
            {
                SetProperty(ref _showFilter, value);
                ShowFilterUpdated?.Invoke(this, value);
            }
        }

        private string _filterIcon = "icon_filter";
        public string FilterIcon
        {
            get => _filterIcon;
            set
            {
                SetProperty(ref _filterIcon, value);
                FilterIconUpdated?.Invoke(this, value);
            }
        }

        private bool _showSyncIcon;
        public bool ShowSyncIcon
        {
            get => _showSyncIcon;
            set
            {
                SetProperty(ref _showSyncIcon, value);
                ShowSyncIconUpdated?.Invoke(this, value);
            }
        }

        private bool _isActiveSelection;
        public bool IsActiveSelection
        {
            get => _isActiveSelection;
            set => SetProperty(ref _isActiveSelection, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _pageTitle;
        public string Title
        {
            get => _pageTitle;
            set
            {
                SetProperty(ref _pageTitle, value);
                ChildViewTitleUpdated?.Invoke(this, value);
            }
        }

        public CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        #endregion

        #region Handlers
        public event EventHandler<PropertyChangedEventArgs> InternalPropertyChanged;
        public new event EventHandler<RootNavigationRequestedEventArgs> RootNavigationRequested;
        #endregion

        #region Services
        protected readonly IBlobCache _cache;
        protected readonly IEventAggregator _eventAggregator;
        #endregion

        #region Ctor
        public ChildBaseViewModel(IEventAggregator eventAggregator,
                                  IBlobCache cache = null!)
        {
            _eventAggregator = eventAggregator;
            _cache = cache;

            ShowSearch = false;
            ShowFilter = false;
            ShowSyncIcon = false;

        }
        #endregion

        #region Public Methods

        #region Navigation
        public void BaseNavigate(string pageName,
                                 NavigationParameters parameters = null!,
                                 bool isAnimated = true,
                                 bool isChildViewNavigation = true,
                                 bool ignoreStackInsertation = false)
        {
            RootNavigationRequested?.Invoke(this, new RootNavigationRequestedEventArgs()
            {
                IsAnimated = isAnimated,
                PageName = pageName,
                Parameters = parameters,
                IsChildViewNavigation = isChildViewNavigation,
                IsBackNavigation = false,
                IgnoreStackInsertation = ignoreStackInsertation
            });
        }

        public void NavigateWithService(string pageName, NavigationParameters parameters = null!)
        {
            RootNavigationRequested?.Invoke(this, new RootNavigationRequestedEventArgs()
            {
                IsAnimated = true,
                PageName = pageName,
                Parameters = parameters,
                IsChildViewNavigation = true,
                IsBackNavigation = false,
                NavigateWithService = true
            });
        }

        public void NavigateWithService(BasePage page, bool isModalNavigation = false, bool isAnimated = true)
        {
            RootNavigationRequested?.Invoke(this, new RootNavigationRequestedEventArgs()
            {
                IsAnimated = isAnimated,
                Page = page,
                IsChildViewNavigation = true,
                IsBackNavigation = false,
                NavigateWithService = true,
                IsModalNavigation = isModalNavigation
            });
        }

        public void BaseBackNavigate(NavigationParameters parameters = null!, bool isAnimated = true)
        {
            if (parameters == null)
                RootNavigationRequested?.Invoke(this, new RootNavigationRequestedEventArgs()
                {
                    IsBackNavigation = true,
                    IsChildViewNavigation = true
                });
            else
                RootNavigationRequested?.Invoke(this, new RootNavigationRequestedEventArgs()
                {
                    IsBackNavigation = true,
                    Parameters = parameters,
                    IsChildViewNavigation = true
                });
        }

        #endregion

        #region Public Methods
        public void InvokeFilterClick() => FilterClicked?.Invoke();
        public void InvokeSearchClick() => SearchClicked?.Invoke();
        public void InvokeSyncIconClick() => SyncClicked?.Invoke();

        public virtual Task DestroyAsync()
        {
            this.PropertyChanged -= PropChanged!;

            return Task.FromResult(true);
        }
        public void NotifyTitleChange(string newTitle)
        {
            Title = newTitle;
        }
        public virtual Task<bool> OnPushActionReceived(NotificationPayloadModel payload)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// When user gets push notification when the app is running in foreground.
        /// </summary>
        /// <param name="payload">Cross payload object.</param>
        /// <returns>Override and return true if child has to do special things, false if nothing.</returns>
        public virtual Task<bool> OnForegroundPushReceived(NotificationPayloadModel payload)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Refers to a code block which will be executed each time user navigates into.
        /// </summary>
        /// <returns></returns>
        public virtual Task OnRecurringNavigatedTo(NavigationParameters parameters)
        {
            ShowSearch = false;
            ShowFilter = false;
            return Task.FromResult(true);
        }

        public virtual Task OnNavigatedTo(NavigationParameters parameters)
        {
            return Task.FromResult(true);
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            FilterClicked = null!;
        }
        #endregion

        private void PropChanged(object sender, PropertyChangedEventArgs e)
        {
            InternalPropertyChanged?.Invoke(this, e);
        }
        #endregion

        #region Protected Methods
        protected async Task PerformDifferedSearch(Action<Task> searchHandler)
        {
            try
            {
                Interlocked.Exchange(ref this.CancellationTokenSource, new CancellationTokenSource()).Cancel();
                await Task.Delay(TimeSpan.FromMilliseconds(500), CancellationTokenSource.Token)
                          .ContinueWith(searchHandler,
                                        CancellationToken.None,
                                        TaskContinuationOptions.OnlyOnRanToCompletion,
                                        TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch { }
        }
        #endregion
    }
}
