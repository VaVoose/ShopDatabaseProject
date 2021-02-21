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
    public sealed partial class MachinesPage : Page
    {
        private IList<Machines> MachineList;
        private ObservableCollection<Machines> Machines;

        public MachinesPage()
        {
            this.InitializeComponent();
            refreshGrid();
        }

        private void refreshGrid()
        {
            Machines = DataAccess.GetMachineList();

            MachinesOutput.ItemsSource = Machines;

            MachineList = new List<Machines>(Machines);
        }

        private void AddMachine(object sender, RoutedEventArgs e)
        {
            if (txtNewMachineName.Text != "") {
                if (DataAccess.AddMachine(txtNewMachineName.Text)) {
                    refreshGrid();
                    txtNewMachineName.Text = "";
                    return;
                }
                txtNewMachineName.Text = "No Duplicate Machine Names";
            }
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

        private void machineFilterChanged(object sender, TextChangedEventArgs e)
        {
            List<Machines> TempFiltered;

            TempFiltered = MachineList.Where(contact => contact.machineName.Contains(FilterByMachineName.Text, StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = Machines.Count - 1; i >= 0; i--)
            {
                var item = Machines[i];
                if (!TempFiltered.Contains(item))
                {
                    Machines.Remove(item);
                }
            }

            foreach (var item in TempFiltered)
            {
                if (!Machines.Contains(item))
                {
                    Machines.Add(item);
                }
            }

        }
    }
}
