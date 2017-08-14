using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MOSEtestClient.Models
{
    public class Question : INotifyPropertyChanged
    {
        private Guid _id;

        public Guid id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
        }

        private int _subject_id;

        public int subject_id
        {
            get { return _subject_id; }
            set
            {
                _subject_id = value;
                this.NotifyPropertyChanged("subject_id");
            }
        }

        private string _question_name;

        public string question_name
        {
            get { return _question_name; }
            set
            {
                _question_name = value;
                this.NotifyPropertyChanged("question_name");
            }
        }

        private string _answer;

        public string answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                this.NotifyPropertyChanged("answer");
            }
        }

        private string _answerTranslation;

        public string answerTranslation
        {
            get { return _answerTranslation; }
            set
            {
                _answerTranslation = value;
                this.NotifyPropertyChanged("answerTranslation");
            }
        }

        private string _explanation;

        public string explanation
        {
            get { return _explanation; }
            set
            {
                _explanation = value;
                this.NotifyPropertyChanged("explanation");
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

        private string __ispicture;

        public string _ispicture
        {
            get { return __ispicture; }
            set
            {
                __ispicture = value;
                this.NotifyPropertyChanged("_ispicture");
            }
        }

        private string _pictureUrl;

        public string pictureUrl
        {
            get { return _pictureUrl; }
            set
            {
                _pictureUrl = value;
                this.NotifyPropertyChanged("pictureUrl");
            }
        }

        private string _picture_name;

        public string picture_name
        {
            get { return _picture_name; }
            set
            {
                _picture_name = value;
                this.NotifyPropertyChanged("picture_name");
            }
        }

        private string _selectOption;

        public string selectedOption
        {
            get { return _selectOption; }
            set
            {
                _selectOption = value;
                this.NotifyPropertyChanged("selectedOption");
            }
        }

        public double QWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth - 220;
            }
        }

        private int _index = -1;

        public int index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (displayIndex != (value + 1))
                {
                    displayIndex = value + 1;
                }
                this.NotifyPropertyChanged("index");
            }
        }

        private int _displayIndex;

        public int displayIndex
        {
            get
            {
                return _displayIndex;
            }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                _displayIndex = value;
                if (index != (value - 1))
                {
                    index = value - 1;
                }
                this.NotifyPropertyChanged("displayIndex");
            }
        }


        public string questionTitle
        {
            get
            {
                return $"Question Number {index + 1}";
            }
        }



        private List<QstnOption> _questionoption = new List<QstnOption>();

        public List<QstnOption> questionoption
        {
            get { return _questionoption; }
            set
            {
                _questionoption = value;
                this.NotifyPropertyChanged("questionoption");
            }
        }

        private int _optionCount;

        public int optionCount
        {
            get { return _optionCount; }
            set
            {
                _optionCount = value;
                this.NotifyPropertyChanged("optionCount");
            }
        }

        private Visibility _pictureVisible = Visibility.Collapsed;

        public Visibility pictureVisible
        {
            get { return _pictureVisible; }
            set
            {
                _pictureVisible = value;
                this.NotifyPropertyChanged("pictureVisible");
            }
        }

        private Visibility _txtVisible;

        public Visibility txtVisible
        {
            get { return _txtVisible; }
            set
            {
                _txtVisible = value;
                this.NotifyPropertyChanged("txtVisible");
            }
        }

        private QstnOption _q_type = new QstnOption();

        public QstnOption q_type
        {
            get { return _q_type; }
            set
            {
                _q_type = value;
                this.NotifyPropertyChanged("q_type");
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
