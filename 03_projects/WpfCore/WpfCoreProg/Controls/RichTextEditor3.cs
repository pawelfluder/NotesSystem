using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfNotesSystemProg3.Controls
{
    public class RichTextEditor3 : RichTextBox
    {
        private Paragraph paragraph;

        public RichTextEditor3()
        {
            // Inicjalizacja kontrolki
            InitializeRichTextEditor();
            Width = 1000; // double.NaN;
            Height = 20; // double.NaN;
            IsReadOnly = true;
            IsDocumentEnabled = true;
            BorderThickness = new System.Windows.Thickness(1);
            Background = Brushes.Transparent;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;

            IsEnabled = false;


        }

            private void InitializeRichTextEditor()
        {
            // Utwórz nowy FlowDocument
            FlowDocument document = new FlowDocument();
            Document = document;
            paragraph = new Paragraph();
            Document.Blocks.Add(paragraph);
        }

        public void AddText(string text)
        {
            paragraph.Inlines.Add(text);
        }

        public void AddHyperlink(Hyperlink hyperlink)
        {
            paragraph.Inlines.Add(hyperlink);
            
        }
    }
}
