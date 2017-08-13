using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MOSEtestClient.Models
{
    public class QstnOption : INotifyPropertyChanged
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                this.NotifyPropertyChanged("Id");
            }
        }

        private Guid _question_Id;

        public Guid question_Id
        {
            get { return _question_Id; }
            set
            {
                _question_Id = value;
                this.NotifyPropertyChanged("question_Id");
            }
        }

        private string _option_name;

        public string option_name
        {
            get { return _option_name; }
            set
            {
                _option_name = value;
                this.NotifyPropertyChanged("option_name");
            }
        }

        private int _isPicture;

        public int isPicture
        {
            get { return _isPicture; }
            set
            {
                _isPicture = value;
                this.NotifyPropertyChanged("isPicture");
            }
        }

        private string _picture_url;

        public string picture_url
        {
            get { return _picture_url; }
            set
            {
                _picture_url = value;
                this.NotifyPropertyChanged("picture_url");
            }
        }

        private string _pictureFullUrl;

        public string pictureFullUrl
        {
            get { return _pictureFullUrl; }
            set
            {
                _pictureFullUrl = value;
                this.NotifyPropertyChanged("pictureFullUrl");
            }
        }

        private string _optionType;

        public string optionType
        {
            get { return _optionType; }
            set
            {
                _optionType = value;
                this.NotifyPropertyChanged("optionType");
            }
        }

        private Visibility _picVisible;

        public Visibility picVisible
        {
            get { return _picVisible; }
            set
            {
                _picVisible = value;
                this.NotifyPropertyChanged("picVisible");
            }
        }

        private Visibility _txVisible;
        public Visibility txVisible
        {
            get { return _txVisible; }
            set
            {
                _txVisible = value;
                this.NotifyPropertyChanged("txVisible");
            }
        }
        private bool _chk_ans;

        public bool chk_ans
        {
            get { return _chk_ans; }
            set
            {
                _chk_ans = value;
                this.NotifyPropertyChanged("chk_ans");
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
