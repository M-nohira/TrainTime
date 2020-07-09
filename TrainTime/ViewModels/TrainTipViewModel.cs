using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using System.Windows.Media;
using TrainTime.Models;

namespace TrainTime.ViewModels
{
    public class TrainTipViewModel:BindableBase
    {
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="isRapid">Rapid / Local</param>
        /// <param name="dest">Destination</param>
        /// <param name="time">ArrivalTime</param>
        public TrainTipViewModel(Plugin_Base.TrainStyle trainStyle,string style, string dest, DateTime time)
        {
            Style = style;
            switch(trainStyle)
            {
                case Plugin_Base.TrainStyle.local: StyleColor = new SolidColorBrush(Color.FromArgb(255, 0, 104, 183)); break;
                case Plugin_Base.TrainStyle.rapid: StyleColor = new SolidColorBrush(Color.FromArgb(255, 233, 84, 100)); break;
                case Plugin_Base.TrainStyle.commuterRapid: StyleColor = new SolidColorBrush(Colors.OrangeRed);break;
            }
            Dest = dest;
            DTime = time;
            Time = time.ToString("HH:mm");
        }

        public DateTime DTime { get; set; }

        private Brush styleColor = Brushes.OrangeRed;
        public Brush StyleColor 
        {
            get
            {
                return styleColor;
            }
            set
            {
                SetProperty(ref styleColor, value);
            }
        }

        private string style = "普通";
        public string Style 
        {
            get
            {
                return style;
            }
            set
            {
                SetProperty(ref style, value);
            }
        }

        private string dest = "中野";
        public string Dest
        {
            get
            {
                return dest;
            }
            set
            {
                SetProperty(ref dest, value);
            }
        }

        private string time = "12:00";
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                SetProperty(ref time, value);
            }
        }    
    }

    


}
