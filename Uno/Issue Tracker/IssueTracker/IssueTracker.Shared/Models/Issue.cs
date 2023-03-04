using System;

namespace IssueTracker.Models
{
    public class Issue
    {
        #region Public Properties
        public int Id { get; set; }
        public IssueType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IssueStatus Status
        {
            get
            {
                if (CompletedAt.HasValue) return IssueStatus.Completed;
                if (StartedAt.HasValue) return IssueStatus.Started;
                return IssueStatus.Planned;
            }
        }

        public int Estimation { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        #endregion
    }
}
