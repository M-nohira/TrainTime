using System;
using System.Threading.Tasks;
using Plugin_Base;
using AngleSharp;

namespace TokyoStatin_Akihabara
{
    public class TokyoStation_Yamanote_Inside : Plugin_Base.ITrainTime
    {
        public string Name => "東京駅/山手線/内回り";

        public string Description => "山手線内回り";

        public int StationCode => 440101;

        public string StationName => "東京駅";

        public string TrainLineName => "山手線";

        public string TrainDirection => "内回り";
        public TrainTimeTable GetTimeTable(bool isNextDay, bool isHoliday)
        {
            var TimeTable = new Plugin_Base.TrainTimeTable();
            TimeTable.IsHoliday = isHoliday;
            TimeTable.StationName = StationName;
            TimeTable.StationCode = StationCode;

            string uri = "https://www.jreast-timetable.jp/2011/timetable/tt1039/1039120.html";
            if (isHoliday) uri = "https://www.jreast-timetable.jp/2011/timetable/tt1039/1039121.html";

            var context = BrowsingContext.New((Configuration.Default.WithDefaultLoader()));

            var document = Task.Run(async () => await context.OpenAsync(uri)).Result;

            var query = $".result_03 > tbody:nth-child(1)";

            var res = document.QuerySelector(query);
            var trs = res.QuerySelectorAll("tr");

            foreach (var thours in trs)
            {
                var row = thours.QuerySelectorAll("td");
                int hour = 0;
                if(int.TryParse(row[0].TextContent.Replace("時", ""),out hour)) continue;
                for (int cnt = 1; cnt < row.Length; cnt++)
                {
                    var min = row[cnt].QuerySelector(".minute");
                }
            }

        }
    }
}
