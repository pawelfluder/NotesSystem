using SharpContainerProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpRepoServiceProg.AAPublic.OutBorder;

namespace SharpApiArgsProg.Registrations;

internal class OutMockRegistration : RegistrationBase
{
    public override void Registrations()
    {
        var file = OutBorder01.FileService();
        MyBorder.OutContainer.RegisterByFunc(
            () => file);
        
        var repo = OutBorder02.RepoService(file);
        MyBorder.OutContainer.RegisterByFunc(
            () => repo);
        // RecreateInfoGroup recreateInfoGroup = new();
        // RegisterByFunc(() => recreateInfoGroup);
    }
}
