using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnoContoso.Models;
using UnoContoso.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoContoso.Views
{
    public sealed partial class CustomerDetailView : UserControl
    {
        public CustomerDetailView()
        {
            this.InitializeComponent();
        }

        public CustomerDetailViewModel ViewModel
        {
            get { return DataContext as CustomerDetailViewModel; }
        }
    }
}
