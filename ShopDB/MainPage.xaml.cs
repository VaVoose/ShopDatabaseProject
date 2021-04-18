using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ShopDB
{
    public sealed partial class MainPage : Page
    {
        private Regex rx = new Regex(@";[0-9]{11}\?");

        public MainPage()
        {
            
            this.InitializeComponent();
            
        }

        /**
         * Sample function, currenty not bound to any UI object
         */
        private void AddData(object sender, RoutedEventArgs e)
        {
            //DataAccess.AddMachine(Input_Box.Text);

            //Output.ItemsSource = DataAccess.GetData();
        }

        /**
         * Functions as the "Log in" button
         * Sets the CurrentUser info based on the textbox and if its a valid user moves onto the UserCertPage
         * if the user is invalid clear the text box and do nothing
         */
        private void NextPage(object sender, RoutedEventArgs e) {
            //If the user exists
            if (DataAccess.GetUserInfo(txtUserInput.Text)) {
                //If the user is not an admin, open UserCertPage
                if (CurrentUser.isAdmin == false)
                {
                    this.Frame.Navigate(typeof(UserCertPage));
                }
                //If the user is an Admin open the admin page
                else {
                    this.Frame.Navigate(typeof(AdminPage));
                }
            }
            //If the user doesn't exist
            else {
                if (int.TryParse(txtUserInput.Text, out int i)) {
                    NewUserPageParameters NUPP = new NewUserPageParameters((txtUserInput.Text).ToString(), false);
                    this.Frame.Navigate(typeof(NewUserPage), NUPP);
                }
            }
            
        }

        private void NextPage(object sender, RoutedEventArgs e, string id)
        {
            //If the user exists
            if (DataAccess.GetUserInfo(id))
            {
                //If the user is not an admin, open UserCertPage
                if (CurrentUser.isAdmin == false)
                {
                    this.Frame.Navigate(typeof(UserCertPage));
                }
                //If the user is an Admin open the admin page
                else
                {
                    this.Frame.Navigate(typeof(AdminPage));
                }
            }
            //If the user doesn't exist, clear the text field
            else
            {
                this.Frame.Navigate(typeof(NewUserPage), id);
            }

        }

        private void MatchID(object sender, KeyRoutedEventArgs e) {
            if (rx.IsMatch(txtUserInput.Text)) {
                NextPage(sender, e, txtUserInput.Text.Substring(1,9).ToString());
                //this.Frame.Navigate(typeof(NewUserPage), (txtUserInput.Text.Substring(1,9)).ToString());
            }
        }
    }
}
