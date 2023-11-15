using WpfNotesSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystem.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //if(parameter.ToString() == "Home")
            //{
            //    viewModel.SelectedViewModel = MyBorder.Container.Resolve<FolderViewModel>();
            //}
            //else if(parameter.ToString() == "Account")
            //{
            //    viewModel.SelectedViewModel = MyBorder.Container.Resolve<TextViewModel>();
            //}
        }
    }
}
