using SwitchingViewsMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Unity;
using WpfNotesSystem.Repetition;

namespace WpfNotesSystem.Creator
{
    public class FolderBodyCreator
    {
        private readonly Grid table;
        private readonly MainViewModel mainViewModel;

        public FolderBodyCreator(Grid grid)
        {
            table = grid;
            mainViewModel = MyBorder.Container.Resolve<MainViewModel>();
        }

        public void Run(Dictionary<string, string> indexQnameDict)
        {
            var jmax = indexQnameDict.Count;
            var imax = 3;

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

            //table.Width = 500;
            //table.Height = 500;
            table.HorizontalAlignment = HorizontalAlignment.Left;
            table.VerticalAlignment = VerticalAlignment.Top;
            table.ShowGridLines = false;
            var border = new Border();
            border.BorderThickness = new Thickness(3);
            border.BorderBrush = Brushes.Gray;
            Grid.SetRowSpan(border, jmax);
            Grid.SetColumnSpan(border, imax);
            table.Children.Add(border);

            for (int j = 0; j < indexQnameDict.Count; j++)
            {
                var indexQname = indexQnameDict.ElementAt(j);
                CreateFolderLine(j, indexQname.Key, indexQname.Value);
            }
        }

        public void CreateFolderLine(int j, string index, string name)
        {
            // index
            TextBlock txt1 = new TextBlock();
            txt1.Text = index;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, j);
            Grid.SetColumn(txt1, 0);
            Grid.SetColumnSpan(txt1, 1);
            table.Children.Add(txt1);

            // link

            //hyperlink.Click += GoItem

            Hyperlink hyperlink = new Hyperlink(new Run(name));
            var address = mainViewModel.Address.Repo +
                "/" + mainViewModel.Address.Loca +
                "/" + index;
            var url = "https://" + address;
            string url2 = "https://www.Example.com";
            var uri = new Uri(url);

            hyperlink.NavigateUri = uri;
            hyperlink.RequestNavigate += HyperlinkRequestNavigate;

            TextBlock txt2 = new TextBlock();
            //txt2.Text = name;
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
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
                var tmp = hyperLink.NavigateUri.OriginalString
                    .Replace("https://", "");
                var index = tmp.IndexOf('/');
                var repo = tmp.Substring(0, index);
                var loca = tmp.Substring(index + 1, tmp.Length - index - 1);
                mainViewModel.GoAction((repo, loca));
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

        public void CreateEmpty((int, int) pos)
        {
        }
    }
}
