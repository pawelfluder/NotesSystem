using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;

namespace SwitchingViewsMVVM.ViewModels
{
    public class HomeViewModel : BaseViewModel, IItemViewModel
    {
        private readonly IBackendService backendService;
        private readonly IFileService fileService;
        private ICommand folderCommand;
        private ICommand contentCommand;
        private ICommand configCommand;

        //private readonly MainViewModel mainViewModel;

        public (string repo, string loca) CurrentAddress { get; set; }

        public HomeViewModel()
        {
            backendService = MyBorder.Container.Resolve<IBackendService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public void GoAction(string type, (string Repo, string Loca) address)
        {
            //backendService.RepoApi(address.Repo, address.Loca);
            var jsonString = backendService.RepoApi(address.Item1, address.Item2);
            ItemModel2 jObj = jObj = JsonConvert.DeserializeObject<ItemModel2>(jsonString);
            CurrentAddress = address;
            HeadersDict = jObj;
        }

        private ItemModel2 headersDict;

        public ItemModel2 HeadersDict
        {
            get => headersDict;
            private set
            {
                headersDict = value;
                OnPropertyChanged(nameof(HeadersDict));
            }
        }

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

        public bool CanExecute = true;
    }
}
