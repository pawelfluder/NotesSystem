


using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfNotesSystemProg3.Controls
{
    public class SelectableRichTextBlock : RichTextBox
    {
        public SelectableRichTextBlock()
        {
            IsReadOnly = true;
            IsDocumentEnabled = true;
            BorderThickness = new System.Windows.Thickness(1);
            Background = Brushes.Transparent;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Width = 1000;// double.NaN;
            Height = 20; // double.NaN;

        }

        public void AddText(string text)
        {
            Paragraph paragraph = new Paragraph(new Run(text));
            Document.Blocks.Add(paragraph);
        }

        public void AddHyperlink(Hyperlink hyperlink)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(hyperlink);
            Document.Blocks.Add(paragraph);
        }
    }
}
