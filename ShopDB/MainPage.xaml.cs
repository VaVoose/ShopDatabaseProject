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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShopDB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            if (DataAccess.GetUserInfo(txtUserInput.Text))
            {
                this.Frame.Navigate(typeof(UserCertPage));
            }
            else {
                txtUserInput.Text = "";
            }
            
        }
    }
}
