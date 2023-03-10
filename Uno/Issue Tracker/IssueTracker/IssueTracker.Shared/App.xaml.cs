using IssueTracker.Services;
using IssueTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IssueTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        #region Fields
        private readonly IServiceCollection _services = new ServiceCollection();
        private ServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
        }
        #endregion

        #region App Events
        /// <summary>
        /// This occurs when something goes wrong. It writes to the console of the browser on WASM
        /// </summary>
        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {

            var rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Windows.UI.Xaml.Window.Current.Content = rootFrame;

                //Stand up object graph
                var navigationService = new NavigationService(rootFrame);
                _services.AddSingleton<INavigationService>(navigationService);
                _services.AddSingleton<IIssueStore, IssueStore>();
                _serviceProvider = _services.BuildServiceProvider();
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    var navigationService = _serviceProvider.GetService<INavigationService>();
                    var issueStore = _serviceProvider.GetService<IIssueStore>();
                    

                    navigationService.Navigate(
                        typeof(IssueListPage),
                        new IssueListViewModel(
                            navigationService,
                            issueStore
                            ),
                        AppViewBackButtonVisibility.Visible);
                }

                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                // Ensure the current window is active
                Windows.UI.Xaml.Window.Current.Activate();
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        #endregion
    }
}
