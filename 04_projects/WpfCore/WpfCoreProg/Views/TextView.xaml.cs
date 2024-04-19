using SharpRepoBackendProg.Service;
using WpfNotesSystem.ViewModels;
using System.Windows.Controls;
using Unity;
using WpfNotesSystem.Repetition;
using OutBorder1 = SharpRepoBackendProg.Repetition.OutBorder;

namespace WpfNotesSystem.Views
{
    public partial class TextView : UserControl
    {
        private readonly IBackendService backendService;

        public TextView()
        {
            InitializeComponent();
            var tmp = MyBorder.Container.Resolve<MainViewModel>();
            //tmp.SelectedViewModel.View = this;
            tmp.BodyView = this;
            //DataContext = tmp.SelectedViewModel;
            DataContext = tmp.SelectedTab.ViewModel;
            tmp.SelectedTab.ViewModel.View = this;

            // DataContext = MyBorder.Container.Resolve<TextViewModel>();
        }

        //private void CreateContent((string Repo, string Loca) address)
        //{
        //    var myGrid = FindName("ContentGrid") as Grid;
        //    ClearGrid(myGrid);

        //    var item = backendService.RepoApi(address.Item1, address.Item2);
        //    if (item.Contains("error")) { return; }
        //    var fileService = MyBorder.Container.Resolve<IFileService>();
        //    var gg = fileService.Yaml.Custom03
        //        .Deserialize<Dictionary<string, object>>(item);

        //    try
        //    {
        //        var name = gg["Name"].ToString();
        //        var content = gg["Content"];

        //        var creator = new ContentCreator(myGrid);
        //        var contentManager = new ContentManager(fileService);
        //        contentManager.Run(creator, content);
        //    }
        //    catch { }
        //}

        

        //private void GoButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var repoTextBox = FindName("RepoName") as TextBox;
        //    var locaTextBox = FindName("Location") as TextBox;

        //    var address = (repoTextBox.Text, locaTextBox.Text);

        //    var type = backendService.RepoApi("GetItemType", address.Item1, address.Item2);
            

        //    var viewModel = DataContext as TextViewModel;
        //    viewModel.CurrentAddress = address;
        //    CreateContent(address);
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
