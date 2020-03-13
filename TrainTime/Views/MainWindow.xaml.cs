using System.Windows;
using System.Windows.Interop;
using TrainTime.Models;
using System;
using System.Windows.Forms;

namespace TrainTime.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.SourceInitialized += MainWindow_SourceInitialized;
            
        }
        private IntPtr _handle;
        private void MainWindow_SourceInitialized(object sender, System.EventArgs e)
        {
            _handle = new WindowInteropHelper(this).Handle;
            ViewModels.MainWindowViewModel.Handle = _handle;
            var Style = W32.GetWindowLong(_handle, -20);
            W32.SetWindowLong(_handle, -20, Style | 0x00000020);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModels.MainWindowViewModel.Window = this;
            
        }
    }
}
