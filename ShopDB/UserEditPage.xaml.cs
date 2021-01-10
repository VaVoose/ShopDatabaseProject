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
    public sealed partial class UserEditPage : Page
    {

        private string userIDEditing;
        public UserEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            userIDEditing = (e.Parameter).ToString(); // get parameter
            setInfo(userIDEditing);
        }

        private void setInfo(string r) {
            UsersCertOutput.ItemsSource = DataAccess.GetUserCeritications(r);
            UsersEditInfoOutput.ItemsSource = DataAccess.GetUserEditInfo(r);
        }

        private void Back(object sender, RoutedEventArgs e) {
            this.Frame.GoBack();
        }

        private void reup(object sender, RoutedEventArgs e) {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string rowID = itemDataContext.ToString();
            DataAccess.reCertify(rowID);
            setInfo(userIDEditing);
            
        }

        private void deleteCert(object sender, RoutedEventArgs e) {
            return;
        }

        private void updateFirstName(object sender, RoutedEventArgs e) {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string rowID = itemDataContext.ToString();
            //DataAccess.updateFirstName( rowID);
            return;
        }
    }
}
