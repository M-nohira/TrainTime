using Prism.Mvvm;
using System.Windows.Interop;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System;
using TrainTime.Models;
using AngleSharp;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Plugin_Base;

namespace TrainTime.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public static IntPtr Handle { get; set; }

        public static Views.MainWindow Window { get; set; }

        public int _usedLib = 0;

        private string _title = "TrainTimeTableViewer";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<TrainTipViewModel> _trainpanel = new ObservableCollection<TrainTipViewModel>();
        public ObservableCollection<TrainTipViewModel> TrainPanel
        {
            get
            {
                return _trainpanel;
            }
            set
            {
                SetProperty(ref _trainpanel, value);
            }
        }

        private double left;
        public double Left
        {
            get => left;
            set
            {
                SetProperty(ref left, value);
            }
        }

        private double top;
        public double Top
        {
            get => top;
            set
            {
                SetProperty(ref top, value);
            }
        }

        public int TrainPanelLimitCount { get; set; } = 3;

        private System.Timers.Timer worker;

        public delegate Plugin_Base.TrainTimeTable setTimeTableFromWeb(bool isNextDay, bool IsHoliday);
        public List<setTimeTableFromWeb> SetTimeTableFromWeb { get; set; } = new List<setTimeTableFromWeb>();

        //public TrainTimeTable TimeTable { get; set; } = new TrainTimeTable();
        public Plugin_Base.TrainTimeTable TimeTable { get; set; }//ここまで

        public MainWindowViewModel()
        {
            LoadPlugin(@".\plugins"); //起動ファイル直下のpluginsフォルダを検索
            Initialize();

            var items = new List<ToolStripItem>();
            var item = new ToolStripMenuItem
            { Text = "EXIT" };
            item.Click += (sender, e) => { Environment.Exit(0); };

            items.Add(item);

            NotifyIcon icon = new NotifyIcon(items);

            worker = new System.Timers.Timer(500);
            worker.Enabled = true;
            worker.AutoReset = true;
            worker.Elapsed += Worker_Elapsed;
            worker.Start();

        }
        ~MainWindowViewModel()
        {
            worker.Elapsed -= Worker_Elapsed;

            worker.Dispose();
        }

        public void LoadPlugin(string pluginDirectoryPath)
        {
            if (!Directory.Exists(pluginDirectoryPath)) //存在確認と生成
                Directory.CreateDirectory(pluginDirectoryPath);

            PluginWorker pwoker = new PluginWorker(pluginDirectoryPath);
            var libs = new List<string>();
            foreach (var file in Directory.GetFiles(pluginDirectoryPath))
            {
                if (Path.GetExtension(file) == ".dll") libs.Add(file); //拡張子がdllのファイルを全取得
                continue;
            }
            if (libs.Count == 0) throw new FileNotFoundException("No Plugin file in Plugin Directory");

            List<ITrainTime> incetances = libs.SelectMany(path =>
            {
                Assembly asm = PluginWorker.LoadPlugin(path);
                return PluginWorker.CreateCommands(asm);
            }).ToList();
            
            if (incetances.Count == 0) throw new FileLoadException("Failed To Load Plugin");
            incetances.ForEach(x => SetTimeTableFromWeb.Add(x.GetTimeTable));
            //SetTimeTableFromWeb = incetances[0].GetTimeTable;
        }

        /// <summary>
        /// Set Window Location to Lower right
        /// </summary>
        /// <param name="window">window handle</param>
        private static void SetLocation(Views.MainWindow window)
        {
            if (window == null | Handle == null) return;
            W32.MoveWindow(Handle, (int)(Screen.PrimaryScreen.WorkingArea.Width - window.ActualWidth), (int)(Screen.PrimaryScreen.WorkingArea.Height - window.ActualHeight), (int)window.ActualWidth, (int)window.ActualWidth, 1);

        }

        private void Worker_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetLocation(Window);
            LimitPanelCount(TrainPanelLimitCount);
            if (TrainPanel.Count <= 0) return;
            if (TrainPanel[0].DTime > DateTime.Now) return;

            if (AddNextTrain(TrainPanel[^1].DTime, 1))
            {
                RemoveTrain(0); return;
            };

            TimeTable = Task.Run(() => SetTimeTableFromWeb[_usedLib](false, IsHoliday(DateTime.Now))).Result;

        }
        private void LimitPanelCount(int count = 3)
        {
            if (_trainpanel.Count <= count) return;
            for (var cnt = TrainPanel.Count; cnt > count; cnt--)
                RemoveTrain(cnt - 1);

        }

        private void Initialize()
        {
            TimeTable = Task.Run(() => SetTimeTableFromWeb[_usedLib](false, IsHoliday(DateTime.Now))).Result;
            AddNextTrain(DateTime.Now, TrainPanelLimitCount);
        }

        /// <summary>
        /// パネルに列車情報を表示
        /// </summary>
        /// <param name="offset">検索オフセット時間</param>
        /// <param name="number">追加個数</param>
        /// <returns></returns>
        private bool AddNextTrain(DateTime offset, int number = 1)
        {
            if (number <= 0) return true;

            foreach (var x in TimeTable.TimeTable.Where(x => x.Time > offset).OrderBy(x => x.Time).Take(number))
            {
                Invoke(() =>
                {
                    TrainPanel.Add(new TrainTipViewModel(x.StyleColor, x.Style, x.DestStationName, x.Time));
                });
            }
            return true;
        }

        private void RemoveTrain(int index)
        {
            Invoke(() => { TrainPanel.RemoveAt(index); });
        }

        private static void Invoke(Action action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
        }

        private static bool IsHoliday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday) return true; //土日判断

            switch (date.Month)
            {
                case 1:
                    switch (date.Day)
                    {
                        case 1: return true;
                    }
                    break;
                case 2:
                    switch (date.Day)
                    {
                        case 11: return true;
                        case 23: return true;
                    }
                    break;
                case 4:
                    {
                        switch (date.Day)
                        {
                            case 29: return true;
                        }
                    }
                    break;
                case 5:
                    {
                        switch (date.Day)
                        {
                            case 3: return true;
                            case 4: return true;
                            case 5: return true;
                        }
                    }
                    break;
                case 7: break;
                case 8:
                    {
                        switch (date.Day)
                        {
                            case 11: return true;
                        }

                    }
                    break;
                case 9: break;
                case 10: break;
                case 11:
                    {
                        switch (date.Day)
                        {
                            case 3: return true;
                            case 23: return true;
                        }

                    }
                    break;
                case 12: break;

            }
            return false;
        }
    }
}
