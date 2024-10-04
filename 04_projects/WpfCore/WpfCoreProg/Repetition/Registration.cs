using WpfNotesSystem.ViewModels;
using OutBorder3 = SharpRepoBackendProg2.Repetition.OutBorder;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg2.Service;

namespace WpfNotesSystem.Repetition
{
    internal class Registration : RegistrationBase
    {
        public override void Registrations()
        {
            RegisterByFunc<IBackendService>(
                OutBorder3.BackendService);
            container.Resolve<IBackendService>();

            RegisterByFunc<MainViewModel>(
                () => new MainViewModel());
            RegisterByFunc<TextViewModel>(
                () => new TextViewModel(), 1);
            RegisterByFunc<FolderViewModel>(
                () => new FolderViewModel(), 1);
        }
    }
}
