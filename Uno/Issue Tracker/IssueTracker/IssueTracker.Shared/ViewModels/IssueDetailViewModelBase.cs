using IssueTracker.Models;
using IssueTracker.Services;
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace IssueTracker.ViewModels
{
    /// <summary>
    /// The view model for issue details
    /// </summary>
    public abstract class IssueDetailViewModelBase : NotifyPropertyChangedBase
    {
        #region Fields
        public Issue Issue { get; }
        protected readonly INavigationService _navigationService;
        #endregion

        #region Constructor
        protected IssueDetailViewModelBase
            (
            INavigationService navigationService,
            Issue issue
            )
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));

            _navigationService = navigationService;
            Issue = issue;
        }
        #endregion

        #region Public Properties
        public int Id
        {
            get => Issue.Id;
            set
            {
                if (Issue.Id != value)
                {
                    Issue.Id = value;
                    RaisePropertyChanged(nameof(Id));
                }
            }
        }

        public IssueType Type
        {
            get => Issue.Type;
            set
            {
                if (Issue.Type != value)
                {
                    Issue.Type = value;
                    RaisePropertyChanged(nameof(Type));
                    //Tell the UI that the color has changed because the issue type was changed
                    RaisePropertyChanged(nameof(IssueTypeBrush));
                }
            }
        }

        public string Title
        {
            get => Issue.Title;
            set
            {
                if (Issue.Title != value)
                {
                    Issue.Title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        public string Description
        {
            get => Issue.Description;
            set
            {
                if (Issue.Description != value)
                {
                    Issue.Description = value;
                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        public IssueStatus Status
        {
            get => Issue.Status;
        }

        public int Estimation
        {
            get => Issue.Estimation;
            set
            {
                if (Issue.Estimation != value)
                {
                    Issue.Estimation = value;
                    RaisePropertyChanged(nameof(Estimation));
                }
            }
        }

        public DateTimeOffset CreatedAt
        {
            get => Issue.CreatedAt;
            set
            {
                if (Issue.CreatedAt != value)
                {
                    Issue.CreatedAt = value;
                    RaisePropertyChanged(nameof(CreatedAt));
                }
            }
        }

        public virtual DateTimeOffset? StartedAt
        {
            get => Issue.StartedAt;
            set
            {

                if (Issue.StartedAt != value)
                {
                    Issue.StartedAt = value;
                    RaisePropertyChanged(nameof(StartedAt));
                    //Tell the UI to update the status based on the change
                    RaisePropertyChanged(nameof(Status));
                }
            }
        }

        public virtual DateTimeOffset? CompletedAt
        {
            get => Issue.CompletedAt;
            set
            {
                if (Issue.CompletedAt != value)
                {
                    Issue.CompletedAt = value;
                    RaisePropertyChanged(nameof(CompletedAt));
                    //Tell the UI to update the status based on the change
                    RaisePropertyChanged(nameof(Status));
                }
            }
        }

        public Brush IssueTypeBrush
        {
            get
            {
                var color = Color.FromArgb(255, 248, 89, 119);

                switch (Type)
                {
                    case IssueType.Feature:
                        color = Color.FromArgb(255, 21, 155, 255);
                        break;
                    case IssueType.Task:
                        color = Color.FromArgb(255, 122, 103, 248);
                        break;
                }

                return new SolidColorBrush(color);
            }
        }
        #endregion
    }
}
