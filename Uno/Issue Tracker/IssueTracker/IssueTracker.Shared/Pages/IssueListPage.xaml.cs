using IssueTracker.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IssueTracker
{

    public sealed partial class IssueListPage : Page
    {
        public IssueListPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var bugListViewModel = (IssueListViewModel)e.Parameter;
            DataContext = bugListViewModel;

            //Save issues in the background (no need to await)
            bugListViewModel.SaveAsync();

            bugListViewModel.Refresh();

            base.OnNavigatedTo(e);
        }
    }
}
