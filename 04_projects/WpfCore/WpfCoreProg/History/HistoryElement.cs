namespace WpfNotesSystemProg3.History
{
    internal class HistoryElement : IHistoryElement
    {
        public HistoryElement(object value)
        {
            Value = value;
        }
        public object Value { get; }
    }
}
