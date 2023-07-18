using SharpRepoBackendProg.Repetition;
using SharpRepoBackendProg.Service;
using System.Windows.Input;
using WpfNotesSystem;

namespace WpfNotesSystemProg.ViewModels
{
    public class TextViewModel : BaseViewModel
    {
        private readonly IBackendService backendService;
        private ICommand folderCommand;
        private ICommand contentCommand;
        private ICommand configCommand;
        private ICommand pdfCommand;
        private ICommand googledocCommand;
        private ICommand runPrinterCommand;
        private ICommand goCommand;

        public (string repo, string loca) CurrentAddress { get; set; }

        public TextViewModel()
        {
            this.backendService = OutBorder.BackendService();
            CurrentAddress = ("Sprawy", "01-02");
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

        public ICommand GoCommand
        {
            get
            {
                return goCommand ?? (goCommand = new CommandHandler(
                    () => GoAction(), () => CanExecute));
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

        public void GoAction()
        {
            backendService.RepoApi(CurrentAddress.repo, CurrentAddress.loca);
        }

        public string GoAction((string Repo, string Loca) address)
        {
            var gg = backendService.RepoApi(address.Repo, address.Loca);
            return gg;
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
