﻿using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using YamlDotNet.Serialization;
using static SharpFileServiceProg.Service.IFileService;

namespace SharpRepoServiceProg.WorkersSystem
{
    internal class ConfigWorker
    {
        private readonly IFileService fileService;
        private readonly IYamlOperations yamlOperations;
        private readonly PathWorker pw;
        private readonly SystemWorker sw;
        public object ErrorValue { get; internal set; }

        public ConfigWorker()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            yamlOperations = fileService.Yaml.Custom03;
            pw = MyBorder.Container.Resolve<PathWorker>();
            sw = MyBorder.Container.Resolve<SystemWorker>();
        }

        public bool TryGetConfigLines(
            (string Repo, string Loca) address,
            out List<string> lines)
        {
            try
            {
                lines = GetConfigLines(address);
                return true;
            }
            catch
            {
                lines = null;
                return false;
            }
        }

        public string GetConfigText(
            (string Repo, string Loca) adrTuple)
        {
            var configFilePath = pw.GetConfigPath(adrTuple);
            var configLines = File.ReadAllLines(configFilePath);
            var configText = string.Join(sw.NewLine, configLines);
            return configText;
        }

        public List<string> GetConfigLines(
            (string Repo, string Loca) adrTuple)
        {
            var configFilePath = pw.GetConfigPath(adrTuple);
            var configLines = File.ReadAllLines(configFilePath).ToList();
            return configLines;
        }

        public void AddSettingsToModel(
            ItemModel model,
            (string Repo, string Loca) adrTuple,
            Dictionary<string, object> settings)
        {
            var newAddress = fileService.RepoAddress.CreateUrlFromAddress(adrTuple);
            settings[Fields_Item.Address] = newAddress;
            var saveNeeded = false;
            if (!settings.ContainsKey(Fields_Item.Id))
            {
                settings[Fields_Item.Id] = Guid.NewGuid().ToString();
                saveNeeded = true;
            }
            if (!settings.ContainsKey(Fields_Item.Type))
            {
                var type = AssumeType(adrTuple);
                settings[Fields_Item.Type] = type;
                saveNeeded = true;
            }
            model.Settings = settings;

            if (saveNeeded)
            {
                CreateConfig(adrTuple, settings);
            }
        }

        public string AssumeType(
            (string repo, string loca) adrTuple)
        {
            var contentFilePath = pw.GetBodyPath(adrTuple);
            if (File.Exists(contentFilePath))
            {
                return ItemTypes.Text;
            }

            return ItemTypes.Folder;
        }

        public void CreateConfig(
            (string Repo, string Loca) adrTuple,
            Dictionary<string, object> dict)
        {
            var nameFilePath = pw.GetConfigPath(adrTuple);
            var content = yamlOperations.Serialize(dict);
            File.WriteAllText(nameFilePath, content);
        }

        public void CreateConfig(
            (string Repo, string Loca) adrTuple,
            List<string> contentLines)
        {
            var nameFilePath = pw.GetConfigPath(adrTuple);
            var content = string.Join(sw.NewLine, contentLines);
            File.WriteAllText(nameFilePath, content);
        }

        public Dictionary<string, object> GetConfigDictionary(
            (string Repo, string Loca) adrTuple)
        {
            var configItemPath = pw.GetConfigPath(adrTuple);
            var dict = yamlOperations.DeserializeFile<Dictionary<string, object>>(configItemPath);
            return dict;
        }

        public void CreateConfigKey(
            (string Repo, string Loca) address,
            string key,
            object value)
        {
            var dict = GetConfigDictionary(address);
            var exists = dict.TryGetValue(key, out var tmp);
            if (exists)
            {
                dict[key] = value;
            }

            if (!exists)
            {
                dict.Add(key, value);
                try
                {
                    CreateConfig(address, dict);
                }
                catch { };
            }
        }



        //public void CreateConfig(
        //    string itemPath,
        //    Dictionary<string, object> dict)
        //{
        //    var nameFilePath = itemPath + slash + configFileName;
        //    var content = yamlOperations.Serialize(dict);
        //    File.WriteAllText(nameFilePath, content);
        //}
    }
}