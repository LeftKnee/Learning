using IssueTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Console = System.Console;

namespace IssueTracker.Services
{
    public class IssueStore : IIssueStore
    {
        #region Fields
        private readonly List<Issue> _issues = new List<Issue>();
        private string LocalSettingsKey => $"{ApplicationData.Current.LocalFolder.Path}\\Issues.json";
        private SemaphoreSlim _ioSemaphoreSlim = new SemaphoreSlim(0, 1);
        #endregion

        #region Public Properties
        public IReadOnlyCollection<Issue> Issues => new ReadOnlyCollection<Issue>(_issues != null ? _issues : new List<Issue>());
        #endregion

        #region Implementation
        public async Task InitializeAsync()
        {
            try
            {
                //Only load the issues the first time in
                if (_ioSemaphoreSlim.CurrentCount > 0) return;

                await _ioSemaphoreSlim.WaitAsync();

                var issues = new List<Issue>();

                //Check if we have a file to load
                if (await FileExists())
                {
                    //Load the file in to the issues list
                    var json = File.ReadAllText(LocalSettingsKey);
                    issues = JsonConvert.DeserializeObject<List<Issue>>(json);
                }
                else
                {
                    //Create the first feature

                    var issue = new Issue
                    {
                        Id = 1,
                        Title = "Save with an API",
                        Description = @"Save using a backend API instead of writing to a file. The current code only writes to the local disk. To share the data we must write to an API.

## Acceptance Criteria

- Can persist data between sessions
- Data is not stored locally
- User A can seee changes from User B on different computer

## Additional Comments

The IIssueStore interface will probably need a refactor",
                        Estimation = 5,
                        Type = IssueType.Feature,
                        CreatedAt = DateTimeOffset.Now.ToLocalTime(),
                    };

                    issues.Add(issue);

                }

                foreach (var issue in issues)
                {
                    _issues.Add(issue);
                }

                Console.WriteLine($"LOADED FILE WITH {_issues.Count} ISSUES");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                await Task.Yield();
                _ioSemaphoreSlim.Release();
            }
        }

        public async Task SaveIssuesAsync()
        {
            try
            {
                //Don't save if the file was never loaded
                if (_ioSemaphoreSlim.CurrentCount == 0) return;

                await _ioSemaphoreSlim.WaitAsync();

                var json = JsonConvert.SerializeObject(_issues);

                File.WriteAllText(LocalSettingsKey, json);

                Console.WriteLine($"SAVED FILE WITH {_issues.Count} ISSUES");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                _ioSemaphoreSlim.Release();
            }
        }

        public Issue Add()
        {
            //Add a new issue
            var newId = 1;
            if (_issues.Count > 0)
            {
                newId = _issues.Max(i => i.Id) + 1;
            }

            var issue = new Issue { Id = newId, Title = "New Issue" };
            _issues.Add(issue);

            //Save in the background
            SaveIssuesAsync();

            return issue;
        }

        public void Delete(int id)
        {
            var removeIssue = _issues.FirstOrDefault(i => i.Id == id);
            if (removeIssue == null) return;
            _issues.Remove(removeIssue);
            SaveIssuesAsync();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Check for the existence of the file
        /// </summary>
        private async Task<bool> FileExists()
        {
#if __WASM__
            //WebAssembly has an issue where the file may be missing at first, but then appear
            //Here we keep trying to check for the file for up to two seconds

            for (var i = 0; i < 40; i++)
            {
                await Task.Delay(50);
                if (File.Exists(LocalSettingsKey)) return true;
            }

            return false;
#else
            return File.Exists(LocalSettingsKey);
#endif
        }
        #endregion
    }
}
