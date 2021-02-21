using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class UsersPage : Page
    {
        private IList<UserList> UserList;
        private ObservableCollection<UserList> Users;

        public UsersPage()
        {
            this.InitializeComponent();
            refreshGrid();
        }

        private void refreshGrid() {

            Users = DataAccess.GetUserList();


            UsersOutput.ItemsSource = Users;

            UserList = new List<UserList>(Users);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminPage));
        }

        private void editUser(object sender, RoutedEventArgs e) {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string rowID = itemDataContext.ToString();
            this.Frame.Navigate(typeof(UserEditPage), rowID);
        }

        private void Add(object sender, RoutedEventArgs e) {

            this.Frame.Navigate(typeof(NewUserPage), txtNewUserID.Text);
        }

        private void deleteUser(object sender, RoutedEventArgs e)
        {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string recordID = itemDataContext.ToString();
            DataAccess.deleteUser(recordID);
            refreshGrid();
        }

        private void changeAdmin(object sender, RoutedEventArgs e) {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string userID = itemDataContext.ToString();
            if (userID != CurrentUser.id) {
                DataAccess.changeAdmin(userID);
            }
            refreshGrid();
        }
        
        private void lastNameFilterChanged(object sender, TextChangedEventArgs e) {
            List<UserList> TempFiltered;

            TempFiltered = UserList.Where(contact => contact.lastName.Contains(FilterByLName.Text, StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = Users.Count - 1; i >= 0; i--)
            {
                var item = Users[i];
                if (!TempFiltered.Contains(item))
                {
                    Users.Remove(item);
                }
            }

            foreach (var item in TempFiltered)
            {
                if (!Users.Contains(item))
                {
                    Users.Add(item);
                }
            }

        }

    }
}
