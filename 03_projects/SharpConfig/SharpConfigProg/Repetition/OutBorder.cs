using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
{
    public static class OutBorder
    {
        public static IConfigService ConfigService(
            IFileService fileService)
        {
            if (!MyBorder.Container.IsRegistered<IConfigService>())
            {
                MyBorder.Registration.RegisterByFunc<IConfigService>(
                    () => new ConfigService(fileService));

                MyBorder.Registration.RegisterByFunc<IPreparer.INotesSystem, IFileService>(
                    (x) => new NotesSystemPreparer(x),
                    fileService);

                MyBorder.Registration.RegisterByFunc<IPreparer.INotesSystem2, IFileService>(
                    (x) => new NotesSystemPreparer2(x),
                    fileService);
            }

            return MyBorder.Container.Resolve<IConfigService>();
        }
    }
}
