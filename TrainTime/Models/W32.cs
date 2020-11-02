using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;

namespace TrainTime.Models
{
    class W32
    {
        [DllImport("User32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int widht, int height, int bRepaint);

        [DllImport("User32.dll")]
        public static extern long GetWindowLong(IntPtr handle, int nindex);

        [DllImport("User32.dll")]
        public static extern long SetWindowLong(IntPtr handle, int nindex, long newLong);
    }
}
