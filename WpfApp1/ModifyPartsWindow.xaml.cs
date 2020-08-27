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
    /// Interaction logic for ModifyPartsWindow.xaml
    /// </summary>
    public partial class ModifyPartsWindow : Window
    {
        private bool isRowsBeingAdded = false; //boolean to deturmine wether the ability to add rows is enabled
        private string filterString = ""; //The string that the parts are filtered by

        public ModifyPartsWindow()
        {
            InitializeComponent();
            bindDataGrid();
        }

        private void bindDataGrid() {
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
            SqlCommand cmd = new SqlCommand("sp_QueryParts", sqlCon) { CommandType = CommandType.StoredProcedure};
            //Sets the filter string
            cmd.Parameters.AddWithValue("@columnFilter", filterString);
            //Creates a new SQL Data Adapter (not sure what this does)
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Creates a new Data Table
            DataTable dtbl = new DataTable("Parts");
            //Fills the data adapter with the information in the data table
            da.Fill(dtbl);

            //Sets the xaml data grid to display the data adapted table
            dgParts.ItemsSource = dtbl.DefaultView;
            sqlCon.Close();
        }

        //Changes the state of being able to add new parts
        private void BtnAddParts_Click(object sender, RoutedEventArgs e)
        {
            if (!isRowsBeingAdded)
            {
                isRowsBeingAdded = true;
                dgParts.CanUserAddRows = true;
                btnAddParts.Content = "End Edit";
            }
            else
            {
                isRowsBeingAdded = false;
                dgParts.CanUserAddRows = false;
                btnAddParts.Content = "Add Users";
            }
        }

        //Currently a catch all situation of handling sql exceptions
        private void handleSqlException(Exception exp)
        {
            MessageBox.Show("Values not changed");
        }

        // Handles thew deletion of parts from the database
        private void BtnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.Open();
            //Instantiates a new sql command string
            SqlCommand cmd = new SqlCommand();
            DataRowView rowSelected = dgParts.SelectedItem as DataRowView;
            string strSelectedID = rowSelected["ID"].ToString();
            string strSelectedPartNo = rowSelected["PartNo"].ToString();
            //This is where you write your query to populate the table
            //You can write any kind of query here
            //Delete statements are OK for security, and its useing the ID to delete which is database created
            //Could potentially be a stored proc but i dont see the need as of now
            cmd.CommandText = "DELETE FROM [parts] WHERE ID = " + strSelectedID;
            cmd.Connection = sqlCon;
            if (MessageBox.Show("Are you sure you want to delete " + strSelectedPartNo + "?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                cmd.ExecuteNonQuery();
                bindDataGrid();
            }
            else
            {
                MessageBox.Show("Deletion Canceled");
            }
            sqlCon.Close();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        // Handles the editing and adding of parts in the database
        private void DgParts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
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
            SqlCommand cmd = new SqlCommand("sp_AddOrEditParts", sqlCon) { CommandType = CommandType.StoredProcedure }; ;

            //Sets parameters based on the new rows values
            cmd.Parameters.AddWithValue("@ID", row_Selected["ID"]);
            cmd.Parameters.AddWithValue("@partNo", row_Selected["PartNo"].ToString());
            cmd.Parameters.AddWithValue("@parentPart", row_Selected["PPNo"].ToString());
            cmd.Parameters.AddWithValue("@room", row_Selected["RN"].ToString());
            cmd.Parameters.AddWithValue("@location", row_Selected["LN"].ToString());

            MessageBox.Show("Row edit ended");
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                //Again, another catch all exception handle
                handleSqlException(exp);
            }

            sqlCon.Close();
            bindDataGrid();
        }

        // Filters based on what is entered in the text box
        // This get results in all columns
        // In the future, if needed it, can be implemented to search a specific column
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            // Sets the filter string
            filterString = txtFilter.Text;
            //Binds the data grid
            bindDataGrid();
        }

        private void DgParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
