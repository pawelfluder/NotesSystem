using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IPreparer.IOnlyRootPathsPreparer>(()
                => new OnlyRootPathsPreparer());
        }
    }
}
