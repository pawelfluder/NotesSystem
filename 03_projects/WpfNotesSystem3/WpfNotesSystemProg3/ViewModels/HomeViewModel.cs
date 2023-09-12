using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Unity;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;

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
            var jsonString = backendService.RepoApi(address.Item1, address.Item2);
            ItemModel2 jObj = jObj = JsonConvert.DeserializeObject<ItemModel2>(jsonString);            
            
            //if (error != null) { return; }
            //var headersDict = fileService.Yaml.Custom03
            //  .Deserialize<Dictionary<string, object>>(item);

            //headersDict.Add("Type", type);

            //HeadersDict = new Dictionary<string, object>();
            HeadersDict = jObj;
        }

        private ItemModel2 headersDict;

        public ItemModel2 HeadersDict
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
