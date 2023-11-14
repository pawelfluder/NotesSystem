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
using System;
using SharpRepoServiceProg.Service;

namespace WpfNotesSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand goCommand;
        private IItemViewModel _selectedViewModel;
        private readonly IBackendService backendService;
        private readonly IRepoService repoService;
        private List<string> _allRepoList;

        private History<(string, string)> addressHistory;

        public UserControl BodyView { get; set; }

        public UserControl MainView { get; set; }

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
            var firstRepoName = _allRepoList.First();
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
            get => CreateAdrTuple(SelectedViewModel.Address);
        }

        public string NavAddress
        {
            get => SelectedViewModel?.Address;
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
                   () => { OnTabRemove(); }, () => CanExecute));
            }
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
            }
        }

        private string PrintCurrentState()
        {
            var result = new List<string>();
            var i = 0;
            var t = "\t";
            var t2 = t + t;
            result.Add("//TabItems");
            foreach (var tabItem in Titles2)
            {
                result.Add(t+"//Item[" + i + "]");
                result.Add(t2 + "Index:" + i);
                result.Add(t2 + "ViewModel.HashCode: " + tabItem.ViewModel.GetHashCode());
                result.Add(t2 + "ViewModel.RepoItem.Name: " + tabItem.ViewModel?.RepoItem?.Name);
                result.Add(t2 + "Header: " + tabItem.Header.ToString());
                result.Add(t2 + "Address: " + tabItem.ViewModel.Address?.ToString());
                result.Add(t2 + "View.HashCode: " + tabItem.ViewModel.View?.GetHashCode());
                i++;
            }


            if (SelectedTab != null)
            {
                i = Titles2.IndexOf(SelectedTab);
                result.Add("//SelectedTab");
                result.Add(t + "Index:" + i);
                result.Add(t + "ViewModel.HashCode: " + SelectedTab.ViewModel.GetHashCode());
                result.Add(t + "ViewModel.RepoItem.Name: " + SelectedTab.ViewModel.RepoItem?.Name);
                result.Add(t + "Header: " + SelectedTab.Header.ToString());
                result.Add(t + "Address: " + SelectedTab.ViewModel.Address?.ToString());
                result.Add(t + "View.HashCode: " + SelectedTab.ViewModel.View?.GetHashCode());
                result.Add("");
            }

            if (SelectedViewModel != null)
            {
                var found = Titles2.SingleOrDefault(x => x.ViewModel == SelectedViewModel);
                i = Titles2.IndexOf(found);
                result.Add("//SelectedViewModel");
                result.Add(t + "Index:" + i);
                result.Add(t + "ViewModel.HashCode: " + SelectedViewModel.GetHashCode());
                result.Add(t + "ViewModel.RepoItem.Name: " + SelectedViewModel.RepoItem.Name);
                result.Add(t + "Address: " + SelectedViewModel.Address?.ToString());
                result.Add(t + "View.HashCode: " + SelectedViewModel.View?.GetHashCode());
                result.Add("");
            }

            var result2 = string.Join("\n", result);
            return result2;
        }

        private void OnTabChanged()
        {
            OnPropertyChanged("RepoItem");
            UpdateViewProps(SelectedTab.ViewModel);
            SelectedViewModel.GoAction();
            var state = PrintCurrentState();
        }

        private void OverrideOnTypeChange(
            string repoItemType,
            IItemViewModel viewModel)
        {
            if (viewModel.ItemType == repoItemType)
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

        private void OnTabRemove()
        {
            Titles2.Remove(Titles2.Last());
        }

        private void OnTabAdd()
        {
            var adrTuple = ("notes", "");
            var type = backendService.RepoApi("GetItemType", adrTuple.Item1, adrTuple.Item2);
            var viewModel = CreateViewModel(type, adrTuple);

            TryAddTab(viewModel);
            UpdateViewProps(viewModel);
            SelectedViewModel.GoAction();

            var state = PrintCurrentState();
        }

        public IItemViewModel GoAction((string Repo, string Loca) address)
        {
            var repoItemType = repoService.Methods.GetItemType(address.Repo, address.Loca);
            OverrideOnTypeChange(repoItemType, SelectedTab.ViewModel);
            SelectedViewModel.Address = CreateAddress(address);
            SelectedViewModel.GoAction();
            UpdateViewProps(SelectedViewModel);
            
            var state = PrintCurrentState();
            return null;
        }

        private void UpdateViewProps(IItemViewModel viewModel)
        {
            var tmp2 = Titles2.SingleOrDefault(x => x.ViewModel == viewModel);
            if (tmp2 != null && _selectedTab != tmp2)
            {
                _selectedTab = tmp2;
            }
            
            SelectedViewModel = viewModel;
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
                var gg = MainView.Content as ScrollViewer;
                var gg2 = gg.Content as Grid;
                var gg3 = gg2.Children[5] as TabControl;
            }
            
        }

        public ObservableCollection<TabItem> Titles2 { get; set; }

        public class TabItem
        {
            public string Header { get; set; }
            public IItemViewModel ViewModel { get; set; }
        }

        public IItemViewModel GetViewModel(int hashCode)
        {
            var tabItem = Titles2
                .SingleOrDefault(x => x.ViewModel.GetHashCode() == hashCode);
            return tabItem.ViewModel;
        }
    }
}
