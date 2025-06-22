using System.ComponentModel;

namespace WpfCoreProg.Debug;

public class DebugState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public DebugState()
    {
        Visibility = "Hidden";
        //Visibility = "Visible";
        MinWidth = 0;
    }

    public int MinWidth { get; set; }

    private string _visibility;
    public string Visibility
    {
        get => _visibility;
        set
        {
            _visibility = value;
            OnPropertyChanged(nameof(Visibility));
        }
    }

    private string _body;
    public string Body
    {
        get => _body;
        set
        {
            _body = value;
            OnPropertyChanged(nameof(Body));
        }
    }
}