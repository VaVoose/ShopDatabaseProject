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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string username;
        private bool MRP, ITRP, TP, AP, SAP;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            //Test Connection
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
            // SQL command using a stored proc
            SqlCommand cmd = new SqlCommand("sp_loginCredentials", sqlCon) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

            // -------- Following is poopy code that should not be used, this is EASILY Suseptable to SQL Injection --------- //
            //This is where you write your query to populate the table
            //You can write any kind of query here
            //cmd.CommandText = "SELECT * FROM [login] WHERE username = '" + txtUsername.Text.Trim() + "' AND password = '" + txtPassword.Text.Trim() + "'";

            //Sets the commands connection
            cmd.Connection = sqlCon;
            // Creates a data adapter and a data table and fills it
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtbl = new DataTable("Login");
            da.Fill(dtbl);
            // If there is a user with the username and password
            if (dtbl.Rows.Count == 1) {
                //Get the permissions of that user from the database and bind it to the current user
                username = dtbl.Rows[0]["username"].ToString();
                MRP = (bool)dtbl.Rows[0]["maintenanceRecordsPermission"];
                ITRP = (bool)dtbl.Rows[0]["itRecordsPermissions"];
                TP = (bool)dtbl.Rows[0]["teacherPermissions"];
                AP = (bool)dtbl.Rows[0]["adminPermissions"];
                SAP = (bool)dtbl.Rows[0]["superAdminPermission"];
                CurrentUser.setPermissions(username, MRP, ITRP, TP, AP, SAP);
                //Open the mainwindow and close the log in window
                MainWindow mw = new MainWindow();
                mw.Show();
                sqlCon.Close();
                this.Close();
            }
            else {
                MessageBox.Show("Invalid Login");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
