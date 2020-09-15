using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB
{
    /**
     * Class that contains information about user certifications
     * INotifyPropertyChanged is used for XAML data binding
     */
    public class UserCerts : INotifyPropertyChanged
    {
        public string recordID { get; set; }
        public string userID { get; set; }
        public string machineName { get; set; }
        public string machineID { get; set; }
        public string certDate { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
