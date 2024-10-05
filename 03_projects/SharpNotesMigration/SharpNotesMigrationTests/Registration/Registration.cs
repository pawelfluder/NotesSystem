using SharpContainerProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpRepoServiceProg.AAPublic;
//using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
//using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
//using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpNotesMigrationProg.Repetition.OutBorder;

namespace SharpNotesMigrationTests.Registration;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        var fileService = container.Resolve<IFileService>();
        var repoService = container.Resolve<IRepoService>();

        RegisterByFunc
        (OutBorder4.MigrationService,
            fileService,
            repoService);
    }
}