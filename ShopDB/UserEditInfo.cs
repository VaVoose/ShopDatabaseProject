using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB
{
    class UserEditInfo : INotifyPropertyChanged
    {
        public string rowID { get; set; }
        public string userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
