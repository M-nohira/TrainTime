using System;
using System.Collections.Generic;
using System.Drawing;
using Plugin_Base;
using AngleSharp;
using System.Threading.Tasks;

namespace Funabashi_Nichidaimae
{
    public class Funabashi_Nichidaimae : Plugin_Base.ITrainTime
    {
        public string Name => "船橋日大前";

        public string Description => "船橋日大前駅用の時刻表取得プラグイン";

        public int StationCode => 9933805;

        public string StationName => "船橋日大前";

        public string TrainLineName => "東葉高速鉄道";

        public string TrainDirection => "三鷹,中野行き";

        TrainTimeTable ITrainTime.GetTimeTable(bool isNextDay, bool isHoliday)
        {
            var timeTable = new TrainTimeTable();
            timeTable.IsHoliday = isHoliday;

            const string uri = "http://www.toyokosoku.co.jp/station/funabashinichidaimae";
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            //var document = await context.OpenAsync(uri);
            var document = Task.Run(async () => await context.OpenAsync(uri)).Result;
            var query = isHoliday ? $"#tabs-3 > table:nth-child(2)" : $"#tabs-1 > table:nth-child(2)";
            var result = document.QuerySelector(query);
            var trs = result.QuerySelectorAll("tr");

            

            for (var count = 0; count <= trs.Length - 1; count += 2)
            {
                var datum = new List<TrainTimeDatum>();
                var hour = trs[count].QuerySelectorAll("td"); //最初の文字が時間
                var minute = trs[count + 1].QuerySelectorAll("td");//分のみ

                var destcnt = 1;
                foreach (var t in minute)
                {
                    var d = new TrainTimeDatum();
                    d.DestStationName = "中野";
                    d.Style = "普通";
                    d.StyleColor = SetColorByStyle(1);
                    if (hour[destcnt].TextContent.Contains("三")) d.DestStationName = "三鷹";
                    if (hour[destcnt].TextContent.Contains("東")) d.DestStationName = "東陽町";
                    if (hour[destcnt].TextContent.Contains("▲"))
                    {
                        d.Style = "快速";
                        d.StyleColor = SetColorByStyle(2);
                    }

                    if (hour[destcnt].TextContent.Contains("●"))
                    {
                        d.Style = "通快";
                        d.StyleColor = SetColorByStyle(3);

                    }

                    var time = new DateTime();
                    if (t.TextContent.Length <= 0) continue;

                    if (!DateTime.TryParse($"{hour[0].TextContent}:{t.TextContent}", out time)) continue;
                    d.Time = time;
                    if (isNextDay)
                        d.Time = d.Time.AddDays(1);
                    
                    timeTable.TimeTable.Add(d);
                    destcnt++;
                }
            }
            return timeTable;
        }

        /// <summary>
        /// 色を設定する
        /// </summary>
        /// <param name="id_Style">1 = 普通, 2 = 快速, 3 = 通快</param>
        /// <returns>色</returns>
        private Color SetColorByStyle(int id_Style)
        {
            switch (id_Style)
            {
                case 1: return Color.FromArgb(255, 0, 104, 183);
                case 2: return Color.FromArgb(255, 233, 84, 100);
                case 3: return Color.OrangeRed;
                case 4: return Color.Yellow;
            }
            return  Color.OrangeRed;
        }

    }
}
