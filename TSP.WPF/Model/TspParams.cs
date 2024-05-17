using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TSP.GA.Model;

namespace TSP.WPF.Model
{
    class TspParams : INotifyPropertyChanged, IDisposable
    {
        private readonly MainWindow _window;
        private readonly Canvas _canvas;

        #region fields & properties

        private int _pathlength = 20;
        private int _populationsize = 5000;
        private int _iterations = 2000;
        private int _randomSeed = 0;

        private List<System.Drawing.Point> _path = new List<System.Drawing.Point>();
        private string _status = "Ready.";
        private string _ellapsed;
        private int _bestDistance;
        private int _currentIteration;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _isBusy = false;
        private DateTime _started;

        public TspParams(MainWindow window)
        {
            _window = window;
            _canvas = _window.PathCanvas;
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isBusy = false;
            Status = "Ready.";
        }

        public int Pathlength
        {
            get => _pathlength;
            set
            {
                _pathlength = value;
                RaisePropertyChanged("Pathlength");
            }
        }

        public int Populationsize
        {
            get => _populationsize;
            set
            {
                _populationsize = value;
                RaisePropertyChanged("Populationsize");
            }
        }

        public int Iterations
        {
            get => _iterations;
            set
            {
                _iterations = value;
                RaisePropertyChanged("Iterations");
            }
        }

        public int RandomSeed
        {
            get => _randomSeed;
            set
            {
                _randomSeed = value;
                RaisePropertyChanged("RandomSeed");
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public int BestDistance
        {
            get => _bestDistance;
            set
            {
                _bestDistance = value;
                RaisePropertyChanged("BestDistance");
            }
        }

        public int CurrentIteration
        {
            get => _currentIteration;
            set
            {
                _currentIteration = value;
                RaisePropertyChanged("CurrentIteration");
            }
        }

        public string Ellapsed
        {
            get => _ellapsed;
            set
            {
                _ellapsed = value;
                RaisePropertyChanged("Ellapsed");
            }
        }


        #endregion fields & properties

        #region Generate path Command

        private RelayCommand _generatePathCommand;

        public ICommand GeneratePathCommand
        {
            get
            {
                if (_generatePathCommand == null)
                {
                    _generatePathCommand = new RelayCommand(param => GeneratePath());
                }
                return _generatePathCommand;
            }
        }

        private void GeneratePath()
        {
            Random rnd = new Random(RandomSeed++);
            int width = (int)_canvas.ActualWidth;
            int height = (int)_canvas.ActualHeight;
            _path.Clear();
            for (int i = 0; i < _pathlength; i++)
            {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, height);
                _path.Add(new System.Drawing.Point(x, y));
            }

            _window.DrawPath(_path);
        }


        #endregion Generate path Command

        #region Start Command

        private RelayCommand _startCommand;

        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(param => Start(), x => !_isBusy);
                }
                return _startCommand;
            }
        }

        private async Task Start()
        {
            _isBusy = true;
            _started = DateTime.Now;
            _worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Population pop = new Population(_path, _populationsize, _randomSeed);
            for (CurrentIteration = 0; CurrentIteration < _iterations; CurrentIteration++)
            {
                pop.Next();
                BestDistance = (int) pop.Best.Length;
                Ellapsed = (DateTime.Now - _started).ToString(@"hh\:mm\:ss");
                
                _worker.ReportProgress(0, pop.Best.Points);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _window.DrawPath((List<System.Drawing.Point>)e.UserState);
        }


        #endregion Start Command


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            _worker?.Dispose();
        }
    }
}
