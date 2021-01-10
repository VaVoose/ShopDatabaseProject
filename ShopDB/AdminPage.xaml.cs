using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopDB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
        }

        private void UsersPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UsersPage));
        }

        private void MachinesPage(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(MachinesPage));
        }

        private void BackupDatabase(object sender, RoutedEventArgs e) {
            DataAccess.BackupDatabase();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            CurrentUser.Reset();
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
