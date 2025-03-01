using System;

namespace BitfinexUI.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class ObservableStack<T> : INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable<T>
    {
        private readonly LinkedList<T> _items = new LinkedList<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count => _items.Count;

        public void Push(T item)
        {
            _items.AddFirst(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, 0);
            OnPropertyChanged(nameof(Count));
        }

        public T Pop()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Стэк пустой.");

            var item = _items.First.Value;
            _items.RemoveFirst();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, 0);
            OnPropertyChanged(nameof(Count));
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Стэк пустой.");

            return _items.First.Value;
        }

        public void Clear()
        {
            _items.Clear();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            OnPropertyChanged(nameof(Count));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, T item = default, int index = -1)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
