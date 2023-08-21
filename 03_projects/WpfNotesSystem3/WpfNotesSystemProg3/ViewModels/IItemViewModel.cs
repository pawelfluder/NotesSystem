namespace SwitchingViewsMVVM.ViewModels
{
    public interface IItemViewModel
    {
        void GoAction(string type, (string Repo, string Loca) address);
    }
}