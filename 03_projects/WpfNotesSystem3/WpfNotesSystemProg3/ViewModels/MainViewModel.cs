using SharpRepoBackendProg.Service;
using SwitchingViewsMVVM.Commands;
using System.Net;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;

namespace SwitchingViewsMVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand goCommand;
        private IItemViewModel _selectedViewModel;
        private readonly IBackendService backendService;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            var container = MyBorder.Container;
            backendService = container.Resolve<IBackendService>();
            GoAction(("Notki", "01"));
        }

        public IItemViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        //public ICommand GoCommand
        //{
        //    get
        //    {
        //        return goCommand ?? (goCommand = new CommandHandler(
        //            () => GoAction(), () => CanExecute));
        //    }
        //}

        public ICommand UpdateViewCommand { get; set; }

        public void GoAction((string Repo, string Loca) address)
        {
            Address = address;
            var type = backendService.RepoApi("GetItemType", address.Item1, address.Item2);
            if (type == "Text") { SelectedViewModel = MyBorder.Container.Resolve<TextViewModel>(); }
            if (type == "Folder") { SelectedViewModel = MyBorder.Container.Resolve<HomeViewModel>(); }
            SelectedViewModel.GoAction(type, address);
        }

        public (string Repo, string Loca) Address { get; private set; }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }
    }
}
