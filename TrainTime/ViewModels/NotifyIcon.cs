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
        public NotifyIcon()
        {
            _icon = new System.Windows.Forms.NotifyIcon();

            ContextMenuStrip strip = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = "EXIT";
            strip.Items.Add(item);
            item.Click += Item_Click;

            IconText = "TEST";
            _icon.Icon = new Icon("タイトルなし.ico");
            _icon.ContextMenuStrip = strip;
            _icon.Visible = true;
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private string iconText = "TrainTime";
        public string IconText
        {
            get { return iconText; }
            set { _icon.Text = value; iconText = value; }
        }

        

    }
}
