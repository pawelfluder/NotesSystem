using SharpConfigProg.AAPublic;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace WpfNotesSystem6
{
    public partial class App : Application
    {
        public App()
        {
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new WpfNotesSystem.MainWindow();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            MainWindow.Show();
        }
    }
}
