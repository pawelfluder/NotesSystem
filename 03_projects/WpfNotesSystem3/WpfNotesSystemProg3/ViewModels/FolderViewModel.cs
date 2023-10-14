using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Windows.Input;
using Unity;
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

        public (string repo, string loca) CurrentAddress { get; set; }

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

        public void GoAction(string type, (string Repo, string Loca) address)
        {
            var jsonString = backendService.RepoApi(address.Item1, address.Item2);
            ItemModel2 jObj = jObj = JsonConvert.DeserializeObject<ItemModel2>(jsonString);

            Name = jObj.Name;
            CurrentAddress = address;
            HeadersDict = jObj;
        }

        private ItemModel2 headersDict;

        public int SelectedIndex { get; set; }

        public ItemModel2 HeadersDict
        {
            get => headersDict;
            private set
            {
                headersDict = value;
                OnPropertyChanged(nameof(HeadersDict));
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

        public void FolderAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenFolder.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void ConfigAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenConfig.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void AddAction()
        {
            if (ValueToAdd != string.Empty)
            {
                backendService.CommandApi(
                    IBackendService.ApiMethods.CreateFolder.ToString(),
                    CurrentAddress.repo,
                    CurrentAddress.loca,
                    ItemTypes[SelectedIndex],
                    ValueToAdd);

                SetValueToAdd_AndNotify(string.Empty);
                GoAction("Folder", CurrentAddress);
            }
        }

        public bool CanExecute = true;
    }
}
