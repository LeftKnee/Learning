using System;
using Windows.UI.Core;

namespace IssueTracker.Services
{
    public interface INavigationService
    {
        void Navigate(Type sourcePageType, object parameter, AppViewBackButtonVisibility appViewBackButtonVisibility);
    }
}
