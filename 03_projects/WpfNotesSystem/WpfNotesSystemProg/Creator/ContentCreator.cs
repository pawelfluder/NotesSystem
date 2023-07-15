using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfNotesSystem.Creator
{
    public class ContentCreator : IContentCreator
    {
        private readonly Grid table;

        public ContentCreator(Grid grid)
        {
            table = grid;
        }

        public void CreateRowsAndColls(int jmax, int imax)
        {
            for (int j = 0; j < jmax; j++)
            {
                var row = new RowDefinition();
                //row.Height = new GridLength(80);
                table.RowDefinitions.Add(row);
            }

            for (int i = 0; i < imax; i++)
            {
                var col = new ColumnDefinition();
                if (i < imax - 1)
                {
                    col.Width = new GridLength(20);
                }
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
        }

        public void CreateHeader((int, int) pos, string text, int collSpan)
        {
            TextBlock txt1 = new TextBlock();
            txt1.Text = text;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, pos.Item1);
            Grid.SetColumn(txt1, pos.Item2);
            Grid.SetColumnSpan(txt1, collSpan);
            table.Children.Add(txt1);
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
