namespace WpfNotesSystemProg.ViewModels
{
    internal class MainViewModel
    {
        private BaseViewModel selectedViewModel = new FolderViewModel();

        public BaseViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; }
        }
    }
}

