using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB
{
    class NewUserPageParameters
    {
        public string userID;
        public bool admin;
        public NewUserPageParameters(string id, bool isAdminCreating) {
            userID = id;
            admin = isAdminCreating;
        }
    }
}
