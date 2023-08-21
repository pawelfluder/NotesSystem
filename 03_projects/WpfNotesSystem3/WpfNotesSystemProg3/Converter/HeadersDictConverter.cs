using SharpFileServiceProg.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Unity;
using WpfNotesSystem.Creator;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystemProg.Converter
{
    [ValueConversion(typeof(Dictionary<string, object>), typeof(Grid))]
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
            Grid myGrid = null;
            try
            {
                var dict = value as Dictionary<string, object>;
                var type = dict["Type"].ToString();
                if (type == "Text")
                {
                    myGrid = ConvertTextItem(dict);
                }
                if (type == "Folder")
                {
                    myGrid = ConvertFolderItem(dict);
                }
            }
            catch { }
            return myGrid;
        }

        private Grid ConvertFolderItem(Dictionary<string, object> dict)
        {
            var grid = new Grid();
            var name = dict["Name"].ToString();
            var body = dict["Body"];
            var tmp = body as List<object>;
            //var indexQnameList = tmp.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
            //var indexQnameList = tmp.Select(kv => (kv.Key, kv.Value)).ToDictionary(x => x.Key);

            var creator = new FolderBodyCreator(grid);
            //creator.Run(indexQnameList);
            return grid;
        }

        private Grid ConvertTextItem(Dictionary<string, object> dict)
        {
            var grid = new Grid();
            var name = dict["Name"].ToString();
            var content = dict["Body"];

            var creator = new ContentCreator(grid);
            var contentManager = new ContentManager(fileService);
            contentManager.Run(creator, content);
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