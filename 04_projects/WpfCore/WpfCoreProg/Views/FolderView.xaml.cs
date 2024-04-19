using WpfNotesSystem.ViewModels;
using System.Windows.Controls;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystem.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class FolderView : UserControl
    {
        public FolderView()
        {
            InitializeComponent();
            var tmp = MyBorder.Container.Resolve<MainViewModel>();
            
            tmp.BodyView = this;
            //DataContext = tmp.SelectedViewModel;
            DataContext = tmp.SelectedTab.ViewModel;
            tmp.SelectedTab.ViewModel.View = this;
            //DataContext = MyBorder.Container.Resolve<FolderViewModel>();
        }
    }
}
