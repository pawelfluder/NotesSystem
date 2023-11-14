using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Windows.Controls;
using System.Windows.Input;
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

        public (string repo, string loca) AdrTuple => CreateAdrTuple(Address);

        private readonly IFileService fileService;

        public TextViewModel()
        {
            //this.mainViewModel = mainViewModel;
            this.backendService = MyBorder.Container.Resolve<IBackendService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            ValueToAdd = string.Empty;
        }

        public string Address { get; set; }

        public string ItemType => "Text";

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
                AdrTuple.repo, AdrTuple.loca);
        }

        public void ContentAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenContent.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void ConfigAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenConfig.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void PdfAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenPdf.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void RunPrinterAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.RunPrinter.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void GoogledocAction()
        {
            backendService.CommandApi(
                IBackendService.ApiMethods.OpenGoogledoc.ToString(),
                AdrTuple.repo, AdrTuple.loca);
        }

        public void GoAction()
        {
            //backendService.RepoApi(CurrentAddress.repo, CurrentAddress.loca);
            var jsonString = backendService.RepoApi(AdrTuple.Item1, AdrTuple.Item2);
            object error = null;
            var jsonObj = JsonConvert.DeserializeObject<RepoItem>(jsonString);
            Name = jsonObj.Name;
            RepoItem = jsonObj;
        }

        public void AddAction()
        {
            if (ValueToAdd != string.Empty)
            {
                backendService.CommandApi(
                    IBackendService.ApiMethods.AddContent.ToString(),
                    AdrTuple.repo,
                    AdrTuple.loca,
                    ValueToAdd);
                SetValueToAdd_AndNotify(string.Empty);
                GoAction();
            }
        }

        private RepoItem repoItem;
        public RepoItem RepoItem
        {
            get => repoItem;
            private set
            {
                repoItem = value;
                OnPropertyChanged(nameof(RepoItem));
                OnPropertyChanged("SelectedViewModel");
            }
        }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        public UserControl View { get; set; }
    }
}
