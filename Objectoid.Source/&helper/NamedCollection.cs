using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Base class for representing a collection of named items</summary>
    /// <typeparam name="TName">Data type of item names</typeparam>
    /// <typeparam name="TItem">Data type of items</typeparam>
    public abstract class NamedCollection<TName, TItem> : IEnumerable<TItem>
        where TItem : INamedCollectionItem<TName>
    {
        #region helper

        /// <summary>Throws a <see cref="NotSupportedException"/> explaining that the collection is readonly</summary>
        /// <exception cref="NotSupportedException">Expected outcome</exception>
        protected static Exception ThrowReadonly_m()
        {
            throw new NotSupportedException("The collection is readonly.");
        }

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        public IEnumerator<TItem> GetEnumerator() => _Items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Creates an instance of <see cref="NamedCollection{TName, TItem}"/></summary>
        public NamedCollection() : this(0) { }

        /// <summary>Creates an instance of <see cref="NamedCollection{TName, TItem}"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public NamedCollection(int capacity)
        {
            try { _Items = new Dictionary<TName, TItem>(capacity); }
            catch when (capacity < 0) { throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative."); }
        }

        #region fields

        private readonly Dictionary<TName, TItem> _Items;

        #endregion

        #region properties

        /// <summary>Number of items in the collection</summary>
        public int Count => _Items.Count;

        /// <summary>Whether or not the collection is readonly</summary>
        public virtual bool IsReadonly => false;

        #endregion

        #region method

        /// <summary>Attempts to get the item with the specified name</summary>
        /// <param name="name">Name of the item</param>
        /// <param name="item">Retrieved item</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool TryGet(TName name, out TItem item)
        {
            try { return _Items.TryGetValue(name, out item); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <summary>Adds the specified item to the collection</summary>
        /// <param name="item">Item to add</param>
        /// <exception cref="NotSupportedException">Collection is readonly</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentException">Collection already contains a item with the same name as <paramref name="item"/></exception>
        public virtual void Add(TItem item) => Add_m(item);

        /// <summary>Attempts to remove the specified item from the collection</summary>
        /// <param name="item">Item to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="NotSupportedException">Collection is readonly</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null</exception>
        public virtual bool Remove(TItem item) => Remove_m(item);

        /// <summary>Attempts to remove the item with the specified name from the collection</summary>
        /// <param name="name">Name of the item to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="NotSupportedException">Collection is readonly</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public virtual bool Remove(TName name) => Remove_m(name);

        /// <summary>Removes all items from the collection</summary>
        /// <exception cref="NotSupportedException">Collection is readonly</exception>
        public virtual void Clear() => Clear_m();

        /// <inheritdoc cref="Add(TItem)"/>
        protected void Add_m(TItem item)
        {
            try { _Items.Add(item.Name, item); }
            catch when (item is null) { throw new ArgumentNullException(nameof(item)); }
            catch when (_Items.ContainsKey(item.Name)) { throw new ArgumentException("Collection already contains an item with the same name.", nameof(item)); }
        }

        /// <inheritdoc cref="Remove(TItem)"/>
        protected bool Remove_m(TItem item)
        {
            try
            {
                if (!_Items.TryGetValue(item.Name, out var found)) return false;
                if ((object)item != (object)found) return false;
                _Items.Remove(item.Name);
                return true;
            }
            catch when (item is null) { throw new ArgumentNullException(nameof(item)); }
        }

        /// <inheritdoc cref="Remove(TName)"/>
        protected bool Remove_m(TName name)
        {
            try { return _Items.Remove(name); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <inheritdoc cref="Clear"/>
        protected void Clear_m()
        {
            _Items.Clear();
        }

        #endregion
    }
}
