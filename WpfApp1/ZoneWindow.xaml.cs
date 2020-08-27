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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Microsoft.Win32;
using System.Drawing;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ZoneWindow.xaml
    /// </summary>
    public partial class ZoneWindow : Window
    {
        public int roomNumber;
        String imageFilePath, documentFilePath;
        public Byte[] imageByteArray;
        public SqlConnection sqlCon;

        // Constructor for the Zone Window
        // Param zoneID: The id of the zone that the information will be queried from
        public ZoneWindow(int zoneID)
        {
            InitializeComponent();
            //If there is some way to pass in data to the new window the title can be dynamically set here so that
            //it displays the proper zone name
            //The zone data id is also required for the query below to get accurate results
            this.Title = "Zone " + zoneID;
            roomNumber = zoneID;
            bindDataGrid();
        }

        // This overrides the red 'X' button to shutdown the application without having to close the previous window
        // This should be in every windows code
        // This should not be used here since the user may want to have multiple windows open with different zones on them so closing out of one will exit the whole program
        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.Shutdown();
        //}

        // -------------This is an example of how to bind data to a data grid --------------- //
        // -------------                  Use it for reference                --------------- //
        private void bindDataGrid() {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.ConnectionString += ";Connection Timeout=30";

            System.Windows.Forms.MessageBox.Show("Connecting to Database...");

            int retries = 0;
            while (true) {
                try {
                    sqlCon.Open();
                    break;
                }
                catch (System.Data.SqlClient.SqlException) {
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

            // --- create stored proc for this --- //
            cmd.CommandText = "SELECT * FROM [parts] WHERE roomID = " + roomNumber;
            //Sets the commands connection
            cmd.Connection = sqlCon;
            //cmd.Parameters.AddWithValue("@roomNumber", roomNumber);
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

        // This function updates when a new row is selected from the data grid
        // This function will be used to update the picture and the details that are saved with each part
        private void DgParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Gets the datagrid that is being changed
            DataGrid gd = (DataGrid)sender;
            //Gets all the data from the row that is selected
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            //If the row is not null
            if (row_selected != null) {
                //changed the text of the text box to the the part number
                //"partNo" is the column header of the specific column that is used in the DATABASE (not set in the program)
                txtTest.Text = row_selected["partNo"].ToString();
            }

            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.Open();
            //Instantiates a new sql command string
            SqlCommand cmd = new SqlCommand();
            DataRowView rowSelected = dgParts.SelectedItem as DataRowView;
            string strCurrentRowID = rowSelected["ID"].ToString();
            //This is where you write your query to populate the table
            //You can write any kind of query here

            // This is fine sercurity wise because it uses a hard coded ID which can not be editted by the user
            cmd.CommandText = "SELECT * FROM [partDocuments] WHERE partID = " + strCurrentRowID ;

            //Sets the commands connection
            cmd.Connection = sqlCon;

            //Creates a new SQL Data Adapter (not sure what this does)
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Creates a new Data Table
            DataTable dtbl = new DataTable("Documents");
            //Fills the data adapter with the information in the data table
            da.Fill(dtbl);

            //Sets the xaml data grid to display the data adapted table
            lvDetails.ItemsSource = dtbl.DefaultView;
            gvcType.DisplayMemberBinding = new Binding("ID");
            gvcText.DisplayMemberBinding = new Binding("documentText");

            //Setting the imgPartPic to be the image assigned to the part
            //If there is an image for the part
            if (row_selected["image"] != DBNull.Value)
            {
                //Get the byte array of the part
                byte[] imageArray = (byte[])row_selected["image"];
                //Set the part to the image
                BitmapImage partImage = ConvertImageByteToImage(imageArray);
                imgPartPic.Source = partImage;
            }
            else {
                imgPartPic.Source = null;
            }
            sqlCon.Close();
        }

        //Converts the byte array of an image stored in the database and converts it to a wpf image source
        //To be honest just googled it and this is what came up - not sure how it works
        private BitmapImage ConvertImageByteToImage(byte[] imageArray) {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageArray))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        //Opens dialog box to select a picture to be added
        private void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.png)|*.png; *.jpg";
            //Currently assumes valid file
            ofd.ShowDialog();
            imageFilePath = ofd.FileName;
            lblFileName.Content = System.IO.Path.GetFileName(imageFilePath);
        }

        // Stores the selected image in binary format to the database
        private void BtnSubmitPic_Click(object sender, RoutedEventArgs e)
        {
            //Instantiates a Connection String
            SqlConnection sqlCon = new SqlConnection();
            //Sets the connection string to point to the master connection set in "App.config"
            sqlCon.ConnectionString = ConfigurationManager.ConnectionStrings["masterConnection"].ConnectionString;
            sqlCon.Open();

            // Creates a temporary image to store the image file
            System.Drawing.Image temp = new Bitmap(imageFilePath);
            // new memory stream to store the image
            MemoryStream strm = new MemoryStream();
            temp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
            //Creates a byte array to store the image data
            imageByteArray = strm.ToArray();
            //Uses a stored proc to add the image to DB
            SqlCommand sqlCmdAddImage = new SqlCommand("sp_ImageAddOrEdit", sqlCon) { CommandType = CommandType.StoredProcedure };
            //Gets the current row selected
            DataRowView rowSelected = dgParts.SelectedItem as DataRowView;
            string strCurrentRowID = rowSelected["ID"].ToString();

            //Prints bytearray for debug purposes
            //foreach (int i in imageByteArray)
            //{
            //    Console.Write(imageByteArray[i]);
            //}

            // Sets parameters for stored procs and executes query
            sqlCmdAddImage.Parameters.AddWithValue("@ID", strCurrentRowID);
            sqlCmdAddImage.Parameters.AddWithValue("@image", imageByteArray);

            sqlCmdAddImage.ExecuteNonQuery();
            sqlCon.Close();
        }

        //Opens a file dialog to select the document to be added to the part
        private void BtnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Document (*.pdf, *.txt, *.doc, *.docx)|*.pdf; *.txt; *.doc; *.docx";
            //Currently assumes valid file
            ofd.ShowDialog();
            documentFilePath = ofd.FileName;
            lblDocumentName.Content = System.IO.Path.GetFileName(documentFilePath);
        }

        // Submits the text as a new "document" in the listview
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
