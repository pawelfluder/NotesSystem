using System.Collections.Generic;
using System.Linq;

namespace WpfNotesSystemProg3.History
{
    internal class PropertyHistory
    {
        private List<IHistoryElement> undoStack;
        private List<IHistoryElement> redoStack;
        private int currentIndex;
        private object _property;
        private object Property
        {
            get => _property;
            set
            {
                _property = value;
            }
        }
        public bool suppressAdding;

        public PropertyHistory(object property)
        {
            this.Property = property;
            suppressAdding = false;
            undoStack = new List<IHistoryElement>();
            redoStack = new List<IHistoryElement>();
            Add(property);
        }

        public void SuppressAdding()
        {
            suppressAdding = true;
        }

        public void UnSuppressAdding()
        {
            suppressAdding = false;
        }

        public void Add(object obj)
        {
            var elem = new HistoryElement(obj);
            Add(elem);
        }

        private void Add(IHistoryElement address)
        {
            if (!suppressAdding)
            {
                undoStack.Add(address);
                currentIndex = undoStack.IndexOf(address);
            }
        }

        public bool Back()
        {
            var nextIndex = currentIndex - 1;

            var nextElement = undoStack.ElementAtOrDefault(nextIndex);
            if (nextElement == default)
            {
                return false;
            }
            var number = undoStack.Count() - nextIndex - 1;
            undoStack.RemoveRange(nextIndex, number);

            Property = nextElement.Value;
            currentIndex = nextIndex;
            return true;
        }

        public bool Forward()
        {
            var nextElement = undoStack.ElementAtOrDefault(currentIndex + 1);
            if (nextElement == default)
            {
                return false;
            }

            Property = nextElement.Value;
            return true;
        }
    }
}
