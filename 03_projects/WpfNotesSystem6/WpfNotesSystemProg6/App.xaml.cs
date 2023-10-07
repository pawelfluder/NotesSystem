using SharpConfigProg.Repetition;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfNotesSystem
{
    public partial class App : Application
    {
        public App()
        {
            new Registration();
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new WpfNotesSystem.MainWindow();
            MainWindow.Show();
        }
    }
}
