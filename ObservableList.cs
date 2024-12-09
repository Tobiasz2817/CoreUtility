using System.Collections.Generic;
using System.Collections;
using System;

namespace CoreUtility {
     [Serializable]
     public class ObservableList<T> : IList<T> {
         readonly IList<T> _list;
         
         public event Action<IList<T>> Changed;
         public event Action<int, T, T> ValueChange;
     
         public ObservableList(IList<T> initialList = null) {
             _list = initialList ?? new List<T>();
         }
     
         public T this[int index] {
             get => _list[index];
             set {
                 var prevValue = _list[index];
                 _list[index] = value;
                 Invoke(index, prevValue, value);
                 Invoke();
             }
         }
     
         public void Invoke() => Changed?.Invoke(_list);
         public void Invoke(int id, T oldValue, T newValue) => ValueChange?.Invoke(id, oldValue, newValue);
     
         public int Count => _list.Count;
     
         public bool IsReadOnly => _list.IsReadOnly;
     
         public void Add(T item) {
             _list.Add(item);
             Invoke();
         }
     
         public void Clear() {
             _list.Clear();
             Invoke();
         }
     
         public bool Contains(T item) => _list.Contains(item);
     
         public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
     
         public bool Remove(T item) {
             var result = _list.Remove(item);
             if (result) {
                 Invoke();
             }
     
             return result;
         }
     
         public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
         IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
     
         public int IndexOf(T item) => _list.IndexOf(item);
     
         public void Insert(int index, T item) {
             _list.Insert(index, item);
             Invoke();
         }
     
         public void RemoveAt(int index) {
             _list.RemoveAt(index);
             Invoke();
         }
     }
}