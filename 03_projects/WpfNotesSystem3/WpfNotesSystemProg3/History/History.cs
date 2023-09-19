using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfNotesSystemProg3.History
{
    internal class History<T>
    {
        private List<T> undoStack;
        private List<T> redoStack;
        public bool suppressAdding;
        T currentElem;

        public History()
        {
            suppressAdding = false;
            undoStack = new List<T>();
            redoStack = new List<T>();
        }

        public void SuppressAdding(Action action)
        {
            SuppressAdding();
            action.Invoke();
            UnSuppressAdding();
        }

        private void UndoStackAdd(T elem)
        {
            if (elem.Equals(default(T)))
            {
                throw new Exception();
            }

            undoStack.Add(elem);
        }

        private void RedoStackAdd(T elem)
        {
            if (elem.Equals(default(T)))
            {
                throw new Exception();
            }

            redoStack.Add(elem);
        }

        private void UnSuppressAdding()
        {
            suppressAdding = false;
        }

        private void SuppressAdding()
        {
            suppressAdding = true;
        }

        public void Add(T elem)
        {
            if (!suppressAdding)
            {
                if (EqualityComparer<T>.Default.Equals(currentElem, default(T)))
                {
                    currentElem = elem;
                    return;
                }

                UndoStackAdd(currentElem);
                redoStack.Clear();
                currentElem = elem;
                //nextUndoIndex++;
            }
        }

        public T Back(out bool success)
        {
            // return element
            var nextElement = undoStack.LastOrDefault();
            if (nextElement.Equals(default(T)))
            {
                success = false;
                return nextElement;
            }

            // undo stack
            undoStack.RemoveAt(undoStack.Count() - 1);

            // redo stack
            RedoStackAdd(currentElem);

            // current elem
            this.currentElem = nextElement;

            // correct return
            success = true;
            return nextElement;
        }

        public T Forward(out bool success)
        {
            // return element
            var nextElement = redoStack.LastOrDefault();
            if (nextElement.Equals(default(T)))
            {
                success = false;
                return nextElement;
            }

            // undo stack
            UndoStackAdd(currentElem);

            // redo stack
            redoStack.RemoveAt(redoStack.Count() - 1);

            // current elem
            this.currentElem = nextElement;

            success = true;
            return nextElement;
        }
    }
}
