using SharpRepoBackendProg.Service;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystem.ViewModels;

namespace WpfNotesSystemProg.Repetition
{
    public class OutBorder
    {
        public static MainViewModel MainViewModel()
        {
            return MyBorder.Container.Resolve<MainViewModel>();
        }
    }
}
