using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Repetition;
using SharpRepoBackendProg.Service;
using SwitchingViewsMVVM.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;

namespace SwitchingViewsMVVM.ViewModels
{
    public class TextViewModel : BaseViewModel, IItemViewModel
    {
        //private readonly MainViewModel mainViewModel;
        private readonly IBackendService backendService;
        private ICommand folderCommand;
        private ICommand contentCommand;
        private ICommand configCommand;
        private ICommand pdfCommand;
        private ICommand googledocCommand;
        private ICommand runPrinterCommand;
        private ICommand goCommand;

        public (string repo, string loca) CurrentAddress { get; set; }

        private readonly IFileService fileService;

        public TextViewModel()
        {
            //this.mainViewModel = mainViewModel;
            this.backendService = MyBorder.Container.Resolve<IBackendService>();
            CurrentAddress = ("Sprawy", "01-02");

            fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public ICommand FolderCommand
        {
            get
            {
                return folderCommand ?? (folderCommand = new CommandHandler(
                    () => FolderAction(), () => CanExecute));
            }
        }

        public ICommand ContentCommand
        {
            get
            {
                return contentCommand ?? (contentCommand = new CommandHandler(
                    () => ContentAction(), () => CanExecute));
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

        public ICommand PdfCommand
        {
            get
            {
                return pdfCommand ?? (pdfCommand = new CommandHandler(
                    () => PdfAction(), () => CanExecute));
            }
        }

        public ICommand GoogledocCommand
        {
            get
            {
                return googledocCommand ?? (googledocCommand = new CommandHandler(
                    () => GoogledocAction(), () => CanExecute));
            }
        }

        public ICommand RunPrinterCommand
        {
            get
            {
                return runPrinterCommand ?? (runPrinterCommand = new CommandHandler(
                    () => RunPrinterAction(), () => CanExecute));
            }
        }

        public void FolderAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenFolder.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void ContentAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenContent.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void ConfigAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenConfig.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void PdfAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenPdf.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void RunPrinterAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.RunPrinter.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void GoogledocAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenGoogledoc.ToString(),
                CurrentAddress.repo, CurrentAddress.loca);
        }

        public void GoAction(string type, (string, string) address)
        {
            CurrentAddress = address;
            //backendService.RepoApi(CurrentAddress.repo, CurrentAddress.loca);
            var jsonString = backendService.RepoApi(address.Item1, address.Item2);
            object error = null;
            var jObj = JsonConvert.DeserializeObject<ItemModel2>(jsonString);

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

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }
    }
}
