using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Microsoft.Data.Sqlite;
using Windows.UI.Xaml.Controls.Maps;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Diagnostics;
using Windows.Networking.Connectivity;
using Windows.Storage.Pickers;

namespace ShopDB
{
    static class DataAccess
    {
        

        public async static void BackupDatabase() {

            StorageFile dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("machineCerts.db");
            
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);

                StorageFile newFile = await folder.CreateFileAsync("tempbackup.bk");
                await dbFile.CopyAndReplaceAsync(newFile);

                await newFile.RenameAsync("machineCerts-" + DateTime.UtcNow.Date.ToString("MM-dd-yyyy") + "db.bk", NameCollisionOption.ReplaceExisting);

            }
            else
            {
                Console.WriteLine("It didn't work");
            }
            
        }

        public async static void RestoreDatabase() {
            StorageFile oldDBFile = await ApplicationData.Current.LocalFolder.GetFileAsync("machineCerts.db");

            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filePicker.FileTypeFilter.Add(".bk");
            StorageFile newDBFile = await filePicker.PickSingleFileAsync();

            if (newDBFile != null)
            {
                await newDBFile.CopyAndReplaceAsync(oldDBFile);
                //File.Move(newDBFile, oldDBFile);
            }
            else {
                Console.WriteLine("It didnt work");
            }

        }

        /*
            This function Initalizes the database for use in the program 
        */
        public async static void InitializeDatabase()
        {
            //Asyncronously open the machineCerts database if it is there, create it if it is not
            await ApplicationData.Current.LocalFolder.CreateFileAsync("machineCerts.db", CreationCollisionOption.OpenIfExists);
            //Create a string to the path of the database
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            //Establish Connection
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}")) {
                db.Open();

                //Query strings to create needed tables 
                String createUserTable = "" +
                    "CREATE TABLE IF NOT EXISTS User" +
                    "(userID      int NOT NULL ," +
                    "fName        varchar(45) NOT NULL ," +
                    "lName        varchar(45) NOT NULL ," +
                    "isAdmin      bit NOT NULL ," +
                    "PRIMARY KEY(userID)" +
                    ");";

                String createMachineTable = "" +
                    "CREATE TABLE IF NOT EXISTS Machine" +
                    "(machineName  varchar(45) NOT NULL ," +
                    "PRIMARY KEY (machineName)" +
                    ");";

                String createCertifiedTable = "" +
                    "CREATE TABLE IF NOT EXISTS Certified" +
                    "(userID       int NOT NULL, " +
                    "machineID     int NOT NULL, " +
                    "dateAquired   datetime NOT NULL, " +
                    "PRIMARY KEY (userID, machineID), " +
                    "FOREIGN KEY (userID) REFERENCES User(userID) " +
                    "   ON DELETE CASCADE " +
                    "   ON UPDATE CASCADE, " +
                    "FOREIGN KEY (machineID) REFERENCES Machine(rowid) " +
                    "   ON DELETE CASCADE " +
                    "   ON UPDATE CASCADE " +
                    ");";

                String insertStatements = "" +
                    "Insert into User VALUES (1234, 'Dominic', 'Ferrante', 1);" +
                    "INSERT INTO Machine VALUES ('lathe');" +
                    "INSERT INTO Machine VALUES ('drill');" +
                    "INSERT INTO Machine VALUES ('testing');" +
                    "INSERT INTO Certified VALUES (1234, 1, datetime('now'));" +
                    "INSERT INTO Certified VALUES (1234, 3, datetime('now'));";

                //Makes those strings into sqlite commands
                SqliteCommand makeUserTable = new SqliteCommand(createUserTable, db);
                SqliteCommand makeMachineTable = new SqliteCommand(createMachineTable, db);
                SqliteCommand makeCertifiedTable = new SqliteCommand(createCertifiedTable, db);
                SqliteCommand makeInsertStatements = new SqliteCommand(insertStatements, db);

                /**
                 * Enable these commands if there is a needed update to the tables 
                 * these reset the tables and drop any data that is in them
                 */

                //SqliteCommand dropTable = new SqliteCommand("DROP TABLE Machine", db);
                //SqliteCommand dropTable2 = new SqliteCommand("DROP TABLE Certified", db);
                //SqliteCommand dropTable3 = new SqliteCommand("DROP TABLE User", db);
                //dropTable.ExecuteReader();
                //dropTable2.ExecuteReader();
                //dropTable3.ExecuteReader();
                // -------------------------------------------------------------------------//

                //Execute the make table statements
                makeUserTable.ExecuteReader();
                makeMachineTable.ExecuteReader();
                makeCertifiedTable.ExecuteReader();
                //makeInsertStatements.ExecuteReader();
            }
        }
        /**
         * Demo function to show how to add data to a table using C# and sqlite
         */
        public static Boolean AddMachine(string inputText) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}")) {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Machine(machineName) VALUES (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", inputText);
                try { insertCommand.ExecuteReader(); }
                catch { db.Close(); return false; }

                db.Close();
                return true;
            }
        }

        public static Boolean CreateNewUser(string userID, string firstname, string lastname)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO User VALUES (@userid, @fname, @lname, 0);";
                insertCommand.Parameters.AddWithValue("@userid", userID);
                insertCommand.Parameters.AddWithValue("@fname", firstname);
                insertCommand.Parameters.AddWithValue("@lname", lastname);
                try { insertCommand.ExecuteReader(); }
                catch {
                    db.Close();
                    return false;
                }
                
                db.Close();
                return true;
            }
        }

        public static void changeAdmin(string inputText) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = " UPDATE User SET isAdmin = ~isAdmin WHERE userID = (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void updateFirstname(string name, string rowID)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = " UPDATE User SET fName = @n WHERE rowid = (@entry);";
                insertCommand.Parameters.AddWithValue("@n", name);
                insertCommand.Parameters.AddWithValue("@entry", rowID);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void updateLastname(string name, string rowID)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = " UPDATE User SET lName = @n WHERE rowid = (@entry);";
                insertCommand.Parameters.AddWithValue("@n", name);
                insertCommand.Parameters.AddWithValue("@entry", rowID);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void deleteMachine(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Machine WHERE rowid = (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", inputText);

                insertCommand.ExecuteReader();

                SqliteCommand insertCommand2 = new SqliteCommand();
                insertCommand2.Connection = db;
                insertCommand2.CommandText = "DELETE FROM Certified WHERE machineID = (@entry);";
                insertCommand2.Parameters.AddWithValue("@entry", inputText);

                insertCommand2.ExecuteReader();


                db.Close();
            }
        }

        public static void deleteUser(string inputText) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM User WHERE userID = (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", inputText);

                insertCommand.ExecuteReader();


                SqliteCommand insertCommand2 = new SqliteCommand();

                insertCommand2.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand2.CommandText = "DELETE FROM Certified WHERE userID = (@entry);";
                insertCommand2.Parameters.AddWithValue("@entry", inputText);

                insertCommand2.ExecuteReader();

                db.Close();
            }
        }

        /**
         * Sample function to query the sqlite database and return the data list
         */
        public static List<String> GetData() {
            List<String> entries = new List<String>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}")) {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT rowid, machineName FROM Machine", db);


                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read()) {
                    entries.Add(query.GetString(0) + " - " + query.GetString(1));
                }

                db.Close();
            }

            return entries;
        }

        public static ObservableCollection<Machines> GetMachineList() {
            ObservableCollection<Machines> entries = new ObservableCollection<Machines>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectCommandString = "" +
                    "SELECT rowid, machineName FROM Machine";

                SqliteCommand selectCommand = new SqliteCommand(selectCommandString, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    Machines machine = new Machines();
                    machine.recordID = query.GetString(0);
                    machine.machineName = query.GetString(1);
                    entries.Add(machine);
                }

                db.Close();
            }
            return entries;
        }

        public static ObservableCollection<UserList> GetUserList() {
            ObservableCollection<UserList> entries = new ObservableCollection<UserList>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectCommandString = "" +
                    "SELECT rowid, userID, fName, lName, isAdmin FROM User";

                SqliteCommand selectCommand = new SqliteCommand(selectCommandString, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    UserList u = new UserList();
                    u.rowID = query.GetString(0);
                    u.userID = query.GetString(1);
                    u.firstName = query.GetString(2);
                    u.lastName = query.GetString(3);
                    u.isAdmin = query.GetBoolean(4);
                    entries.Add(u);
                }

                db.Close();
            }
            return entries;
        }

        /**
         * This function returns an observable collection of UserCerts associated with the current user
         */
        public static ObservableCollection<UserCerts> GetUserCeritications(string userInfo)
        {
            ObservableCollection<UserCerts> entries = new ObservableCollection<UserCerts>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectCommandString = "" +
                    "SELECT User.userID, Machine.machinename, Machine.rowid, Certified.dateaquired, Certified.rowid " +
                    "FROM Certified " +
                    "INNER JOIN User " +
                        "ON User.userID = Certified.userID " +
                    "INNER JOIN Machine " +
                        "ON Machine.rowid = Certified.machineID " +
                    "WHERE User.userID = @entry; ";

                SqliteCommand selectCommand = new SqliteCommand(selectCommandString, db);

                selectCommand.Parameters.AddWithValue("@entry", userInfo);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    UserCerts cert = new UserCerts();
                    cert.userID = query.GetString(0);
                    cert.machineName = query.GetString(1);
                    cert.machineID = query.GetString(2);
                    cert.certDate = query.GetString(3);
                    cert.recordID = query.GetString(4);
                    entries.Add(cert);                   
                }

                db.Close();
            }
            return entries;
        }

        public static Boolean addCertification(string UID, string MID) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Certified VALUES (@u, @m, datetime('now'));";
                insertCommand.Parameters.AddWithValue("@u", UID);
                insertCommand.Parameters.AddWithValue("@m", MID);
                try { insertCommand.ExecuteReader(); }
                catch {
                    db.Close();
                    return false;
                }
                
                db.Close();
                return true;
            }
        }

        public static void deleteCertification(string RID) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Certified WHERE rowid = (@r); ";
                insertCommand.Parameters.AddWithValue("@r", RID);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void reCertify(string input) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = " UPDATE Certified SET dateAquired = datetime('now') WHERE rowid = (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", input);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        /**
         * Sets the current user info based on the userId string parameter
         * Returns true if it is a valid user in the database
         * Returns false if there is not a user with that id in the db
         */
        public static bool GetUserInfo(string userID) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectCommandString = "" +
                    "SELECT userID, fName, lName, isAdmin " +
                    "FROM User " +
                    "WHERE userID = @entry; ";

                SqliteCommand selectCommand = new SqliteCommand(selectCommandString, db);

                selectCommand.Parameters.AddWithValue("@entry", userID);
                try
                {
                    SqliteDataReader query = selectCommand.ExecuteReader();

                    query.Read();
                    CurrentUser.id = query.GetString(0);
                    CurrentUser.firstname = query.GetString(1);
                    CurrentUser.lastname = query.GetString(2);
                    CurrentUser.isAdmin = query.GetBoolean(3);
                    db.Close();
                }
                catch (Exception) {
                    //Do Something
                    db.Close();
                    return false;
                }
                return true;
                
            }
        }

        public static ObservableCollection<UserEditInfo> GetUserEditInfo(string userInfo)
        {
            ObservableCollection<UserEditInfo> entries = new ObservableCollection<UserEditInfo>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectCommandString = "SELECT rowID, userID, fName, lName " +
                    "FROM User " +
                    "WHERE userID = @entry; ";

                SqliteCommand selectCommand = new SqliteCommand(selectCommandString, db);

                selectCommand.Parameters.AddWithValue("@entry", userInfo);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    UserEditInfo uei = new UserEditInfo();
                    uei.rowID = query.GetString(0);
                    uei.userID = query.GetString(1);
                    uei.firstName = query.GetString(2);
                    uei.lastName = query.GetString(3);
                    entries.Add(uei);
                }

                db.Close();
            }
            return entries;
        }

    }
}
