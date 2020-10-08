using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
//Singleton

namespace RMAPPV2
{

    public class Clock : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _date = DateTime.Now.ToShortDateString();
        private string _time = DateTime.Now.ToShortTimeString();
        private bool _isServerOnline = false;

        Clock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += Tickevent;
            timer.Start();
        }

        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    RaisePropertyChanged("Date");
                }
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    RaisePropertyChanged("Time");
                }
            }
        }


        public bool IsServerOnline
        {
            get { return _isServerOnline; }
            set
            {
                if (_isServerOnline != value)
                {
                    _isServerOnline = value;
                    RaisePropertyChanged("IsServerOnline");
                }
            }
        }


        private void Tickevent(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToShortTimeString();
            IsServerOnline = ServerConnection.IsServerOnline();
            CheckIdleUser();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static Clock Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private void CheckIdleUser()
        {
            var idleTime = IdleUserDetector.GetIdleTimeInfo();

            if (idleTime.IdleTime.TotalSeconds >= 30)
            {
                if (!Datos.EstoyEnLoginPage)
                {
                    Datos.ResetDatos();
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
                }
            }
        }



        class Nested
        {
            static Nested()
            {
            }
            internal static readonly Clock instance = new Clock();
        }

    }
}