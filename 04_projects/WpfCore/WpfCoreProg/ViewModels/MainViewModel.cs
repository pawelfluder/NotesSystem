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
using System.Windows.Controls;
using WpfCoreProg.Debug;
using System.ComponentModel;
using SharpRepoServiceProg.AAPublic;

namespace WpfNotesSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand goCommand;
        //private IItemViewModel _selectedViewModel;
        private readonly IBackendService backendService;
        private readonly IRepoService repoService;
        private List<string> _allRepoList;

        private History<(string, string)> addressHistory;

        public UserControl BodyView { get; set; }

        public UserControl MainView { get; set; }

        public (string, string) defaultRepo;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            backendService = MyBorder.Container.Resolve<IBackendService>();
            repoService = MyBorder.Container.Resolve<IRepoService>();
            addressHistory = new History<(string, string)>();
            var jString = backendService.CommandApi(
            IBackendService.ApiMethods.GetAllRepoName.ToString());
            _allRepoList = JsonConvert.DeserializeObject<List<string>>(jString);

            Titles2 = new ObservableCollection<TabItem>();
            defaultRepo = (_allRepoList.First(), "");
            DebugState = new DebugState();
            OnTabAdd();
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

        //public IItemViewModel SelectedViewModel
        //{
        //    get { return _selectedViewModel; }
        //    set
        //    {
        //        _selectedViewModel = value;
        //        OnPropertyChanged(nameof(SelectedViewModel));
        //    }
        //}

        public ICommand UpdateViewCommand { get; set; }

        public IItemViewModel CreateViewModel(string type, (string, string) adrTuple)
        {
            IItemViewModel viewModel = null;
            if (type == "Text") { viewModel = MyBorder.Container.Resolve<TextViewModel>(); }
            if (type == "Folder") { viewModel = MyBorder.Container.Resolve<FolderViewModel>(); }
            viewModel.Address = CreateAddress(adrTuple);
            return viewModel;
        }

        public (string Repo, string Loca) AdrTuple
        {
            get => CreateAdrTuple(SelectedTab.ViewModel.Address);
        }

        public string NavAddress
        {
            get => SelectedTab.ViewModel?.Address;
            set
            {
                //SelectedViewModel.Address = value;
                SelectedTab.ViewModel.Address = value;
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
            if (address == null)
            {
                return default;
            }

            if (!address.Contains('/'))
            {
                return (address, "");
            }

            var tmp = address.Split('/');
            var repo = tmp[0];
            var loca = address.Replace(repo + '/', "");

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
        static int tabs = 0;
        private ICommand _addTab;
        private ICommand _removeTab;
        public ICommand RemoveTab
        {
            get
            {
                return _removeTab ?? (_removeTab = new CommandHandler(
                   () => { OnRemoveClicked(); }, () => CanExecute));
            }
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new CommandHandler(
                   () => { OnGoBackClicked(); }, () => CanExecute));
            }
        }

        private void OnGoBackClicked()
        {
            var tmp = NavAddress.Split('/').ToList();
            if (!(tmp.Count > 1))
            {
                return;
            }

            tmp.RemoveAt(tmp.Count() - 1);
            var newAddress = string.Join('/', tmp);

            //NavAddress = newAddress;
            var adrTuple = CreateAdrTuple(newAddress);
            GoAction(adrTuple);
        }

        private ICommand _debugCommand;
        public ICommand DebugCommand
        {
            get
            {
                return _debugCommand ?? (_debugCommand = new CommandHandler(
                   () => { OnDebugClicked(); }, () => CanExecute));
            }
        }

        private void OnDebugClicked()
        {
            DebugState.Body = PrintCurrentState();
            DebugState.Visibility = "Visible";
            var tabObservable = Titles2;
        }

        public ICommand AddTab
        {
            get
            {
                return _addTab ?? (_addTab = new CommandHandler(
                   () => { OnTabAdd(); }, () => CanExecute));
            }
        }

        private TabItem _selectedTab;

        public TabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnTabChanged();
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        private string PrintCurrentState()
        {
            var result = new List<string>();
            var i = 0;
            var t = "    ";
            var t2 = t + t;
            
            if (SelectedTab != null)
            {
                i = Titles2.IndexOf(SelectedTab);
                result.Add("//SelectedTab");
                result.Add(t + "Index:" + i);
                result.Add(t + "ViewModel.HashCode: " + SelectedTab.ViewModel.GetHashCode());
                result.Add(t + "ViewModel.RepoItem.Name: " + SelectedTab.ViewModel.RepoItem?.Name);
                result.Add(t + "ViewModel.Type: " + SelectedTab.ViewModel.ItemType);
                result.Add(t + "Header: " + SelectedTab.Header.ToString());
                result.Add(t + "Address: " + SelectedTab.ViewModel.Address?.ToString());
                result.Add(t + "View.HashCode: " + SelectedTab.ViewModel.View?.GetHashCode());
                result.Add("");
            }

            i = 0;
            result.Add("//TabItems");
            foreach (var tabItem in Titles2)
            {
                result.Add(t + "//Item[" + i + "]");
                result.Add(t2 + "Index:" + i);
                result.Add(t2 + "ViewModel.HashCode: " + tabItem.ViewModel.GetHashCode());
                result.Add(t2 + "ViewModel.RepoItem.Name: " + tabItem.ViewModel?.RepoItem?.Name);
                result.Add(t + "ViewModel.Type: " + tabItem.ViewModel.ItemType);
                result.Add(t2 + "Header: " + tabItem.Header.ToString());
                result.Add(t2 + "Address: " + tabItem.ViewModel.Address?.ToString());
                result.Add(t2 + "View.HashCode: " + tabItem.ViewModel.View?.GetHashCode());
                i++;
            }
            result.Add("");

            var result2 = string.Join("\n", result);
            return result2;
        }

        private void AddItemLog(
            string title,
            List<string> inputList)
        {

        }

        private void OnTabChanged()
        {
            OnPropertyChanged("RepoItem");
            if (SelectedTab != null)
            {
                UpdateViewProps(SelectedTab.ViewModel);
                SelectedTab.ViewModel.GoAction();
            }
            
            DebugState.Body = PrintCurrentState();
        }

        private DebugState _debugState;
        public DebugState DebugState
        {
            get => _debugState;
            set
            {
                _debugState = value;
                OnPropertyChanged("State");
            }
        }


        private void OverrideOnTypeChange(
            string repoItemType,
            IItemViewModel viewModel)
        {
            if (SelectedTab.ViewModel.ItemType == repoItemType)
            {
                return;
            }

            var tmp = Titles2.SingleOrDefault(x => x.ViewModel == viewModel);
            if (tmp != null)
            {
                var index = Titles2.IndexOf(tmp);
                var newViewModel = CreateViewModel(repoItemType, CreateAdrTuple(viewModel.Address));
                tmp.ViewModel = newViewModel;
            }
        }

        private void TryAddTab(IItemViewModel viewModel)
        {
            var tmp = Titles2.SingleOrDefault(x => x.ViewModel == viewModel);
            if (tmp == null)
            {
                tabs++;
                var header = "Tab " + tabs;
                var item = new TabItem { Header = header, ViewModel = viewModel };
                Titles2.Add(item);
            }
        }

        private void OnRemoveClicked()
        {
            if(!(Titles2.Count > 1))
            {
                return;
            }

            var toRemove = Titles2.Last();

            if (SelectedTab == toRemove &&
                Titles2.Count() == 1)
            {
                UpdateViewProps(null);
            }

            if (SelectedTab == toRemove)
            {
                SelectedTab = Titles2.First();
            }

            Titles2.Remove(toRemove);
            DebugState.Body =PrintCurrentState();
        }

        private void OnTabAdd()
        {
            var type = backendService.RepoApi("GetItemType", defaultRepo.Item1, defaultRepo.Item2);
            var viewModel = CreateViewModel(type, defaultRepo);

            TryAddTab(viewModel);
            UpdateViewProps(viewModel);
            SelectedTab.ViewModel.GoAction();

            DebugState.Body =PrintCurrentState();
        }

        public void GoAction((string Repo, string Loca) address)
        {
            var repoItemType = repoService.Methods.GetItemType(address.Repo, address.Loca);
            OverrideOnTypeChange(repoItemType, SelectedTab.ViewModel);
            SelectedTab.ViewModel.Address = CreateAddress(address);
            SelectedTab.ViewModel.GoAction();
            UpdateViewProps(SelectedTab.ViewModel);

            DebugState.Body =PrintCurrentState();
        }

        private void UpdateViewProps(IItemViewModel viewModel)
        {
            var tmp2 = Titles2.SingleOrDefault(x => x.ViewModel == viewModel);
            if (tmp2 != null && _selectedTab != tmp2)
            {
                _selectedTab = tmp2;
            }
            
            //SelectedViewModel = viewModel;
            var gg = NavAddress;
            if (BodyView != null)
            {
                BodyView.DataContext = viewModel;
            }

            OnPropertyChanged("RepoItem");
            OnPropertyChanged("Titles2");
            OnPropertyChanged("NavAddress");
            OnPropertyChanged("SelectedTab");

            MainViewInspect();
        }

        private void MainViewInspect()
        {
            if (MainView != null)
            {
                //var gg = MainView.Content as ScrollViewer;
                //var gg2 = gg.Content as Grid;
                //var gg3 = gg2.Children[5] as TabControl;
            }
            
        }

        public ObservableCollection<TabItem> Titles2 { get; set; }

        public class TabItem : INotifyPropertyChanged
        {
            private IItemViewModel _viewModel;

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        
            public string Header { get; set; }

            public IItemViewModel ViewModel
            {
                get => _viewModel;
                set
                {
                    _viewModel = value;
                    OnPropertyChanged(nameof(ViewModel));
                }
            }
        }

        public IItemViewModel GetViewModel(int hashCode)
        {
            var tabItem = Titles2
                .SingleOrDefault(x => x.ViewModel.GetHashCode() == hashCode);
            return tabItem.ViewModel;
        }
    }
}
