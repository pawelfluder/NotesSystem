using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;
using WpfNotesSystem.Creator;
using WpfNotesSystem.Repetition;
using OutBorder1 = SharpRepoBackendProg.Repetition.OutBorder;

namespace WpfNotesSystem
{
    public partial class MainWindow : Window
    {
        private readonly IBackendService backendService;

        public MainWindow()
        {
            InitializeComponent();
            backendService = OutBorder1.BackendService();
            DataContext = new MainViewModel(backendService);



            //var repo = repoTextBox.Text;
            //var loca = locaTextBox.Text;
            var repo = "Sprawy";
            var loca = "01/02";

            CreateContent((repo, loca));

            var gg5 = 0;
        }

        private void CreateContent((string Repo, string Loca) address)
        {
            var myGrid = this.FindName("ContentGrid") as Grid;
            ClearGrid(myGrid);

            var item = backendService.RepoApi(address.Item1, address.Item2);
            var fileService = MyBorder.Container.Resolve<IFileService>();
            var gg = fileService.Yaml.Custom03
                .Deserialize<Dictionary<string, object>>(item);

            try
            {
                var name = gg["Name"].ToString();
                var content = gg["Content"];


                var creator = new ContentCreator(myGrid);
                var contentManager = new ContentManager(fileService);
                contentManager.Run(creator, content);
            }
            catch { }
        }

        private static void ClearGrid(Grid? myGrid)
        {
            var rowsCount = myGrid.RowDefinitions.Count();
            if (rowsCount > 0)
            {
                myGrid.RowDefinitions.RemoveRange(0, rowsCount);
            }
            var collsCount = myGrid.ColumnDefinitions.Count();
            if (collsCount > 0)
            {
                myGrid.ColumnDefinitions.RemoveRange(0, collsCount);
            }

            var childrenCount = myGrid.Children.Count;
            myGrid.Children.Clear();
        }

        private void GoButtonClick(object sender, RoutedEventArgs e)
        {
            var repoTextBox = this.FindName("RepoName") as TextBox;
            var locaTextBox = this.FindName("Location") as TextBox;

            var address = (repoTextBox.Text, locaTextBox.Text);
            var viewModel = (DataContext as MainViewModel);
            viewModel.CurrentAddress = address;
            CreateContent(address);
        }
    }
}
