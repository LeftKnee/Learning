using IssueTracker.Commands;
using IssueTracker.Services;
using IssueTracker.Models;
using System;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace IssueTracker.ViewModels
{
    public class IssuePageViewModel : IssueDetailViewModelBase
    {
        #region Public Properties
        public ICommand Start { get; }
        public ICommand Complete { get; }
        public ICommand ClearStatus { get; }
        public ICommand Delete { get; }
        public IssueType[] IssueTypeList => new[]
        {
            IssueType.Bug,
            IssueType.Feature,
            IssueType.Task
        };
        public Visibility StartVisibility => !StartedAt.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility CompleteVisibility => StartedAt.HasValue && !CompletedAt.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ClearStatusVisibility => StartedAt.HasValue || CompletedAt.HasValue ? Visibility.Visible : Visibility.Collapsed;

        public override DateTimeOffset? StartedAt
        {
            get => base.StartedAt;
            set
            {
                base.StartedAt = value;

                //Tell the UI that buttons should reflect whether or not the issue has started
                RaisePropertyChanged(nameof(StartVisibility));
                RaisePropertyChanged(nameof(CompleteVisibility));
                RaisePropertyChanged(nameof(ClearStatusVisibility));
            }
        }

        public override DateTimeOffset? CompletedAt
        {
            get => base.CompletedAt;
            set
            {
                base.CompletedAt = value;

                //Tell the UI that buttons should reflect whether or not the issue has completed
                RaisePropertyChanged(nameof(StartVisibility));
                RaisePropertyChanged(nameof(CompleteVisibility));
                RaisePropertyChanged(nameof(ClearStatusVisibility));
            }
        }
        #endregion

        #region Constructor
        public IssuePageViewModel(
            INavigationService navigationService,
            IIssueStore issueStore,
            Issue issue
            ) : base(navigationService, issue)
        {
            Start = new SimpleCommand(() => { StartedAt = DateTimeOffset.Now.ToLocalTime(); });
            Complete = new SimpleCommand(() => { CompletedAt = DateTimeOffset.Now.ToLocalTime(); });
            ClearStatus = new SimpleCommand(() => { CompletedAt = null; StartedAt = null; });
            Delete = new SimpleCommand(() => 
            { 
                issueStore.Delete(issue.Id);
                _navigationService.Navigate(typeof(IssueListPage), new IssueListViewModel(_navigationService, issueStore), AppViewBackButtonVisibility.Collapsed);
            });
        }
        #endregion
    }
}
