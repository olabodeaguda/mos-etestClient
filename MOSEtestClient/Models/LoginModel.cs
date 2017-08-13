using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
        private string _username;

        public string username
        {
            get { return _username; }
            set
            {
                _username = value;
                this.NotifyPropertyChanged("username");
            }
        }
        private string _password;

        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                this.NotifyPropertyChanged("password");
            }
        }

        #region property change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
