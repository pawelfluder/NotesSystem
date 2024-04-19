using System.Windows.Controls;
using System.Windows;

namespace WpfNotesSystemProg3.Controls
{

public class SelectableTextBox : TextBox
    {
        public SelectableTextBox()
        {
            Background = System.Windows.Media.Brushes.Transparent;
            BorderThickness = new Thickness(1);
            SetBinding(TextProperty, new System.Windows.Data.Binding("Text") { Mode = System.Windows.Data.BindingMode.OneWay });
            IsReadOnly = true;
            TextWrapping = TextWrapping.Wrap;
            Text = "gg";
        }
    }
}
