using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Models
{
    public class Profile : INotifyPropertyChanged
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
        }

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

        private string _firstname;

        public string firstname
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                this.NotifyPropertyChanged("firstname");
            }
        }
        private string _lastname;

        public string lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                this.NotifyPropertyChanged("lastname");
            }
        }

        private string _surname;

        public string surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                this.NotifyPropertyChanged("surname");
            }
        }

        private string _status;

        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
                this.NotifyPropertyChanged("status");
            }
        }

        private int _questionCount;

        public int questionCount
        {
            get { return _questionCount; }
            set
            {
                _questionCount = value;
                this.NotifyPropertyChanged("questionCount");
            }
        }

        public string displayName
        {
            get
            {
                return $"Welcome {surname} {firstname} {lastname}";
            }
        }

        private string _remoteUrl;

        public string remoteUrl
        {
            get { return _remoteUrl; }
            set
            {
                _remoteUrl = value;
                this.NotifyPropertyChanged("remoteUrl");
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
