using SharpConfigProg.Repetition;
using System;
using System.Globalization;
using System.Threading;
using WpfNotesSystem;

namespace WpfNotesSystemProg6
{
    public static class Program
    {
        private static NLog.Logger _logger =
            NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        public static void Main()
        {
            _logger.Info("Application started...");

            try
            {
                var registration = new Registration();
                registration.Start();
                var application = new App();
                application.InitializeComponent();
                application.Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            finally
            {
                _logger.Info("Application stopped...");
            }
        }
    }
}
