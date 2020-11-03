using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Plugin_Base;
using System.Threading.Tasks;


namespace Plugin_Base
{
    public interface ITrainTime
    {
        string Name { get; }
        
        string TrainLineName { get; }
        string Description { get; }

        int StationCode { get; }
        string StationName { get; }
        string TrainDirection { get; }

        TrainTimeTable GetTimeTable(bool isNextDay, bool isHoliday);
        //object GetTimeTable();
    }

    /// <summary>
    /// 時刻表の情報を保持
    /// </summary>
    public class TrainTimeTable
    {

        public int StationCode { get; set; }    //船橋日大前は9933805
        public string StationName { get; set; }
        public DateTime Date { get; set; }
        public bool IsHoliday { get; set; }

        public List<TrainTimeDatum> TimeTable { get; set; } = new List<TrainTimeDatum>();
    }

    /// <summary>
    /// 時刻表の各便の時間情報のみを記録する. 
    /// </summary>
    public class TrainTimeDatum
    {
        public DateTime Time { get; set; }
        public int DestStationCode { get; set; }
        public string DestStationName { get; set; }
        public Color StyleColor { get; set; }
        public string Style { get; set; }
    }
}