using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Unity;
using WpfNotesSystem.Creator;
using WpfNotesSystem.Repetition;
using WpfNotesSystemProg3.Models;

namespace WpfNotesSystemProg.Converter
{
    [ValueConversion(typeof(RepoItem), typeof(StackPanel))]
    public class HeadersDictConverter : MarkupExtension, IValueConverter
    {
        private object converter;
        private readonly IFileService fileService;

        public HeadersDictConverter()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null)
            {
                converter = new HeadersDictConverter();
            }

            return converter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            var itemModel = value as RepoItem;
            Grid myGrid = null;
            StackPanel stackPanel = null;
            try
            {
                var type = itemModel.Type;
                if (type == "Text")
                {
                    myGrid = ConvertTextItem(itemModel);
                }
                if (type == "Folder")
                {
                    myGrid = ConvertFolderItem(itemModel);
                }

                //var resourceDict = new ResourceDictionary();
                //resourceDict.Source = new Uri("Style/AppResources.xaml",
                //    UriKind.RelativeOrAbsolute);

                //var buttonStyle = resourceDict["Converter_StackPanel"] as Style;


                var width = 600;

                //myGrid.MinWidth = width;
                //myGrid.MaxWidth = width;
                stackPanel = new StackPanel();
                if (myGrid != null)
                {
                    stackPanel.Children.Add(myGrid);
                }
                

                //stackPanel.MinWidth = width;
                //stackPanel.MaxWidth = width;

                var stackPanelStyle = Application.Current.Resources["Converter_StackPanel"] as Style;
                if (stackPanelStyle != null)
                {
                    stackPanel.Style = stackPanelStyle;
                }

                var gridPanelStyle = Application.Current.Resources["Converter_Grid"] as Style;
                if (gridPanelStyle != null)
                {
                    myGrid.Style = gridPanelStyle;
                }



                //stackPanel.Style = new System.Windows.Style();

                //var w = 150;
                //var h = 150;
                ////myGrid.MinHeight = h;
                ////myGrid.MinWidth = w;
                //if (myGrid.Height < h )//||
                //    //Double.IsNaN(myGrid.Height))
                //{
                //    myGrid.Height = h;
                //}
                //if (myGrid.Width < w )//||
                //    //Double.IsNaN(myGrid.Width))
                //{
                //    myGrid.Width = w;
                //}
                //stackPanel.Height = myGrid.Height;
                //stackPanel.Width = myGrid.Width;

                //stackPanel.MinHeight = h;
                //stackPanel.MinWidth = w;

                //stackPanel.Width = 500;
            }
            catch { }
            return stackPanel;
        }

        private Grid ConvertFolderItem(RepoItem itemModel)
        {
            var grid = new Grid();
            //var body = itemModel.Body as Dictionary<string, string>;

            if (itemModel.Body == null)
            {
                // log error
            }

            var indexQnameDict = JsonConvert
                .DeserializeObject<Dictionary<string, string>>(itemModel.Body.ToString());
            var creator = new FolderBodyCreator(grid, fileService);
            creator.Run(indexQnameDict);
            
            //var indexQnameList = tmp.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
            //var indexQnameList = tmp.Select(kv => (kv.Key, kv.Value)).ToDictionary(x => x.Key);

            return grid;
        }

        private Grid ConvertTextItem(RepoItem dict)
        {
            var grid = new Grid();

            var creator = new ContentCreator(grid);
            var contentManager = new ContentManager(fileService);

            if (dict.Body == null)
            {
                // log error
            }

            var tmp = dict.Body.ToString();
            var lines = tmp.Split('\n').Skip(4).ToArray();

            contentManager.Run(creator, lines);

            return grid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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
    }
}