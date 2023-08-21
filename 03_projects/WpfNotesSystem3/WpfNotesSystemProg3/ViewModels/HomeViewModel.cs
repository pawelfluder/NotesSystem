using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using Unity;
using WpfNotesSystem.Repetition;

namespace SwitchingViewsMVVM.ViewModels
{
    public class HomeViewModel : BaseViewModel, IItemViewModel
    {
        private readonly IBackendService backendService;
        private readonly IFileService fileService;

        //private readonly MainViewModel mainViewModel;

        public HomeViewModel()
        {
            backendService = MyBorder.Container.Resolve<IBackendService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public void GoAction(string type, (string Repo, string Loca) address)
        {
            //backendService.RepoApi(address.Repo, address.Loca);
            var item = backendService.RepoApi(address.Item1, address.Item2);
            if (item.Contains("error")) { return; }
            var headersDict = fileService.Yaml.Custom03
                .Deserialize<Dictionary<string, object>>(item);

            //headersDict.Add("Type", type);

            HeadersDict = headersDict;
        }

        private Dictionary<string, object> headersDict;
        public Dictionary<string, object> HeadersDict
        {
            get => headersDict;
            private set
            {
                headersDict = value;
                OnPropertyChanged(nameof(HeadersDict));
            }
        }
    }
}
