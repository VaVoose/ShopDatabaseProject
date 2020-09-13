using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Microsoft.Data.Sqlite;

namespace ShopDB
{
    static class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("machineCerts.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "machineCerts.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}")) {
                db.Open();

                String createUserTable = "" +
                    "CREATE TABLE IF NOT EXISTS User" +
                    "(id          int NOT NULL ," +
                    "fName        varchar(45) NOT NULL ," +
                    "lName        varchar(45) NOT NULL ," +
                    "isAdmin      bit NOT NULL ," +
                    "PRIMARY KEY(id)" +
                    ");";

                String createMachineTable = "" +
                    "CREATE TABLE IF NOT EXISTS Machine" +
                    "(machineName  varchar(45) NOT NULL ," +
                    "PRIMARY KEY (machineName)" +
                    ");";

                String createCertifiedTable = "" +
                    "CREATE TABLE IF NOT EXISTS Certified" +
                    "(id          int NOT NULL, " +
                    "userID       int NOT NULL, " +
                    "machineID    int NOT NULL, " +
                    "dateAquired  datetime NOT NULL, " +
                    "PRIMARY KEY (id, userID, machineID), " +
                    "FOREIGN KEY (userID) REFERENCES User(id) " +
                    "   ON DELETE CASCADE " +
                    "   ON UPDATE CASCADE, " +
                    "FOREIGN KEY (machineID) REFERENCES Machine(machineID) " +
                    "   ON DELETE CASCADE " +
                    "   ON UPDATE CASCADE " +
                    ");";

                SqliteCommand makeUserTable = new SqliteCommand(createUserTable, db);
                SqliteCommand makeMachineTable = new SqliteCommand(createMachineTable, db);
                SqliteCommand makeCertifiedTable = new SqliteCommand(createCertifiedTable, db);

                SqliteCommand dropMachineTable = new SqliteCommand("DROP TABLE Machine", db);

                dropMachineTable.ExecuteReader();

                makeUserTable.ExecuteReader();
                makeMachineTable.ExecuteReader();
                makeCertifiedTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText) {
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

    }
}
