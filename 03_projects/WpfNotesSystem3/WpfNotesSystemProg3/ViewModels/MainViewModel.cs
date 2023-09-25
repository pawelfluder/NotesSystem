using Newtonsoft.Json;
using SharpRepoBackendProg.Service;
using SwitchingViewsMVVM.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.History;
using WpfNotesSystemProg3.ViewModelBase;

namespace SwitchingViewsMVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand goCommand;
        private IItemViewModel _selectedViewModel;
        private readonly IBackendService backendService;
        private List<string> _allRepoList;
        private string _addressString;
        private (string Repo, string Loca) _address;

        private History<(string, string)> addressHistory;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            var container = MyBorder.Container;
            backendService = container.Resolve<IBackendService>();
            addressHistory = new History<(string, string)>();
            var jString = backendService.CommandApi(
                IBackendService.ApiMethods.GetAllRepoName.ToString());
            _allRepoList = JsonConvert.DeserializeObject<List<string>>(jString);
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
            addressHistory.Add(address);
            Address = address;
            AddressString = CreateUrlFromAddress(address);
            var type = backendService.RepoApi("GetItemType", address.Item1, address.Item2);
            if (type == "Text") { SelectedViewModel = MyBorder.Container.Resolve<TextViewModel>(); }
            if (type == "Folder") { SelectedViewModel = MyBorder.Container.Resolve<FolderViewModel>(); }
            SelectedViewModel.GoAction(type, address);
        }

        public (string Repo, string Loca) Address
        {
            get { return _address; }
            private set
            {
                if (value.Loca.StartsWith('/'))
                {
                    throw new Exception();
                }
                _address = value;
            }
        }

        public string AddressString
        {
            get { return _addressString; }
            set
            {
                _addressString = value;
                OnPropertyChanged(nameof(AddressString));
            }
        }


        private string CreateUrlFromAddress((string Repo, string Loca) address)
        {
            if (address.Loca == string.Empty)
            {
                return address.Repo;
            }

            var url = address.Repo + "/" + address.Loca;
            return url;
        }

        internal void BackArrow()
        {
            var next = addressHistory.Back(out var success);
            if (success)
            {
                addressHistory.SuppressAdding(() =>
                    GoAction(next));
            }
        }

        internal void ForwardArrow()
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
    }
}
