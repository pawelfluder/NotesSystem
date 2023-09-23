namespace WpfNotesSystemProg3.ViewModelBase
{
    public interface IItemViewModel
    {
        void GoAction(string type, (string Repo, string Loca) address);
    }
}