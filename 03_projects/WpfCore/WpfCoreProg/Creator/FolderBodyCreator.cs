using SharpFileServiceProg.Service;
using WpfNotesSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystem.Creator
{
    public class FolderBodyCreator
    {
        private readonly IFileService fileService;
        private readonly Grid table;
        private readonly MainViewModel mainViewModel;

        public FolderBodyCreator(
            Grid grid,
            IFileService fileService)
        {
            this.fileService = fileService;
            table = grid;
            mainViewModel = MyBorder.Container.Resolve<MainViewModel>();
        }

        public void Run(Dictionary<string, string> indexQnameDict)
        {
            if (indexQnameDict.Count() == 0)
            {
                return;
            }

            var jmax = indexQnameDict.Count;
            var imax = 2;

            for (int j = 0; j < jmax; j++)
            {
                var row = new RowDefinition();
                table.RowDefinitions.Add(row);
            }

            for (int i = 0; i < imax; i++)
            {
                var col = new ColumnDefinition();
                table.ColumnDefinitions.Add(col);
            }

            table.ColumnDefinitions[0].Width = new GridLength(23);

            //var column01Style = Application.Current.Resources["Converter_Column01"] as Style;
            //if (column01Style != null)
            //{
            //    table.ColumnDefinitions[0].Style = column01Style;
            //}

            table.HorizontalAlignment = HorizontalAlignment.Left;
            table.VerticalAlignment = VerticalAlignment.Top;
            table.ShowGridLines = false;
            var border = CreateBorder();
            Grid.SetRowSpan(border, jmax);
            Grid.SetColumnSpan(border, imax);
            table.Children.Add(border);

            for (int j = 0; j < indexQnameDict.Count; j++)
            {
                var indexQname = indexQnameDict.ElementAt(j);
                CreateFolderLine(j, indexQname.Key, indexQname.Value);
            }

            //table.Background = Brushes.Yellow;
        }

        public Border CreateBorder()
        {
            var border = new Border();
            border.BorderBrush = Brushes.Gray;
            return border;
        }

        public void CreateFolderLine(int j, string indexString, string name)
        {
            TextBlock txt1 = new TextBlock();
            txt1.Text = indexString;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            txt1.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(txt1, j);
            Grid.SetColumn(txt1, 0);
            Grid.SetColumnSpan(txt1, 1);
            table.Children.Add(txt1);

            if (mainViewModel.AdrTuple.Loca.Contains("//"))
            {
                throw new Exception();
            }

            Hyperlink hyperlink = new Hyperlink(new Run(name));
            hyperlink.FontFamily = new FontFamily("Arial");

            var index = int.Parse(indexString);
            hyperlink.NavigateUri = fileService.RepoAddress.
                CreateUriFromAddress(mainViewModel.AdrTuple, index);
            hyperlink.RequestNavigate += HyperlinkRequestNavigate;

            TextBlock txt2 = new TextBlock();
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            txt2.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(txt2, j);
            Grid.SetColumn(txt2, 1);
            Grid.SetColumnSpan(txt2, 1);

            txt2.Inlines.Add(hyperlink);
            table.Children.Add(txt2);
        }

        private void HyperlinkRequestNavigate(
            object sender, RequestNavigateEventArgs e)
        {
            if (sender is Hyperlink hyperLink)
            {
                var addressString = hyperLink.NavigateUri.OriginalString
                    .Replace("https://", string.Empty);

                var address = fileService.RepoAddress
                    .CreateAddressFromString(addressString);
                mainViewModel.GoAction(address);
            }
        }

        public void CreateLines((int, int) pos, string line, int collSpan)
        {
            TextBlock txt1 = new TextBlock();
            txt1.Text = line;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, pos.Item1);
            Grid.SetColumn(txt1, pos.Item2);
            Grid.SetColumnSpan(txt1, collSpan);
            table.Children.Add(txt1);
        }
    }
}
