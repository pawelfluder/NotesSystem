using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

            table.HorizontalAlignment = HorizontalAlignment.Left;
            table.VerticalAlignment = VerticalAlignment.Top;
            table.ShowGridLines = false;
            var border = CreateBorder();
            Grid.SetRowSpan(border, jmax);
            Grid.SetColumnSpan(border, imax);
            table.Children.Add(border);
        }

        public Border CreateBorder()
        {
            var border = new Border();
            //border.BorderThickness = new Thickness(3);
            border.BorderBrush = Brushes.Gray;
            return border;
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
            txt1.TextWrapping = TextWrapping.Wrap;
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            var pattern2 = @"\bhttps?://\S+";
            var match = Regex.Match(line, pattern2);

            var groupsCount = match.Captures.Count;
            if (groupsCount > 0)
            {
                var captured = match.Captures[0].Value;
                var tmp = line.Split(captured).ToList();

                var tmp2 = new List<string>();
                for (int i = 0; i < tmp.Count(); i++)
                {
                    tmp2.Add(tmp[i]);
                    if (i != tmp.Count() - 1)
                    {
                        tmp2.Add(captured);
                    }
                }

                foreach (var item in tmp2)
                {
                    if (item == captured)
                    {
                        var hyperlink = new Hyperlink(new Run(captured));
                        hyperlink.NavigateUri = new Uri(captured);
                            
                        hyperlink.RequestNavigate += (s, e) =>
                        {
                            var hyperLink = (Hyperlink)s;
                            var destinationurl = hyperLink.NavigateUri.OriginalString;
                            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
                            {
                                UseShellExecute = true,
                            };
                            System.Diagnostics.Process.Start(sInfo);

                        };
                        txt1.Inlines.Add(hyperlink);
                        //txt1.AddHyperlink(hyperlink);
                    }
                    else
                    {
                        txt1.Inlines.Add(item);
                        //txt1.AddText(item);
                    }
                }
            }

            if (!(groupsCount > 0))
            {
                txt1.Inlines.Add(line);
                //txt1.AddText(line);
            }

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
