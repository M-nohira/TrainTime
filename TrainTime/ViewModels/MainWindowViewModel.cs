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
//using System.Threading;


namespace TrainTime.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public static IntPtr Handle { get; set; }

        public static Views.MainWindow Window { get; set; }

        private string _title = "TrainTimeTableViewer";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<TrainTipViewModel> trainpanel = new ObservableCollection<TrainTipViewModel>();
        public ObservableCollection<TrainTipViewModel> TrainPanel
        {
            get
            {
                return trainpanel;
            }
            set
            {
                SetProperty(ref trainpanel, value);
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


        private System.Timers.Timer worker;

        public TrainTimeTable TimeTable { get; set; } = new TrainTimeTable();

        public MainWindowViewModel()
        {
            Initialize();

            worker = new System.Timers.Timer(500);
            worker.Enabled = true;
            worker.AutoReset = true;
            worker.Elapsed += Worker_Elapsed;
            worker.Start();

        }
        ~MainWindowViewModel()
        {
            worker.Elapsed -= Worker_Elapsed;
            //W32.RedrawWindow(_workerw, , new W32.RECT(,), W32.RedrawWindowFlags.UpdateNow);
            worker.Dispose();
        }

        private void SetLocation(Views.MainWindow window)
        {
            if (window == null | Handle == null) return;
            W32.MoveWindow(Handle, (int)(Screen.PrimaryScreen.WorkingArea.Width - window.ActualWidth), (int)(Screen.PrimaryScreen.WorkingArea.Height - window.ActualHeight), (int)window.ActualWidth, (int)window.ActualWidth, 1);

        }

        private void Worker_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetLocation(Window);
            if (TrainPanel.Count <= 0) return;
            if (TrainPanel[0].DTime <= DateTime.Now)
            {
                if (AddNextTrain(TrainPanel[TrainPanel.Count - 1].DTime, 1))
                {
                    RemoveTrain(0); return;
                };
                SetTimeTabeFromWeb(true, IsHoliday(DateTime.Now.AddDays(1)));
            }
        }


        private void Initialize()
        {
            SetTimeTabeFromWeb(false, IsHoliday(DateTime.Now));//日付を確認するコードが必要

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
            int cnt = 0;
            //var sample = TimeTable.TimeTable[12][0];
            //var timeTableDate = new DateTime(sample.Time.Year, sample.Time.Month, sample.Time.Day);
            /*foreach (var key in TimeTable.TimeTable.Keys)
            {                
                foreach (var t in TimeTable.TimeTable[key])
                {
                    if (t.Time > offset)
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                        { 
                            TrainPanel.Add(new TrainTipViewModel(t.TrainStyle, t.TrainStyle.GetStringValue(), t.DestStationName, t.Time));
                        
                        }));
                        cnt++;
                        if (cnt >= number) return true;
                    }
                }
            }*/
            foreach(var t in TimeTable.TimeTable)
            {
                if (t.Time > offset)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TrainPanel.Add(new TrainTipViewModel(t.TrainStyle, t.TrainStyle.GetStringValue(), t.DestStationName, t.Time));
                    }));
                    cnt++;
                    if (cnt >= number) return true;
                }
            }
            return false;
        }
        private void RemoveTrain(int index)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => { TrainPanel.RemoveAt(index); }));
        }

        private async void SetTimeTabeFromWeb(bool isNextDay, bool isHoliday)
        {
            TimeTable = new TrainTimeTable();
            var uri = "http://www.toyokosoku.co.jp/station/funabashinichidaimae";
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            var document = await context.OpenAsync(uri);
            var query = isHoliday ? $"#tabs-3 > table:nth-child(2)" : $"#tabs-1 > table:nth-child(2)";
            var result = document.QuerySelector(query);
            var trs = result.QuerySelectorAll("tr");
            TimeTable.StationCode = 9933805;
            TimeTable.StationName = document.Title;

            for (int count = 0; count <= trs.Length - 1; count += 2)
            {
                List<TrainTimeDatum> datum = new List<TrainTimeDatum>();
                var hour = trs[count].QuerySelectorAll("td"); //最初の文字が時間
                var minute = trs[count + 1].QuerySelectorAll("td");//分のみ

                int destcnt = 1;
                foreach (var t in minute)
                {
                    var d = new TrainTimeDatum();
                    d.DestStationName = "中野";
                    d.TrainStyle = TrainStyle.local;
                    if (hour[destcnt].TextContent.Contains("三")) d.DestStationName = "三鷹";
                    if (hour[destcnt].TextContent.Contains("東")) d.DestStationName = "東陽町";
                    if (hour[destcnt].TextContent.Contains("▲")) d.TrainStyle = TrainStyle.rapid;
                    if (hour[destcnt].TextContent.Contains("●")) d.TrainStyle = TrainStyle.commuterRapid;
                    d.IsHoliday = isHoliday;
                    var time = new DateTime();
                    if (t.TextContent.Length <= 0) continue;

                    if (!DateTime.TryParse($"{hour[0].TextContent}:{t.TextContent}", out time)) continue;
                    d.Time = time;
                    if (isNextDay)
                        d.Time = d.Time.AddDays(1);
                    //datum.Add(d);
                    TimeTable.TimeTable.Add(d);
                    destcnt++;
                }
                //TimeTable.TimeTable.Add(Convert.ToInt32(hour[0].TextContent), datum);
                //TimeTable.TimeTable.Add(datum);

            }
            if (AddNextTrain(DateTime.Now, 3 - TrainPanel.Count)) return;
            SetTimeTabeFromWeb(true, IsHoliday(DateTime.Now.AddDays(1)));
        }

        private bool IsHoliday(DateTime date)
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
