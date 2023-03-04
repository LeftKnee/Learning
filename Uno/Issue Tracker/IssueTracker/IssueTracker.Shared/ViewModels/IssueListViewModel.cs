using IssueTracker.Commands;
using IssueTracker.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace IssueTracker.ViewModels
{
    public class IssueListViewModel : NotifyPropertyChangedBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IIssueStore _issueStore;
        #endregion

        #region Public Properties
        public ObservableCollection<IssueDetailViewModel> Issues { get; } = new ObservableCollection<IssueDetailViewModel>();
        public ICommand Add { get; }

        public decimal StoryPoints =>
            decimal.Round(
                    (decimal)Issues
                    .Where(i => !i.CompletedAt.HasValue)
                    .Sum(
                        i => i.StartedAt.HasValue ?
                        //Estimation - Days already spent working
                        i.Estimation - (DateTimeOffset.Now.ToLocalTime() - i.StartedAt.Value).TotalDays
                        //Estimation
                        : i.Estimation
                    ), 2);

        #endregion

        #region Constructor
        public IssueListViewModel(
            INavigationService navigationService,
            IIssueStore issueStore
            )
        {
            _navigationService = navigationService;
            _issueStore = issueStore;

            Add = new SimpleCommand(() => AddIssue(issueStore));

            LoadAsync();
        }
        #endregion

        #region Public Methods
        public Task SaveAsync()
        {
            return _issueStore.SaveIssuesAsync();
        }

        public void Refresh()
        {
            Issues.Clear();

            foreach (var issue in _issueStore.Issues.OrderByDescending(i => i.Id))
            {
                Issues.Add(new IssueDetailViewModel(_navigationService, _issueStore, issue));
            }

            RaisePropertyChanged(nameof(StoryPoints));
        }
        #endregion

        #region Private Methods

        private void AddIssue(IIssueStore issueStore)
        {
            var newIssue = issueStore.Add();
            Refresh();
            //This navigates to the Issue Page. It should only happen from the Bug List Page
            _navigationService.Navigate(typeof(IssuePage), new IssuePageViewModel(_navigationService, _issueStore, newIssue), AppViewBackButtonVisibility.Visible);
        }

        private async Task LoadAsync()
        {
            await _issueStore.InitializeAsync();
            Refresh();
        }
        #endregion
    }
}
