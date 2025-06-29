using SharpContainerProg.AAPublic;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Data;
using SharpIdentityProg.Workers;

namespace SharpIdentityProg.Registrations;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        IApiRegister api = new ApiRegister<ApplicationUser>();
        MyBorder.MyContainer.RegisterByFunc(
            () => api);
    }
}
