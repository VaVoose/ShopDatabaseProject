using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    static class CurrentUser
    {
        private static string username; //{get; set;} this creates a default get and set method for the members, not sure if currently needed but could be usefull in future
        private static bool MRP;
        private static bool ITRP;
        private static bool TP;
        private static bool AP;
        private static bool SAP;

        static public void setPermissions(string un, bool m, bool i, bool t, bool a, bool s) {
            username = un;
            MRP = m;
            ITRP = i;
            TP = t;
            AP = a;
            SAP = s;
        }

        static public void clearUser() {
            username = null;
            MRP = false;
            ITRP = false;
            TP = false;
            AP = false;
            SAP = false;

        }

        static public string getUsername() {
            return username;
        }

        static public bool getMRP() {
            return MRP;
        }

        static public bool getITRP() {
            return ITRP;
        }

        static public bool getTP() {
            return TP;
        }

        static public bool getAP() {
            return AP;
        }

        static public bool getSAP() {
            return SAP;
        }

    }
}
