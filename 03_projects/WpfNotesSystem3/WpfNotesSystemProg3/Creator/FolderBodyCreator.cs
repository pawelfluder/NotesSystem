using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfNotesSystem.Creator
{
    public class FolderBodyCreator
    {
        private readonly Grid table;

        public FolderBodyCreator(Grid grid)
        {
            table = grid;
        }

        public void Run(Dictionary<string, object> indexQnameList)
        {
            var jmax = indexQnameList.Count;
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

            table.Width = 500;
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

            for (int j = 0; j < indexQnameList.Count; j++)
            {
                //var indexQname = indexQnameList[j];
                //CreateFolderLine(j, indexQname);
            }
        }

        public void CreateFolderLine(int j, (string, object) indexQname)
        {
            // index
            TextBlock txt1 = new TextBlock();
            txt1.Text = indexQname.Item1;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, j);
            Grid.SetColumn(txt1, 0);
            Grid.SetColumnSpan(txt1, 1);
            table.Children.Add(txt1);

            // link
            TextBlock txt2 = new TextBlock();
            txt2.Text = indexQname.Item2.ToString();
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt2, j);
            Grid.SetColumn(txt2, 1);
            Grid.SetColumnSpan(txt2, 1);
            table.Children.Add(txt2);
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
