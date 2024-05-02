using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Registration;
using System.Collections.Generic;

namespace SharpRepoServiceProg.Models
{
    public class ItemModel
    {
        private string address;

        private Dictionary<string, object> settings;

        internal string Name { get; set; }

        internal string Type { get; set; }

        internal string Id { get; set; }

        internal string Address
        {
            get => address;
            set
            {
                address = value;
                var fileService = MyBorder.Container.Resolve<IFileService>();
                var adrTuple = fileService.RepoAddress.CreateAddressFromString(address);
                AdrTuple = adrTuple;
            }
        }

        internal (string Repo, string Loca) AdrTuple { get; set; }

        public object Body { get; set; }

        public Dictionary<string, object> Settings
        {
            get => settings;
            set
            {
                settings = value;
                SetIndentificators(settings);
            }
        }

        private void SetIndentificators(
            Dictionary<string, object> dict)
        {
            Name = dict[ItemFields.Name].ToString();
            Id = dict[ItemFields.Id].ToString();
            Type = dict[ItemFields.Type].ToString();
            Address = dict[ItemFields.Address].ToString();
        }
    }
}