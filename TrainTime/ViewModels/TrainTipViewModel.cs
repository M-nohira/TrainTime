using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using System.Windows.Media;
using TrainTime.Models;

namespace TrainTime.ViewModels
{
    public class TrainTipViewModel : BindableBase
    {
        /// <summary>
        /// initialize of Train Tip View 
        /// </summary>
        /// <param name="styleColor">Tip Border Color (Recommend to show Train Color )</param>
        /// <param name="style">Train Style Status (普通|快速|通快)</param>
        /// <param name="dest">Train Destination</param>
        /// <param name="time">Departure Time</param>
        public TrainTipViewModel(Color styleColor, string style, string dest, DateTime time)
        {
            Style = style;
            StyleColorBrush = new SolidColorBrush(styleColor);
            DTime = time;
            Time = time.ToString("HH:mm");
        }

        /// <summary>
        /// initialize of Train Tip View 
        /// </summary>
        /// <param name="styleColor">Tip Border Color (Recommend to show Train Color )</param>
        /// <param name="style">Train Style Status (普通|快速|通快)</param>
        /// <param name="dest">Train Destination</param>
        /// <param name="time">Departure Time</param>
        public TrainTipViewModel(System.Drawing.Color styleColor, string style, string dest, DateTime time)
        {
            Style = style;
            StyleColorBrush = new SolidColorBrush(Color.FromArgb(styleColor.A, styleColor.R, styleColor.G, styleColor.B));
            DTime = time;
            Time = time.ToString("HH:mm");
        }

        public DateTime DTime { get; set; }

        private Brush styleColor = Brushes.OrangeRed;
        public Brush StyleColorBrush
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
