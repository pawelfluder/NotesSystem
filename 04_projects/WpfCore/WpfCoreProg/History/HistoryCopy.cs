using System.Collections.Generic;
using System.Linq;

namespace WpfNotesSystemProg3.History
{
    internal class HistoryCopy
    {
        private List<IHistoryElement> undoStack;
        private List<IHistoryElement> redoStack;
        private int currentIndex;
        private readonly object property;
        public bool suppressAdding;

        public HistoryCopy(object property)
        {
            this.property = property;
            suppressAdding = false;
            undoStack = new List<IHistoryElement>();
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

        public bool Back(ref IHistoryElement element)
        {
            var nextIndex = currentIndex - 1;

            var nextElement = undoStack.ElementAtOrDefault(nextIndex);
            if (nextElement == default)
            {
                return false;
            }
            var number = undoStack.Count() - nextIndex - 1;
            undoStack.RemoveRange(nextIndex, number);

            element = nextElement;
            currentIndex = nextIndex;
            return true;
        }

        public bool Forward(ref IHistoryElement element)
        {
            var nextElement = undoStack.ElementAtOrDefault(currentIndex + 1);
            if (nextElement == default)
            {
                return false;
            }

            element = nextElement;
            return true;
        }
    }
}
