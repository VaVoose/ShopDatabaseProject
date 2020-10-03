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
    public sealed partial class MachinesPage : Page
    {
        public MachinesPage()
        {
            this.InitializeComponent();
            refreshGrid();
        }

        private void refreshGrid()
        {
            MachinesOutput.ItemsSource = DataAccess.GetMachineList();
        }

        private void AddMachine(object sender, RoutedEventArgs e)
        {
            if (txtNewMachineName.Text != "") {
                DataAccess.AddMachine(txtNewMachineName.Text);
                refreshGrid();
            }
            txtNewMachineName.Text = "";
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminPage));
        }

        private void deleteMachine(object sender, RoutedEventArgs e) {
            var itemDataContext = (sender as FrameworkElement).DataContext;
            string recordID = itemDataContext.ToString();
            DataAccess.deleteMachine(recordID);
            refreshGrid();
        }
    }
}
