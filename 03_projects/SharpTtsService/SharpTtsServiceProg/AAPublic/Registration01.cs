using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpRepoServiceProg.AAPublic.OutBorder;
using SharpContainerProg.AAPublic;
using SharpRepoServiceProg.Service;

namespace WpfNotesSystem.AAPublic
{
    internal class Registration01 : RegistrationBase
    {
        public override void Registrations()
        {
            var fileService = OutBorder1.FileService();
            var repoService = OutBorder2.RepoService(fileService);
            RegisterByFunc<IRepoService>(() => repoService);
        }
    }
}
