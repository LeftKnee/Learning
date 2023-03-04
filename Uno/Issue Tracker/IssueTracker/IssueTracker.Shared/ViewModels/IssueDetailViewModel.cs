using IssueTracker.Commands;
using IssueTracker.Services;
using IssueTracker.Models;
using System.Windows.Input;
using Windows.UI.Core;

namespace IssueTracker.ViewModels
{
    public class IssueDetailViewModel : IssueDetailViewModelBase
    {
        #region Commands
        public ICommand Select { get; }
        #endregion

        #region Constructor
        public IssueDetailViewModel
            (
            INavigationService navigationService,
            IIssueStore issueStore,
            Issue issue
            )
            : base(navigationService, issue)
        {
            Select = new SimpleCommand(() =>
            {
                //This navigates to the Issue Page. It should only happen from the Bug List Page
                _navigationService.Navigate(typeof(IssuePage), new IssuePageViewModel(_navigationService, issueStore, issue), AppViewBackButtonVisibility.Visible);
            });
        }
        #endregion
    }
}
