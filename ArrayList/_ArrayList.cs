﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayList
{
    public class _ArrayList
    {
        private Object[] _items;
        private int _size;
        private int _version;
        private static readonly Object[] emptyArray = new Object[0];

        private const int _defaultCapacity = 4;

        public _ArrayList()
        {
            _items = emptyArray;
        }
        public _ArrayList(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException();
            if (capacity == 0)
                _items = emptyArray;
            else
                _items = new Object[capacity];
        }
        public virtual int Count
        {
            get
            {
                return _size;
            }
        }
        public virtual Object this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException();
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException();
                _items[index] = value;
                _version++;
            }
        }
        public virtual int Add(Object value)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size] = value;
            _version++;
            return _size++;
        }
        public virtual bool Contains(Object item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                for (int i = 0; i < _size; i++)
                    if ((_items[i] != null) && (_items[i].Equals(item)))
                        return true;
                return false;
            }
        }
        public virtual int IndexOf(Object value)
        {
            return Array.IndexOf((Array)_items, value, 0, _size);
        }
        public virtual int IndexOf(Object value, int startIndex)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException();
           return Array.IndexOf((Array)_items, value, startIndex, _size - startIndex);
        }
        public virtual int IndexOf(Object value, int startIndex, int count)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException();
            if (count < 0 || startIndex > _size - count) throw new ArgumentOutOfRangeException();
            return Array.IndexOf((Array)_items, value, startIndex, count);
        }
        public virtual void Insert(int index, Object value)
        {
            if (index < 0 || index > _size) throw new ArgumentOutOfRangeException();
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = value;
            _size++;
            _version++;
        }
        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException();
            Contract.Ensures(Count >= 0);
            Contract.EndContractBlock();

            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = null;
            _version++;
        }
        public virtual void RemoveRange(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentException();
            Contract.Ensures(Count >= 0);
            Contract.EndContractBlock();

            if (count > 0)
            {
                int i = _size;
                _size -= count;
                if (index < _size)
                {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                while (i > _size) _items[--i] = null;
                _version++;
            }
        }
        public virtual void Remove(Object obj)
        {
            Contract.Ensures(Count >= 0);

            int index = IndexOf(obj);
            if (index >= 0)
                RemoveAt(index);
        }
        public virtual void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }
        public virtual void Reverse(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentException();
            Contract.EndContractBlock();
            Array.Reverse(_items, index, count);
            _version++;
        }
        public virtual int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        Object[] newItems = new Object[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = new Object[_defaultCapacity];
                    }
                }
            }
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > 0X7FEFFFFF) newCapacity = 0X7FEFFFFF;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
        public virtual IEnumerator GetEnumerator()
        {
            return new _ArrayListEnumeratorSimple(this);
        }
        public sealed class _ArrayListEnumeratorSimple : IEnumerator
        {
            private _ArrayList list;
            private int index;
            private int version;
            private Object currentElement;
            [NonSerialized]
            private bool isArrayList;
            static Object dummyObject = new Object();

            internal _ArrayListEnumeratorSimple(_ArrayList list)
            {
                this.list = list;
                this.index = -1;
                version = list._version;
                isArrayList = (list.GetType() == typeof(_ArrayList));
                currentElement = dummyObject;
            }

            public bool MoveNext()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException();
                }

                if (isArrayList)
                {  
                    if (index < list._size - 1)
                    {
                        currentElement = list._items[++index];
                        return true;
                    }
                    else
                    {
                        currentElement = dummyObject;
                        index = list._size;
                        return false;
                    }
                }
                else
                {
                    if (index < list.Count - 1)
                    {
                        currentElement = list[++index];
                        return true;
                    }
                    else
                    {
                        index = list.Count;
                        currentElement = dummyObject;
                        return false;
                    }
                }
            }

            public Object Current
            {
                get
                {
                    object temp = currentElement;
                    if (dummyObject == temp)
                    { 
                        if (index == -1)
                        {
                            throw new InvalidOperationException();
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }

                    return temp;
                }
            }

            public void Reset()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException();
                }

                currentElement = dummyObject;
                index = -1;
            }
        }
    }
   
}
