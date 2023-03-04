using IssueTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssueTracker.Services
{
    public interface IIssueStore
    {
        IReadOnlyCollection<Issue> Issues { get; }
        Task SaveIssuesAsync();
        Task InitializeAsync();
        Issue Add();
        void Delete(int id);
    }
}
