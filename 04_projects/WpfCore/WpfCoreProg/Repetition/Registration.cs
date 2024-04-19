using SharpRepoBackendProg.Service;
using WpfNotesSystem.ViewModels;
using OutBorder3 = SharpRepoBackendProg.Repetition.OutBorder;
using SharpContainerProg.AAPublic;

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
