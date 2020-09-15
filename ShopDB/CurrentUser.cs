using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB
{
    /**
     * Static class for storing the data of the current user that is "logged in"
     */
    static class CurrentUser
    {
        public static string id { get; set; }
        public static string firstname { get; set; }
        public static string lastname { get; set; }
        public static bool isAdmin { get; set; }

        /**
         * Resets all the data in the class
         * used for when the application is navigated back to the main menu etc.
         */
        public static void Reset() {
            id = null;
            firstname = null;
            lastname = null;
            isAdmin = false;
        }
    }
}
