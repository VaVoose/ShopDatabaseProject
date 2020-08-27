using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setPermissions();
            Console.WriteLine(CurrentUser.getUsername() + CurrentUser.getMRP() + CurrentUser.getITRP() + CurrentUser.getTP() + CurrentUser.getAP() + CurrentUser.getSAP());
        }

        // This overrides the red 'X' button to shutdown the application without having to close the previous window
        // This should be in every windows code
        // This does not work because it runs when the window is closed but when logging out the window needs to be closed because the user permissions are cleared
        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.Shutdown();
        //}

        // Wayne - Here you can set the permissions for what users should be able to do in this screen
        // Change everything to be default enabled to be false and set true based on certain permissions
        // Example - if (currentUser.getAP()){miNew.isEnabled = true}
        private void setPermissions() {

        }

        private void FileNewClick(object sender, RoutedEventArgs e)
        {

        }

        private void FileOpenClick(object sender, RoutedEventArgs e)
        {

        }

        private void FileSaveClick(object sender, RoutedEventArgs e)
        {

        }

        private void FileExitClick(object sender, RoutedEventArgs e)
        {

        }

        private void ToolsModifyClick(object sender, RoutedEventArgs e)
        {

        }

        private void ToolsBlueprintClick(object sender, RoutedEventArgs e)
        {

        }

        private void ToolsSaveClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            ZoneWindow zw = new ZoneWindow(1);
            zw.Owner = this;
            zw.Show();
        }

        private void OptionsModifyLogin_Click(object sender, RoutedEventArgs e)
        {
            ModifyLoginWindow mlw = new ModifyLoginWindow();
            mlw.Owner = this;
            mlw.ShowDialog();
        }

        private void OptionsLogOut_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.clearUser();
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }

        private void BtnCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            mm.Owner = this;
            mm.ShowDialog();
        }

        private void MiModifyParts_Click(object sender, RoutedEventArgs e)
        {
            ModifyPartsWindow mpw = new ModifyPartsWindow();
            mpw.Owner = this;
            mpw.ShowDialog();
        }

        private void MiExit2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
