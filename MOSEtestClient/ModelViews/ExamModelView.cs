using MOSEtestClient.Dao;
using MOSEtestClient.Models;
using MOSEtestClient.Utilities;
using MOSEtestClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MOSEtestClient.ModelViews
{
    public class ExamModelView : INotifyPropertyChanged
    {
        public AppConfigDao appConfigDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        public QuestionDao questionDao
        {
            get
            {
                return new QuestionDao();
            }
        }

        public Profile profile
        {
            get
            {
                return appConfigDao.read();
            }
        }

        private Visibility _isSpin = Visibility.Collapsed;

        public Visibility isSpin
        {
            get { return _isSpin; }
            set
            {
                _isSpin = value;
                this.NotifyPropertyChanged("isSpin");
            }
        }

        public DelegateCommand<object> loginPageCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {


                    isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                       
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var r = Application.Current.Windows.OfType<Window>().FirstOrDefault();

                            if (r != null)
                            {
                                InstructionView ev = new InstructionView();
                                ev.DataContext = this;
                                ev.Show();
                                r.Close();

                            }
                        }));
                    });

                    isSpin = Visibility.Collapsed;
                });
            }
        }
        private LoginModel _loginModel = new LoginModel();

        public LoginModel loginModel
        {
            get { return _loginModel; }
            set
            {
                _loginModel = value;
                this.NotifyPropertyChanged("loginModel");
            }
        }

        private Question _currentQuestion = new Question();

        public Question currentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                this.NotifyPropertyChanged("currentQuestion");
            }
        }


        public DelegateCommand<object> loadExamPage
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        this.questions = new ObservableCollection<Question>(questionDao.Questions(profile.questionCount));

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var r = Application.Current.Windows.OfType<Window>().FirstOrDefault();

                            if (r != null)
                            {
                                ExamView ev = new ExamView();
                                ev.DataContext = this;
                                ev.Show();
                                r.Close();
                            }
                        }));
                    });

                    isSpin = Visibility.Collapsed;
                });
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        if (questions.Count > 0)
                        {
                            selectedQuestion = questions[0];
                        }
                    });
                });
            }
        }

        public DelegateCommand<object> previousCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {

                    });
                });
            }
        }

        public DelegateCommand<object> nextCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {

                    });
                });
            }
        }

        private Question _selectedQuestion = new Question();

        public Question selectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                this.NotifyPropertyChanged("selectedQuestion");
            }
        }


        ObservableCollection<Question> _questions = new ObservableCollection<Question>();
        public ObservableCollection<Question> questions
        {
            get
            {

                return _questions;
            }
            set
            {
                _questions = value;
                this.NotifyPropertyChanged("questions");
            }
        }

        public List<int> QuestionCount
        {
            get
            {
                return Common.GeneratCount(profile.questionCount);
            }
        }

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        public double QWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth - 220;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
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
