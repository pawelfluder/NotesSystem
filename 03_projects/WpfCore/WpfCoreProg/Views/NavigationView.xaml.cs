using WpfNotesSystem.ViewModels;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Unity;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public NavigationView()
        {
            InitializeComponent();

            DataContext = MyBorder.Container.Resolve<MainViewModel>();
        }

        private void GoButtonClick(object sender, RoutedEventArgs e)
        {
            var addressTextBox = FindName("Address") as TextBox;
            var url = addressTextBox.Text;

            var address = CreateAddressFromUrl(url);

            var mainViewModel = DataContext as MainViewModel;
            mainViewModel.GoAction(address);
        }

        private (string, string) CreateAddressFromUrl(string address)
        {
            address = address.Trim('/');
            var index = address.IndexOf('/');
            if (!address.Contains('/'))
            {
                return (address, "");
            }

            var repo = address.Substring(0, index);
            var loca = address.Substring(index + 1, address.Length - index - 1);
            return (repo, loca);
        }

        private void GoButtonClick2(object sender, RoutedEventArgs e)
        {
            var repoTextBox = FindName("RepoName") as ComboBox;
            var locaTextBox = FindName("Location") as TextBox;

            //var repo = repoTextBox.SelectedItem.ToString();

            //var mainViewModel = DataContext as MainViewModel;
            //var address = (repo, locaTextBox.Text);
            //mainViewModel.GoAction(address);
        }

        private void BackArrowClick(object sender, RoutedEventArgs e)
        {
            var mainViewModel = DataContext as MainViewModel;
            mainViewModel.BackArrow();
        }

        private void ForwardArrowClick(object sender, RoutedEventArgs e)
        {
            var mainViewModel = DataContext as MainViewModel;
            mainViewModel.ForwardArrow();
        }
    }
}
