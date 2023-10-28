using PdfService.PdfService;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg.Service;
using OutBorder1 = SharpPdfServiceProg.Repetition.OutBorder;

namespace SharpRepoBackendProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        public override void Registrations()
        {
            RegisterByFunc<IPdfService2>(OutBorder1.PdfService);
            RegisterByFunc<IBackendService>(() => new BackendService());
        }
    }
}
