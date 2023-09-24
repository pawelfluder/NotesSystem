using PdfService.PdfService;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
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
        private bool registrationStarted;

        public UnityContainer TryInitialize()
        {
            if (!registrationStarted)
            {
                registrationStarted = true;
                Registrations();
            }

            return container;
        }

        protected override void Registrations()
        {
            RegisterByFunc<IFileService>(OutBorder1.FileService);
            RegisterByFunc<IConfigService, IFileService>(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            var configService = container.Resolve<IConfigService>();
            configService.Prepare(typeof(IConfigService.INotesSystemPreparer2));

            RegisterByFunc<IRepoService, IFileService>(
                OutBorder3.RepoService,
                container.Resolve<IFileService>());
            RegisterByFunc<IPdfService2>(OutBorder4.PdfService);

            RegisterByFunc<IBackendService>(() => new BackendService());
        }
    }
}
