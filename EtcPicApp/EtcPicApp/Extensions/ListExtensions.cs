using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EtcPicApp.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            var collection = new ObservableCollection<T>();
            foreach (var item in list)
            {
                collection.Add(item);
            }

            return collection;
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(
        this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket.Select(x => x);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
            {
                Array.Resize(ref bucket, count);
                yield return bucket.Select(x => x);
            }
        }
    }
}
