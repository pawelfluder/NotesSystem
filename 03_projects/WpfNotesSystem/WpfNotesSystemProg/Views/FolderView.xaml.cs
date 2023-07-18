using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfNotesSystemProg.ViewModels;

namespace WpfNotesSystemProg.Views
{
    public partial class FolderView : Window
    {
        public FolderView()
        {
            InitializeComponent();
            DataContext = new FolderViewModel();
        }
    }
}
