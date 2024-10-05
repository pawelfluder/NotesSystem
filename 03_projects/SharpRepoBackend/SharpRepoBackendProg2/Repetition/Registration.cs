using PdfService.PdfService;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg2.Service;
using OutBorder1 = SharpPdfServiceProg.Repetition.OutBorder;

namespace SharpRepoBackendProg2.Repetition;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        RegisterByFunc<IPdfService2>(OutBorder1.PdfService);
        RegisterByFunc<IBackendService>(() => new BackendService());
    }
}