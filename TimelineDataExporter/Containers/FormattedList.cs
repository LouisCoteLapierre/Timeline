using System.Collections;
using System.Collections.Generic;

namespace TimelineDataExporter.Containers
{
    public class FormattedList<T> : IList<T>
    {
        public static FormattedList<T> CreateFormattedList(List<T> otherList)
        {
            FormattedList<T> newFormattedList = new FormattedList<T>();
            foreach (T content in otherList)
            {
                newFormattedList.Add(content);
            }
            return newFormattedList;
        }

        public override string ToString()
        {
            string formattedContent = "";
            for (int i = 0; i < InternalList.Count; ++i)
            {
                formattedContent += InternalList[i].ToString();
                if (i != InternalList.Count - 1)
                {
                    formattedContent += " , ";
                }
            }

            return formattedContent;
        }

        public int IndexOf(T item)
        {
            return InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            InternalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InternalList.RemoveAt(index);
        }

        public void Add(T item)
        {
            InternalList.Add(item);
        }

        public void Clear()
        {
            InternalList.Clear();
        }

        public bool Contains(T item)
        {
            return InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InternalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return InternalList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        public int Count => InternalList.Count;

        public bool IsReadOnly => ((ICollection<T>)InternalList).IsReadOnly;

        public T this[int index] { get => InternalList[index]; set => InternalList[index] = value; }

        private List<T> InternalList = new List<T>();
    }
}
