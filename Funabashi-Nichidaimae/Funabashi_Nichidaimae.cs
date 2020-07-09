using System;
using System.Collections.Generic;
using Plugin_Base;
using AngleSharp;
using System.Threading.Tasks;

namespace Funabashi_Nichidaimae
{
    public class Funabashi_Nichidaimae : Plugin_Base.ITrainTime
    {
        public string Name => "船橋日大前プラグイン";

        public string Description => "船橋日大前駅用の時刻表取得プラグイン";

        public int StationCode => 9933805;

        public string StationName => "船橋日大前";

        
        TrainTimeTable ITrainTime.GetTimeTable(bool isNextDay, bool isHoliday)
        {
            var TimeTable = new TrainTimeTable();
            var uri = "http://www.toyokosoku.co.jp/station/funabashinichidaimae";
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            //var document = await context.OpenAsync(uri);
            var document = Task.Run(async () => await context.OpenAsync(uri)).Result;
            var query = isHoliday ? $"#tabs-3 > table:nth-child(2)" : $"#tabs-1 > table:nth-child(2)";
            var result = document.QuerySelector(query);
            var trs = result.QuerySelectorAll("tr");


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
                    d.Style = TrainStyle.local;
                    if (hour[destcnt].TextContent.Contains("三")) d.DestStationName = "三鷹";
                    if (hour[destcnt].TextContent.Contains("東")) d.DestStationName = "東陽町";
                    if (hour[destcnt].TextContent.Contains("▲")) d.Style = TrainStyle.rapid;
                    if (hour[destcnt].TextContent.Contains("●")) d.Style = TrainStyle.commuterRapid;
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
            return TimeTable;
        }

        
    }

    /*
    public class TrainTimeTable : ITrainTimeTable
    {
        public int StationCode => 9933805;

        public string StationName => "船橋日大前";

        public DateTime Date { get; set; }

        public List<Plugin_Base.TrainTimeDatum> TimeTable { get; set; } = new List<Plugin_Base.TrainTimeDatum>();
    }
    */
        /*
        public class TrainTimeDatum : ITrainTimeDatum
        {
            public bool IsHoliday { get; set; }

            public DateTime Time { get; set; }

            public int DestStationCode { get; set; }

            public string DestStationName { get; set; }

            public TrainStyle Style { get; set; }

        }
        `*/
}

