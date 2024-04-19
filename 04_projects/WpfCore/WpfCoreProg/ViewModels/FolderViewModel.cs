using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;
using WpfNotesSystemProg3.ViewModelBase;

namespace WpfNotesSystem.ViewModels
{
    public class FolderViewModel : BaseViewModel, IItemViewModel
    {
        private readonly IBackendService backendService;
        private readonly IFileService fileService;
        private ICommand addCommand;
        private ICommand folderCommand;
        private ICommand contentCommand;
        private ICommand configCommand;

        public (string repo, string loca) AdrTuple => CreateAdrTuple(Address);

        public string name;

        public string Name
        {
            get => name;
            private set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public FolderViewModel()
        {
            backendService = MyBorder.Container.Resolve<IBackendService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            ItemTypes = new List<string>{ "Text", "Folder" };
            ValueToAdd = string.Empty;
        }

        public string Address { get; set; }

        private (string Repo, string Loca) CreateAdrTuple(string address)
        {
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

        public void GoAction()
        {
            var jsonString = backendService.RepoApi(AdrTuple.Item1, AdrTuple.Item2);
            RepoItem jObj = jObj = JsonConvert.DeserializeObject<RepoItem>(jsonString);

            Name = jObj.Name;
            RepoItem = jObj;
        }

        private RepoItem repoItem;

        public int SelectedIndex { get; set; }

        public RepoItem RepoItem
        {
            get => repoItem;
            private set
            {
                repoItem = value;
                OnPropertyChanged(nameof(RepoItem));
            }
        }

        public string ValueToAdd { get; set; }

        public void SetValueToAdd_AndNotify(string valueToAdd)
        {
            ValueToAdd = valueToAdd;
            OnPropertyChanged(nameof(ValueToAdd));
        }

        public List<string> ItemTypes { get; set; }

        public ICommand FolderCommand
        {
            get
            {
                return folderCommand ?? (folderCommand = new CommandHandler(
                    () => FolderAction(), () => CanExecute));
            }
        }

        public ICommand ConfigCommand
        {
            get
            {
                return configCommand ?? (configCommand = new CommandHandler(
                    () => ConfigAction(), () => CanExecute));
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new CommandHandler(
                    () => AddAction(), () => CanExecute));
            }
        }

        public string ItemType => "Folder";

        public UserControl View { get; set; }

        public void FolderAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenFolder.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void ConfigAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenConfig.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void AddAction()
        {
            if (ValueToAdd != string.Empty)
            {
                backendService.CommandApi(
                    IBackendService.ApiMethods.CreateItem.ToString(),
                    AdrTuple.repo,
                    AdrTuple.loca,
                    ItemTypes[SelectedIndex],
                    ValueToAdd);

                SetValueToAdd_AndNotify(string.Empty);
                GoAction();
            }
        }

        public void SetView(UserControl control)
        {
            this.View = control;
        }

        public bool CanExecute = true;
    }
}
