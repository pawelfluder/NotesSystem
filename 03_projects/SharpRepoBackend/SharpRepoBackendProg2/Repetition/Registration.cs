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

namespace SharpRepoBackendProg.Repetition
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
            RegisterByFunc<IRepoService, IFileService>(
                OutBorder3.RepoService,
                container.Resolve<IFileService>());
            RegisterByFunc<IPdfService2>(OutBorder4.PdfService);

            RegisterByFunc<IBackendService>(() => new BackendService());
        }
    }
}
