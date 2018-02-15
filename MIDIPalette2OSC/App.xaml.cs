using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MIDIPalette2OSC.ViewModel;

namespace MIDIPalette2OSC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            this.MainWindow = mainWindow;
            mainWindow.Show();

            var mainViewModel = new MainViewModel();
            mainWindow.DataContext = mainViewModel;
        }
    }
}
