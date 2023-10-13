using SharpConfigProg.Repetition;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace WpfNotesSystem
{
    public partial class App : Application
    {
        public App()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            var registration = new Registration();
            registration.Start();
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new WpfNotesSystem.MainWindow();
            MainWindow.Show();
        }
    }
}
