using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreUtility.Extensions {
    public static class CollectionExtensions {

        #region Collection
        
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> target) {
            foreach (var t in target) 
                collection.Add(t);

            return collection;
        }


        #endregion

        #region Array

        public static T ElementOrDefault<T>(this T[] array, int index) {
            return index >= array.Length || index < 0 ? default : array[index];
        }
        
        public static void ResetElement<T>(this T[] array, int index) {
            if (index >= array.Length || index < 0) return;
            
            array[index] = default;
        }
        
        public static T[] Add<T>(this T[] array, T value) {
            var newArray = new T[array.Length + 1];

            for (var i = 0; i < array.Length; i++) 
                newArray[i] = array[i];
            
            newArray[^1] = value;
            return newArray;
        }
        
        public static bool IsOutOfRange<T>(this T[] array, int index) {
            return index >= array.Length || index < 0;
        }
        
        public static void ResetElement<T>(this ICollection<T>[] array, int index) {
            if (index >= array.Length || index < 0) return;
            
            array[index] = default;
        }
        
        public static bool IsOutOfRange<T>(this ICollection<T> array, int index) {
            return index >= array.Count || index < 0;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action) {
            foreach (var element in array) 
                action?.Invoke(element);
        }
        
        public static void ForEach<T>(this IEnumerable<T> array, Action<T, int> action) {
            var index = 0;
            foreach (var element in array) {
                action?.Invoke(element, index);
                index++;
            }
        }

        // Copies only possible ids main array = 5 side = 3 will be copy only 3 values
        public static T[] CopySubset<T>(this T[] array, T[] target) {
            for (var i = 0; i < target.Length; i++) {
                if (array.IsOutOfRange(i)) 
                    break;

                array[i] = target[i];
            }

            return array;
        }

        #endregion

        #region Enumerator

        /// <summary>
        /// Converts the Task into an IEnumerator for Unity coroutine usage.
        /// </summary>
        /// <param name="task">The Task to convert.</param>
        /// <returns>An IEnumerator representation of the Task.</returns>
        public static IEnumerator AsCoroutine(this Task task) {
            while (!task.IsCompleted) yield return null;
            // When used on a faulted Task, GetResult() will propagate the original exception. 
            // see: https://devblogs.microsoft.com/pfxteam/task-exception-handling-in-net-4-5/
            task.GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Converts an IEnumerator<T> to an IEnumerable<T>.
        /// </summary>
        /// <param name="e">An instance of IEnumerator<T>.</param>
        /// <returns>An IEnumerable<T> with the same elements as the input instance.</returns>    
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> e) {
            while (e.MoveNext()) {
                yield return e.Current;
            }
        }

        #endregion

        #region List

                /// <summary>
        /// Determines whether a collection is null or has no elements
        /// without having to enumerate the entire collection to get a count.
        ///
        /// Uses LINQ's Any() method to determine if the collection is empty,
        /// so there is some GC overhead.
        /// </summary>
        /// <param name="list">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list) {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Creates a new list that is a copy of the original list.
        /// </summary>
        /// <param name="list">The original list to be copied.</param>
        /// <returns>A new list that is a copy of the original list.</returns>
        public static List<T> Clone<T>(this IList<T> list) {
            List<T> newList = new List<T>();
            foreach (T item in list) {
                newList.Add(item);
            }

            return newList;
        }
        
        public static bool IsOutOfRange<T>(this List<T> array, int index) {
            return index >= array.Count || index < 0;
        }

        /// <summary>
        /// Swaps two elements in the list at the specified indices.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.</param>
        /// <param name="indexB">The index of the second element.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }
        
        public static T ElementOrDefault<T>(this IList<T> list, int index) {
            return index >= list.Count ? default : list[index];
        }

        public static int GetIndex<T>(this IList<T> collection, T compareInstance) where T: class {
            for (var i = 0; i < collection.Count; i++) {
                if (ReferenceEquals(collection[i],compareInstance))
                    return i;
            }

            return -1;
        }

        #endregion
    }
}