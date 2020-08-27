using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ModifyLoginWindow.xaml
    /// </summary>
    public partial class ModifyLoginWindow : Window
    {
        private bool isRowsBeingAdded = false; //Are new rows being added

        public ModifyLoginWindow()
        {
            InitializeComponent();
            setUseableColumns();
            bindDataGrid();
        }

        // Gets the info from CurrentUser and sets the ability to change the data table based on the users permissions
        private void setUseableColumns() {
            // Can be set visible or read only not sure which on at this time
            // Admin users can change permissions of all other groups exept other admin possitions
            // Super Admin user can change admin permissions as well
            if (CurrentUser.getAP()) {
                dgtcMRP.Visibility = Visibility.Visible;
                dgtcITRP.Visibility = Visibility.Visible;
                dgtcTP.Visibility = Visibility.Visible;
            }
            if (CurrentUser.getSAP()) {
                dgtcAP.Visibility = Visibility.Visible;
                dgtcUsername.IsReadOnly = false;
            }
        }

        private void bindDataGrid()
        {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.ConnectionString += ";Connection Timeout=30";

            int retries = 0;
            while (true)
            {
                try
                {
                    sqlCon.Open();
                    break;
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    if (++retries == 3) throw;
                    System.Windows.Forms.MessageBox.Show("Connection Failed. Retry iteration " + (retries));
                    sqlCon.Close();
                    continue;
                }
            }

            //Instantiates a new sql command string
            SqlCommand cmd = new SqlCommand();
            //This is where you write your query to populate the table
            //You can write any kind of query here

            // Take a look at this query later, OK for primary injection, but could be suceptable to secondary sql injection specifically the ELSE statement in this IF
            if (CurrentUser.getAP() || CurrentUser.getSAP()) cmd.CommandText = "SELECT * FROM [login]";
            else cmd.CommandText = "SELECT * FROM [login] WHERE username='" + CurrentUser.getUsername() + "'";

            //Sets the commands connection
            cmd.Connection = sqlCon;

            //Creates a new SQL Data Adapter (not sure what this does)
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Creates a new Data Table
            DataTable dtbl = new DataTable("Parts");
            //Fills the data adapter with the information in the data table
            da.Fill(dtbl);

            //Sets the xaml data grid to display the data adapted table
            dgLogins.ItemsSource = dtbl.DefaultView;
            sqlCon.Close();
        }

        // This was added to handle the unique key constraint exception
        // Currently it handles all exceptions but that can be changed later
        private void handleSqlException(Exception exp) {
            MessageBox.Show("Values not changed");
        }

        //Handles the ability for the user to add more users
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (!isRowsBeingAdded)
            {
                isRowsBeingAdded = true;
                dgLogins.CanUserAddRows = true;
                btnAddUser.Content = "End Edit";
            }
            else {
                isRowsBeingAdded = false;
                dgLogins.CanUserAddRows = false;
                btnAddUser.Content = "Add Users";
            }
        }

        // Handles the deletion of users
        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.Open();
            //Instantiates a new sql command string
            SqlCommand cmd = new SqlCommand();
            DataRowView rowSelected = dgLogins.SelectedItem as DataRowView;
            string strSelectedID = rowSelected["ID"].ToString();
            string strSelectedUsername = rowSelected["username"].ToString();
            //This is where you write your query to populate the table
            //You can write any kind of query here
            //Delete statements are OK for security, and its useing the ID to delete which is database created
            //Could potentially be a stored proc but i dont see the need as of now
            cmd.CommandText = "DELETE FROM [login] WHERE ID = " + strSelectedID;
            cmd.Connection = sqlCon;
            if (MessageBox.Show("Are you sure you want to delete user " + strSelectedUsername + "?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                cmd.ExecuteNonQuery();
                bindDataGrid();
            }
            else {
                MessageBox.Show("Deletion Canceled");
            }
            sqlCon.Close();
        }

        // This function runs after the row edit has ended
        // This is used for adding or editing users
        // Currently there is no validation of data
        // We need to make sure that there are no duplicate usernames added and that the usernames contain only 50 length string and no symbols like " ' "
        // This kind of vetting should probably done in the stored proc or declared somewhere server side
        private void DgLogins_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            //Console.WriteLine(grdItem);
            ////Gets all the data from the row that is selected
            DataRowView row_Selected = gd.SelectedItem as DataRowView;

            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.Open();
            //Instantiates a new sql command string
            SqlCommand cmd = new SqlCommand("sp_AddOrEditLoginPermissions", sqlCon) { CommandType = CommandType.StoredProcedure }; ;

            //Sets parameters based on the new rows values
            cmd.Parameters.AddWithValue("@ID", row_Selected["ID"]);
            cmd.Parameters.AddWithValue("@username", row_Selected["username"].ToString());
            cmd.Parameters.AddWithValue("@password", row_Selected["password"].ToString());
            cmd.Parameters.AddWithValue("@first", row_Selected["firstName"].ToString());
            cmd.Parameters.AddWithValue("@last", row_Selected["lastName"].ToString());
            
            // If the inputted value is null it returns false, this is needed because the default value when creating a new row is null
            if (row_Selected["maintenanceRecordsPermission"] == DBNull.Value) cmd.Parameters.AddWithValue("@MRP", 0);
            else cmd.Parameters.AddWithValue("@MRP", row_Selected["maintenanceRecordsPermission"]);
            if (row_Selected["itRecordsPermissions"] == DBNull.Value) cmd.Parameters.AddWithValue("@ITRP", 0);
            else cmd.Parameters.AddWithValue("@ITRP", row_Selected["itRecordsPermissions"]);
            if (row_Selected["teacherPermissions"] == DBNull.Value) cmd.Parameters.AddWithValue("@TP", 0);
            else cmd.Parameters.AddWithValue("@TP", row_Selected["teacherPermissions"]);
            if (row_Selected["adminPermissions"] == DBNull.Value) cmd.Parameters.AddWithValue("@AP", 0);
            else cmd.Parameters.AddWithValue("@AP", row_Selected["adminPermissions"]);

            MessageBox.Show("Row edit ended");
            //This statement prints all of the parameters values for debug
            //Console.WriteLine(row_Selected["ID"] + row_Selected["username"].ToString() + row_Selected["password"].ToString() + row_Selected["maintenanceRecordsPermission"] + row_Selected["itRecordsPermissions"] + row_Selected["teacherPermissions"] + row_Selected["adminPermissions"]);
            try {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp) {
                handleSqlException(exp);
            }
            
            sqlCon.Close();
            //Rebinding the datagrid is needed because if the value is inputted as null is will continue to look like its null in the grid unless updated, then it will become false
            bindDataGrid();
        }
    }
}
