using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;
using WpfNotesSystemProg3.ViewModelBase;

namespace WpfNotesSystem.ViewModels
{
    public class TextViewModel : BaseViewModel, IItemViewModel
    {
        private readonly IBackendService backendService;
        private ICommand folderCommand;
        private ICommand contentCommand;
        private ICommand configCommand;
        private ICommand pdfCommand;
        private ICommand googledocCommand;
        private ICommand runPrinterCommand;
        private ICommand goCommand;
        private ICommand addCommand;

        public (string repo, string loca) CurrentAddress { get; set; }

        private readonly IFileService fileService;

        public TextViewModel()
        {
            //this.mainViewModel = mainViewModel;
            this.backendService = MyBorder.Container.Resolve<IBackendService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            ValueToAdd = string.Empty;
        }

        public string ValueToAdd { get; set; }

        public void SetValueToAdd_AndNotify(string valueToAdd)
        {
            ValueToAdd = valueToAdd;
            OnPropertyChanged(nameof(ValueToAdd));
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

        public void AddAction()
        {
            if (ValueToAdd != string.Empty)
            {
                backendService.CommandApi(
                    IBackendService.ApiMethods.AddContent.ToString(),
                    CurrentAddress.repo,
                    CurrentAddress.loca,
                    ValueToAdd);
                SetValueToAdd_AndNotify(string.Empty);
                GoAction("Text", CurrentAddress);
            }
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
