using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin_Base
{
    public interface ITrainTime
    {
        string Name { get; }
        string Description { get; }
        int StationCode { get; }
        string StationName { get; }

        
    }

    public interface ITrainTimeTable
    {
       List<ITrainTimeDatum> TimeTable { get;}
    }

    public interface ITrainTimeDatum
    {
        bool IsHoliday { get; }
        DateTime Time { get; }
        int DestStationCode { get; }
        string DestStationName { get;}

       
    }
    public enum TrainStyle
    {
        
    }
}