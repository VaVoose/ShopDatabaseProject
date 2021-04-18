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
    public sealed partial class NewUserPage : Page
    {

        private string userID;
        private bool admin;

        public NewUserPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = (NewUserPageParameters)e.Parameter; // get parameter
            userID = param.userID;
            admin = param.admin;
            tbUserID.Text = userID;
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.GoBack();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtFirstName.Text) || String.IsNullOrEmpty(txtLastName.Text)) {
                tbUserID.Text = "FAILED: Names are required";
                return;
            }
            if (DataAccess.CreateNewUser(userID, txtFirstName.Text, txtLastName.Text))
            {
                if (DataAccess.GetUserInfo(userID))
                {
                    if (admin)
                    {
                        this.Frame.Navigate(typeof(UserEditPage), userID);
                    }
                    else {
                        this.Frame.Navigate(typeof(UserCertPage));
                    }
                    
                }
            }
            else {
                tbUserID.Text = "FAILED: ID is not unique";
            }
            
        }
    }
}
