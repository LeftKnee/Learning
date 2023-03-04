using IssueTracker.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IssueTracker
{
    /// <summary>
    /// The detail page for issues
    /// </summary>
    public sealed partial class IssuePage : Page
    {
        #region Fields
        private IssueDetailViewModelBase _viewModel;
        #endregion

        #region Page Events
        /// <summary>
        /// This occurs when the Navigation service navigates to this page
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _viewModel = (IssueDetailViewModelBase)e.Parameter;
            DataContext = _viewModel;
            base.OnNavigatedTo(e);
        }
        #endregion

        #region Constructor
        public IssuePage()
        {
            InitializeComponent();
        }
        #endregion

        #region UI
        public string FormatDate(string header, DateTimeOffset? dateTime)
            => $"{header} {dateTime:MMM dd, yyyy hh:mm tt}";
        #endregion
    }
}
