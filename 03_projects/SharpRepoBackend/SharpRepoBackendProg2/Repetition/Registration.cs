using PdfService.PdfService;
using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using Unity;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;
using OutBorder3 = SharpRepoServiceProg.Repetition.OutBorder;
using OutBorder4 = SharpPdfServiceProg.Repetition.OutBorder;

namespace SharpNotesMigrationTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IFileService>(OutBorder1.FileService);
            RegisterByFunc<IConfigService, IFileService>(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            var configService = container.Resolve<IConfigService>();
            configService.Prepare(typeof(IPreparer.INotesSystem2));

            RegisterByFunc<IRepoService, IFileService>(
                OutBorder3.RepoService,
                container.Resolve<IFileService>());
            RegisterByFunc<IPdfService2>(OutBorder4.PdfService);
        }
    }
}
