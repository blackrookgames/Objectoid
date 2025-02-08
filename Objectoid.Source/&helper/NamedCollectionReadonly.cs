using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Base class for representing a readonly collection of named items</summary>
    /// <typeparam name="TName">Data type of item names</typeparam>
    /// <typeparam name="TItem">Data type of items</typeparam>
    public abstract class NamedCollectionReadonly<TName, TItem> : NamedCollection<TName, TItem>
        where TItem : INamedCollectionItem<TName>
    {
        /// <summary>Creates an instance of <see cref="NamedCollectionReadonly{TName, TItem}"/></summary>
        public NamedCollectionReadonly() : base() { }

        /// <summary>Creates an instance of <see cref="NamedCollectionReadonly{TName, TItem}"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public NamedCollectionReadonly(int capacity) : base(capacity) { }

        #region properties

        /// <summary>Whether or not the collection is readonly</summary>
        public sealed override bool IsReadonly => true;

        #endregion

        #region method

        /// <summary>Do not use this</summary>
        /// <exception cref="NotSupportedException">Calling <see cref="Add(TItem)"/> will always throw an <see cref="NotSupportedException"/></exception>
        /// <remarks>This collection is readonly, and therefore items cannot be added or removed</remarks>
        public sealed override void Add(TItem item) => throw ThrowReadonly_m();

        /// <summary>Do not use this</summary>
        /// <exception cref="NotSupportedException">Calling <see cref="Remove(TItem)"/> will always throw an <see cref="NotSupportedException"/></exception>
        /// <remarks>This collection is readonly, and therefore items cannot be added or removed</remarks>
        public sealed override bool Remove(TItem item) => throw ThrowReadonly_m();

        /// <summary>Do not use this</summary>
        /// <exception cref="NotSupportedException">Calling <see cref="Remove(TName)"/> will always throw an <see cref="NotSupportedException"/></exception>
        /// <remarks>This collection is readonly, and therefore items cannot be added or removed</remarks>
        public sealed override bool Remove(TName name) => throw ThrowReadonly_m();

        /// <summary>Do not use this</summary>
        /// <exception cref="NotSupportedException">Calling <see cref="Clear"/> will always throw an <see cref="NotSupportedException"/></exception>
        /// <remarks>This collection is readonly, and therefore items cannot be added or removed</remarks>
        public sealed override void Clear() => throw ThrowReadonly_m();

        #endregion
    }
}
