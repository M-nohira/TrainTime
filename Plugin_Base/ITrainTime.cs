using System;
using System.Collections.Generic;
using System.Text;
using Plugin_Base;
using System.Threading.Tasks;

namespace Plugin_Base
{
    public interface ITrainTime
    {
        string Name { get; }
    
        
        string Description { get; }
        int StationCode { get; }
        string StationName { get; }

        TrainTimeTable GetTimeTable(bool isNextDay, bool isHoliday);
        
    }

    public class TrainTimeTable
    {
        public int StationCode { get; set; }    //船橋日大前は9933805
        public string StationName { get; set; }
        public DateTime Date { get; set; }

        public List<TrainTimeDatum> TimeTable { get; set; } = new List<TrainTimeDatum>();
    }

    public class TrainTimeDatum
    {
        public bool IsHoliday { get; set; }
        public DateTime Time { get; set; }
        public int DestStationCode { get; set; }
        public string DestStationName { get; set; }

        public TrainStyle Style { get; set; }

    }
    public enum TrainStyle
    {
        [StringValue("普通")]
        local,
        [StringValue("快速")]
        rapid,
        [StringValue("通快")]
        commuterRapid
    }
    
}