using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TrainTime.ViewModels
{
    public class NotifyIcon
    {

        private new System.Windows.Forms.NotifyIcon _icon = null;

        public NotifyIcon(List<ToolStripItem> items)
        {
            _icon = new System.Windows.Forms.NotifyIcon();

            var strip = new ContextMenuStrip();
            
            strip.Items.AddRange(items.ToArray());

            IconText = "TEST";
            _icon.Icon = new Icon("タイトルなし.ico");
            _icon.ContextMenuStrip = strip;
            _icon.Visible = true;
        }

        private string _iconText = "TrainTime";
        public string IconText
        {
            get { return _iconText; }
            set { _icon.Text = value; _iconText = value; }
        }



    }
}
