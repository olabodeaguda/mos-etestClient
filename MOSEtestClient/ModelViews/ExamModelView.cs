using MOSEtestClient.Dao;
using MOSEtestClient.Models;
using MOSEtestClient.Services;
using MOSEtestClient.Utilities;
using MOSEtestClient.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MOSEtestClient.ModelViews
{
    public class ExamModelView : INotifyPropertyChanged
    {
        #region LoginModel

        public LoginService loginService
        {
            get
            {
                return new LoginService();
            }
        }

        private bool _isEnabled = false;

        public bool isEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (value)
                {
                    isVisibleRefresh = Visibility.Collapsed;
                }
                else
                {
                    isVisibleRefresh = Visibility.Visible;
                }
                this.NotifyPropertyChanged("isEnabled");
            }
        }

        private Visibility _isVisibleRefresh = Visibility.Collapsed;
        public Visibility isVisibleRefresh
        {
            get
            {
                return _isVisibleRefresh;
            }
            set
            {
                _isVisibleRefresh = value;
                this.NotifyPropertyChanged("isVisibleRefresh");
            }
        }

        public DelegateCommand<object> loginLoadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    try
                    {
                        isSpin = Visibility.Visible;
                        bool result = await loginService.PingConnection();
                        if (result)
                        {
                            isEnabled = true;
                            // this.isVisibleRefresh = Visibility.Collapsed;
                        }
                        else
                        {
                            //this.isVisibleRefresh = Visibility.Visible;
                            isEnabled = false;
                            throw new Exception("Connection to the server failed. Please refresh to try again or contact administrator");
                        }

                        isSpin = Visibility.Collapsed;
                    }
                    catch (Exception x)
                    {
                        isSpin = Visibility.Collapsed;
                        isEnabled = false;
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        #endregion

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

        private Profile _profile = new Profile();

        public Profile profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
                this.NotifyPropertyChanged("profile");
            }
        }

        private string _pTime = "00:00:00";

        public string pTime
        {
            get { return _pTime; }
            set
            {
                _pTime = value;
                this.NotifyPropertyChanged("pTime");
            }
        }
        private DispatcherTimer timer;
        private double currentTime;

        public double CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
                this.NotifyPropertyChanged("CurrentTime");
            }
        }

        private void StartTimer(double initial)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            timer.Tick += new EventHandler(timer_Tick);
            CurrentTime = initial;
            timer.Start();
        }

        private bool _enableStartExam = true;

        public bool enableStartExam
        {
            get { return _enableStartExam; }
            set
            {
                _enableStartExam = value;
                if (value)
                {
                    enableSubmitExam = false;
                }
                else
                {
                    enableSubmitExam = true;
                }
                this.NotifyPropertyChanged("enableStartExam");
            }
        }

        private bool _enableSubmitExam = false;

        public bool enableSubmitExam
        {
            get { return _enableSubmitExam; }
            set
            {
                _enableSubmitExam = value;
                this.NotifyPropertyChanged("enableSubmitExam");
            }
        }

        public DelegateCommand<object> StartCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    MessageBoxResult msg = MessageBox.Show("You are about to start exam, are you sure?. Click \"YES\" to continue or \"NO\" to cancel.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (msg == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (questions.Count > 0)
                    {
                        Question q = questions[0];
                        q.index = 0;
                        selectedQuestion = q;
                    }
                    enableStartExam = false;

                    StartTimer(300);
                });
            }
        }//SubmitCommand

        public SubmitExamService submitExamService
        {
            get
            {
                return new SubmitExamService();
            }
        }

        public DelegateCommand<object> SubmitCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    MessageBoxResult msg = MessageBox.Show("You are about to submit, are you sure?. Click \"YES\" to continue or \"NO\" to cancel.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (msg == MessageBoxResult.No)
                    {
                        return;
                    }
                    //contact remote and submit data
                    try
                    {
                        SubmitModel submitmodel = new SubmitModel();
                        submitmodel.username = profile.username;
                        submitmodel.submisionDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        submitmodel.userId = profile.id;
                        List<AnswerModel> lst = new List<AnswerModel>();
                        foreach (var tm in questions)
                        {
                            if (tm.q_type == null || tm.q_type.Id == 0)
                            {
                                continue;
                            }
                            AnswerModel ansmodel = new AnswerModel();
                            ansmodel.id = tm.id;
                            ansmodel.optionType = tm.q_type.optionType;
                            ansmodel.answer = tm.q_type.Id.ToString();
                            lst.Add(ansmodel);
                        }
                        submitmodel.answers = lst.ToArray();

                        bool result = await submitExamService.SubmitExam(submitmodel);
                        if (result)
                        {
                            MessageBox.Show("Your exam have been submit successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var r = Application.Current.Windows.OfType<Window>().FirstOrDefault();

                                if (r != null)
                                {
                                    LoginView ev = new LoginView();
                                    ev.DataContext = new ExamModelView();
                                    ev.Show();
                                    r.Close();
                                }
                            }));
                        }
                        else
                        {
                            //save to db
                            ResultBackUp resultBUp = new ResultBackUp();
                            resultBUp.username = submitmodel.username;
                            resultBUp.datecreated = DateTime.Now.ToString("dd-MM-yyyy HH mm:ss");
                            string jsonEqui = JsonConvert.SerializeObject(submitmodel);
                            resultBUp.content = Convert.ToBase64String(Encoding.ASCII.GetBytes(jsonEqui));
                            resultBUp.id = submitmodel.userId;
                            //save to db
                            bool res = questionDao.updateBackUp(resultBUp);
                            if (res)
                            {
                                MessageBox.Show("Submission can not reach remote server.Result have been archive for easy assess ", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Please try again. submission was not successfull", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CurrentTime -= 1;

            if (CurrentTime == 300)
            {
                MessageBox.Show("you have 5 mins left", "Time is running off", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            TimeSpan time = TimeSpan.FromSeconds(CurrentTime);
            pTime = time.ToString(@"hh\:mm\:ss");
            this.NotifyPropertyChanged("pTime");
            this.NotifyPropertyChanged("CurrentTime");
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

        private Visibility _enableArchive = Visibility.Collapsed;

        public Visibility enableArchive
        {
            get { return _enableArchive; }
            set
            {
                _enableArchive = value;
                this.NotifyPropertyChanged("enableArchive");
            }
        }

        public DelegateCommand<object> submitArchive
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    MessageBoxResult msg = MessageBox.Show("You are about to submit, are you sure?. Click \"YES\" to continue or \"NO\" to cancel.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (msg == MessageBoxResult.No)
                    {
                        return;
                    }
                    isSpin = Visibility.Visible;
                    try
                    {
                        bool result = await submitExamService.SubmitExam(submitM);
                        if (result)
                        {
                            MessageBox.Show("Your exam have been submit successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            //archive delete
                            questionDao.deleteArchiveData(profile.username);

                            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var r = Application.Current.Windows.OfType<Window>().FirstOrDefault();

                                if (r != null)
                                {
                                    LoginView ev = new LoginView();
                                    ev.DataContext = new ExamModelView();
                                    ev.Show();
                                    r.Close();
                                }
                            }));
                        }
                        else
                        {
                            MessageBox.Show("Please try again. submission was not successfull", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        isSpin = Visibility.Collapsed;
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }

        private SubmitModel _submitM = null;

        public SubmitModel submitM
        {
            get { return _submitM; }
            set
            {
                _submitM = value;
                this.NotifyPropertyChanged("submitM");
            }
        }


        public DelegateCommand<object> loginPageCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {


                    isSpin = Visibility.Visible;
                    await Task.Run(async () =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(loginModel.username))
                            {
                                isSpin = Visibility.Collapsed;
                                throw new Exception("Username is required");
                            }
                            else if (string.IsNullOrEmpty(loginModel.pwd))
                            {
                                isSpin = Visibility.Collapsed;
                                throw new Exception("Password is required");
                            }

                            bool p = await loginService.ValidateUser(loginModel);
                            profile = appConfigDao.read();
                            this.submitM = questionDao.getArchiveData(profile.username);
                            if (submitM != null)
                            {
                                enableArchive = Visibility.Visible;
                            }
                            isSpin = Visibility.Collapsed;

                            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
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
                        }
                        catch (Exception x)
                        {
                            isSpin = Visibility.Collapsed;
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
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

        public DelegateCommand<object> selectedIndexCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                    if (questions.Count <= 0)
                    {
                        selectedQuestion.index = -1;
                        return;
                    }

                    Question q = questions[selectedQuestion.index];
                    int index = questions.IndexOf(q);
                    q.index = index;
                    selectedQuestion = q;
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
                        //if (questions.Count > 0)
                        //{
                        //    Question q = questions[0];
                        //    q.index = 0;
                        //    selectedQuestion = q;
                        //}
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
                    this.isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        if (questions.Count > 0)
                        {
                            int currentIndex = questions.IndexOf(selectedQuestion);
                            if (currentIndex == 0)
                            {
                                return;
                            }

                            int newIndex = currentIndex - 1;

                            selectedQuestion = questions[newIndex];
                            selectedQuestion.index = newIndex;
                        }
                    });
                    this.isSpin = Visibility.Collapsed;
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
                        if (questions.Count > 0)
                        {
                            int currentIndex = questions.IndexOf(selectedQuestion);
                            if (currentIndex == (questions.Count - 1))
                            {
                                return;
                            }

                            int newIndex = currentIndex + 1;

                            selectedQuestion = questions[newIndex];
                            selectedQuestion.index = newIndex;
                        }
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
