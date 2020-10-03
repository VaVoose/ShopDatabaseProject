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

namespace ShopDB
{
    static class DataAccess
    {
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
        public static void AddMachine(string inputText) {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}")) {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();

                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Machine(machineName) VALUES (@entry);";
                insertCommand.Parameters.AddWithValue("@entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void CreateNewUser(string userID, string firstname, string lastname)
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
        
    }
}
