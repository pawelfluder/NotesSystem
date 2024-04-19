using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfNotesSystemProg3.Controls
{
    public class SelectableTextBlock2 : TextBlock
    {
        private TextSelectionAdorner textSelectionAdorner;

        public SelectableTextBlock2()
        {
            Background = Brushes.Transparent;

            // W momencie dołączenia kontrolki do drzewa wizualnego
            this.Loaded += (s, e) =>
            {
                // Inicjalizacja adornera
                textSelectionAdorner = new TextSelectionAdorner(this);
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    adornerLayer.Add(textSelectionAdorner);
                }
            };
        }

        public void AddText(string text)
        {
            Inlines.Add(new Run(text));
        }

        public void AddHyperlink(string text, string url)
        {
            Hyperlink hyperlink = new Hyperlink(new Run(text));
            hyperlink.NavigateUri = new Uri(url);
            Inlines.Add(hyperlink);
        }
    }

    public class TextSelectionAdorner : Adorner
    {
        private Point startPoint;
        private Point endPoint;
        private Pen selectionPen;
        private SolidColorBrush selectionBrush;

        public TextSelectionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            selectionBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));
            selectionPen = new Pen(selectionBrush, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (IsMouseCaptured)
            {
                var selectionGeometry = new RectangleGeometry(new Rect(startPoint, endPoint));
                drawingContext.DrawGeometry(selectionBrush, selectionPen, selectionGeometry);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (IsMouseCaptured)
            {
                return;
            }
            CaptureMouse();
            startPoint = e.GetPosition(this);
            endPoint = startPoint;
            InvalidateVisual();
        }

        protected override void OnPreviewMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (IsMouseCaptured)
            {
                endPoint = e.GetPosition(this);
                InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
        }
    }
}
