using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using SwitchingViewsMVVM.ViewModels;
using Unity;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;
using OutBorder3 = SharpRepoBackendProg.Repetition.OutBorder;

namespace WpfNotesSystem.Repetition
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
            RegisterByFunc<IBackendService>(
                OutBorder3.BackendService);
            container.Resolve<IBackendService>();

            RegisterByFunc<IFileService>(OutBorder1.FileService);
            RegisterByFunc<IConfigService, IFileService>(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            RegisterByFunc<MainViewModel>(
                () => new MainViewModel());
            RegisterByFunc<TextViewModel>(
                () => new TextViewModel());
            RegisterByFunc<FolderViewModel>(
                () => new FolderViewModel());
        }
    }
}
