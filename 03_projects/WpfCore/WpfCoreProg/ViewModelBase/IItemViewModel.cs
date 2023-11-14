using System.Windows.Controls;
using WpfNotesSystemProg3.Models;

namespace WpfNotesSystemProg3.ViewModelBase
{
    public interface IItemViewModel
    {
        void GoAction();
        string Address { get; set; }
        string ItemType { get; }
        RepoItem RepoItem { get; }
        UserControl View { get; set; }
    }
}