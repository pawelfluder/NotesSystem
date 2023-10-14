namespace WpfNotesSystemProg
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
                var application = new WpfNotesSystem6.App();
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
