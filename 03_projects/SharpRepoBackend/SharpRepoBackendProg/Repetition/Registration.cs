using PdfService.PdfService;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg.Services;
using OutBorder1 = SharpPdfServiceProg.Repetition.OutBorder;

namespace SharpRepoBackendProg.Repetition;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        OutContainer.RegisterByFunc<IPdfService2>(OutBorder1.PdfService);
        OutContainer.RegisterByFunc<IBackendService>(() => new BackendService());
    }
}
