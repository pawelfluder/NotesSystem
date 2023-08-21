using SharpRepoBackendProg.Service;
using SwitchingViewsMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;
using WpfNotesSystem.Repetition;

namespace SwitchingViewsMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = MyBorder.Container.Resolve<MainViewModel>();
        }

        private void GoButtonClick(object sender, RoutedEventArgs e)
        {
            var repoTextBox = FindName("RepoName") as TextBox;
            var locaTextBox = FindName("Location") as TextBox;

            var mainViewModel = DataContext as MainViewModel;
            var address = (repoTextBox.Text, locaTextBox.Text);
            mainViewModel.GoAction(address);
        }
    }
}
