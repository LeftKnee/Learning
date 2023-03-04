using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace IssueTracker.Services
{
    public class NavigationService : INavigationService
    {
        #region Fields
        private readonly Frame _rootFrame;
        #endregion

        #region Constructor
        public NavigationService(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }
        #endregion

        #region Implementation
        public void Navigate(Type sourcePageType, object parameter, AppViewBackButtonVisibility appViewBackButtonVisibility)
        {
            _rootFrame.Navigate(sourcePageType, parameter);

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = appViewBackButtonVisibility;
        }
        #endregion
    }
}
