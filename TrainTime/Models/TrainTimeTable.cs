using Plugin_Base;
using System;
using System.Collections.Generic;
using System.Text;


namespace TrainTime.Models
{
    /*
    public class TrainTimeTable : Plugin_Base.ITrainTimeTable
    {
        public int StationCode { get; set; }    //船橋日大前は9933805
        public string StationName { get; set; }

        public DateTime Date { get; set; }
        //public Dictionary<int, List<TrainTimeDatum>> TimeTable { get; set; } = new Dictionary<int, List<TrainTimeDatum>>();
        public List<Plugin_Base.TrainTimeDatum> TimeTable { get; set; } = new List<Plugin_Base.TrainTimeDatum>();
        //List<ITrainTimeDatum> ITrainTimeTable.TimeTable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class TrainTimeDatum:ITrainTimeDatum
    {
        public bool IsHoliday { get; set; }
        public DateTime Time { get; set; }
        public int DestStationCode { get; set; }
        public string DestStationName { get; set; }

        //public TrainStyle TrainStyle { get; set; }
        public Plugin_Base.TrainStyle Style { get; set; }
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
    */
}
