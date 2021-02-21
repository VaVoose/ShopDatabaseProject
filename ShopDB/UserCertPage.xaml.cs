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
    public sealed partial class UserCertPage : Page
    {
        /**
         * Everytime the page is loaded initialize the page and popualte the certifications of the specific user
         */
        public UserCertPage()
        {
            this.InitializeComponent();

            txtName.Text = "Current User: " + CurrentUser.firstname + " " + CurrentUser.lastname + " - " + CurrentUser.id ;
            UserCertOutput.ItemsSource = DataAccess.GetUserCeritications(CurrentUser.id);

        }

        /**
         * This is the back button
         * it removes all data stored in current user and navigates back to the main menu
         */
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            CurrentUser.Reset();
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
