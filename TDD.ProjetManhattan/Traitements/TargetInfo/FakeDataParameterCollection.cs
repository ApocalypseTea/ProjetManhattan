using System.Collections;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class FakeDataParameterCollection : IDataParameterCollection
    {
        public object this[string parameterName] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object? this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count { get; set; }

        public bool IsSynchronized => true;

        public object SyncRoot => throw new NotImplementedException();

        public int Add(object? value)
        {
            this.Count++;
            return Count;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string parameterName)
        {
            return true;        }

        public bool Contains(object? value)
        {
           return true;
        }

        public void CopyTo(Array array, int index)
        {
            
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object? value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object? value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object? value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}