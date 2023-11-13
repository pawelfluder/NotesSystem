using Newtonsoft.Json;
using SharpRepoBackendProg.Service;
using WpfNotesSystem.Commands;
using System.Collections.Generic;
using System.Windows.Input;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.History;
using WpfNotesSystemProg3.ViewModelBase;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfNotesSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand goCommand;
        private IItemViewModel _selectedViewModel;
        private readonly IBackendService backendService;
        private List<string> _allRepoList;

        private History<(string, string)> addressHistory;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            backendService = MyBorder.Container.Resolve<IBackendService>();
            addressHistory = new History<(string, string)>();
            var jString = backendService.CommandApi(
                IBackendService.ApiMethods.GetAllRepoName.ToString());
            _allRepoList = JsonConvert.DeserializeObject<List<string>>(jString);

            Titles2 = new ObservableCollection<TabItem>();
            var firstRepoName = _allRepoList.First();
            AddTabItem2();
        }

        public List<string> AllRepoList
        {
            get { return _allRepoList; }
            set
            {
                _allRepoList = value;
                OnPropertyChanged(nameof(_allRepoList));
            }
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

        public ICommand UpdateViewCommand { get; set; }

        public IItemViewModel GoAction((string Repo, string Loca) address)
        {
            var type = backendService.RepoApi("GetItemType", address.Item1, address.Item2);
            IItemViewModel viewModel = null;

            if (SelectedViewModel == null)
            {
                if (type == "Text") { viewModel = MyBorder.Container.Resolve<TextViewModel>(); }
                if (type == "Folder") { viewModel = MyBorder.Container.Resolve<FolderViewModel>(); }
                if (type == null) { return viewModel; }
                SelectedViewModel = viewModel;
            }

            NavAddress = CreateAddress(address);
            SelectedViewModel.GoAction(type, address);
            addressHistory.Add(address);
            return viewModel;
        }

        public (string Repo, string Loca) AdrTuple
        {
            get => CreateAdrTuple(SelectedViewModel.Address);
        }

        public string NavAddress
        {
            get => SelectedViewModel.Address;
            set
            {
                SelectedViewModel.Address = value;
                OnPropertyChanged(nameof(NavAddress));
            }
        }

        private string CreateAddress(
            (string Repo, string Loca) address)
        {
            if (address.Loca == string.Empty)
            {
                return address.Repo;
            }

            var url = address.Repo + "/" + address.Loca;
            return url;
        }

        private (string Repo, string Loca) CreateAdrTuple(string address)
        {
            if (!address.Contains('/'))
            {
                return (address, "");
            }

            var tmp = address.Split('/');
            var repo = tmp[0];
            var loca = address.Replace("repoName" + '/', "");

            var adrTuple = (repo, loca);
            return adrTuple;
        }

        public void BackArrow()
        {
            var next = addressHistory.Back(out var success);
            if (success)
            {
                addressHistory.SuppressAdding(() =>
                    GoAction(next));
            }
        }

        public void ForwardArrow()
        {
            var next = addressHistory.Forward(out var success);
            if (success)
            {
                addressHistory.SuppressAdding(() =>
                    GoAction(next));
            }
        }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        // -------------------------------
        static int tabs = 1;
        private ICommand _addTab;
        private ICommand _removeTab;

        public ICommand AddTab
        {
            get
            {
                return _addTab ?? (_addTab = new CommandHandler(
                   () => { AddTabItem2(); }, () => CanExecute));
            }
        }

        private void AddTabItem2()
        {
            IItemViewModel viewModel = GoAction(("notes", ""));

            var header = "Tab " + tabs;
            var item = new TabItem { Header = header, ViewModel = viewModel };

            Titles2.Add(item);
            tabs++;
            OnPropertyChanged("Titles2");
        }

        public ObservableCollection<TabItem> Titles2 { get; set; }

        public class TabItem
        {
            public string Header { get; set; }
            public IItemViewModel ViewModel { get; set; }
        }
    }
}
