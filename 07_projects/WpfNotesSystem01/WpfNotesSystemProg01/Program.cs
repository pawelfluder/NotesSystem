using System;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace WpfNotesSystemProg01;

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
            // AppFasade app = new AppFasade();
            // ContainerService.SetOutContainer(app.Container);
            // OutBorder01.GetPreparer("DefaultPreparer").Prepare();
            // app.Start();
            
            OutBorder01.GetPreparer("DefaultPreparer").Prepare();
            WpfNotesSystem.App application = new();
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
