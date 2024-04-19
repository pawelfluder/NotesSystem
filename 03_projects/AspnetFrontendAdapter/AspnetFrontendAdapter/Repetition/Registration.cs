using SharpRepoBackendProg.Service;
using OutBorder1 = SharpRepoBackendProg.Repetition.OutBorder;

namespace AspnetFrontendAdapterProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IBackendService>(OutBorder1.BackendService);
        }
    }
}
