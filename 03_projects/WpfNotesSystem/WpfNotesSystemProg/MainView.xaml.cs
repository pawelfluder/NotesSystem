using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfNotesSystemProg.ViewModels;

namespace WpfNotesSystemProg
{
    public partial class MainView : Window
    {
        private readonly IBackendService backendService;

        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
