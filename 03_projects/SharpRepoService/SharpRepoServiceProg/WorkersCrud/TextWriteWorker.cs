using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.WorkersSystem;
using SharpTinderComplexTests;
using System;
using System.Collections.Generic;
using SharpRepoServiceProg.AAPublic.Names;

namespace SharpRepoServiceProg.Workers
{
    public class TextWriteWorker
    {
        private readonly PathWorker pw;
        private readonly SystemWorker sw;
        private readonly ConfigWorker cw;
        private readonly BodyWorker bw;
        private readonly ReadWorker rw;
        private readonly IFileService fileService;

        public TextWriteWorker()
        {
            this.rw = MyBorder.Container.Resolve<ReadWorker>();
            this.bw = MyBorder.Container.Resolve<BodyWorker>();
            this.cw = MyBorder.Container.Resolve<ConfigWorker>();
            this.sw = MyBorder.Container.Resolve<SystemWorker>();
            this.fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public void Put(
            string name,
            (string Repo, string Loca) adrTuple,
            string content)
        {
            var item = new ItemModel();

            // config
            var address = fileService.RepoAddress.CreateUrlFromAddress(adrTuple);
            item.Settings = new Dictionary<string, object>()
            {
                { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
                { FieldsForUniItem.Type, ItemTypeNames.Text },
                { FieldsForUniItem.Name, name },
                { FieldsForUniItem.Address, address }
            };

            // body
            item.Body = content;

            Put(item);
        }

        internal ItemModel Put(ItemModel item)
        {
            // directory
            sw.CreateDirectoryIfNotExists(item.AdrTuple);

            // config
            cw.CreateConfig(item.AdrTuple, item.Settings);

            // body
            bw.CreateBody(item.AdrTuple, item.Body.ToString());

            return item;
        }

        public (string Repo, string Loca) Post(
            string name,
            (string Repo, string Loca) adrTuple,
            string content)
        {
            var item = InternalPost(name, adrTuple, content);
            return item.AdrTuple;
        }

        internal ItemModel InternalPost(
            string name,
            (string Repo, string Loca) adrTuple,
            string content)
        {
            ItemModel item = null;
            var foundAdrTuple = rw.GetAdrTupleByName(adrTuple, name);
            if (foundAdrTuple != default)
            {
                item = rw.GetItemConfig(foundAdrTuple);
                item.Body = content;
                Put(name, foundAdrTuple, content);
                return item;
            }

            var lastIndex = rw.GetFolderLastNumber(adrTuple);
            var newIndex = lastIndex + 1;
            var newIndexString = fileService.Index.IndexToString(newIndex);

            var newAdrTuple = fileService.RepoAddress.AdrTupleJoinLoca(adrTuple, newIndexString);
            item = PrepareItem(name, newAdrTuple, content);
            Put(item);
            return item;
        }

        public void Patch(
            string content,
            (string Repo, string Loca) adrTuple)
        {
            var item = rw.GetItem(adrTuple);
            if (item == default)
            {
                throw new Exception();
            }

            item.Body = content;
            Put(item);
        }

        public (string, string) Append(
            (string Repo, string Loca) address,
            string name,
            string content)
        {
            var existingItem = rw.GetAdrTupleByName(address, name);
            if (existingItem != default)
            {
                Append(existingItem, content);
                return existingItem;
            }

            var lastNumber = rw.GetFolderLastNumber(address);
            var newAddress = fileService.Index.SelectAddress(address, lastNumber + 1);
            Put(name, newAddress, content);
            return newAddress;
        }

        public void Append(
            (string Repo, string Loca) address,
            string content)
        {
            // todo
            //AppendTextGenerate(address, content);
        }

        private ItemModel PrepareItem(
            string name,
            (string Repo, string Loca) adrTuple,
            string content)
        {
            var item = new ItemModel();

            // config
            var settings = new Dictionary<string, object>()
            {
                { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
                { FieldsForUniItem.Type, ItemTypeNames.Text },
                { FieldsForUniItem.Name, name },
            };
            cw.AddSettingsToModel(item, adrTuple, settings);

            // body
            item.Body = content;

            return item;
        }
    }
}