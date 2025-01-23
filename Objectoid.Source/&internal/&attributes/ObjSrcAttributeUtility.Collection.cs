using System;
using System.Collections;
using System.Collections.Generic;

namespace Objectoid.Source
{
    internal static partial class ObjSrcAttributeUtility
    {
        private interface ICollection_<TKey, TAttribute, TEntry>
            where TAttribute : ObjSrcAttribute
            where TEntry : Entry_<TKey, TAttribute>
        {
			/// <summary>Attempts to create and add an entry using the specified attribute and type</summary>
			/// <param name="attribute">Attribute</param>
			/// <param name="type">Type</param>
			/// <returns>Whether or not successful</returns>
			/// <exception cref="ArgumentNullException"><paramref name="type"/> is null</exception>
			bool TryAdd(TAttribute attribute, Type type);
        }

        public abstract class Collection_<TKey, TAttribute, TEntry> : ICollection_<TKey, TAttribute, TEntry>, IEnumerable<TEntry>
            where TAttribute : ObjSrcAttribute
            where TEntry : Entry_<TKey, TAttribute>
        {
            #region ICollection_

            bool ICollection_<TKey, TAttribute, TEntry>.TryAdd(TAttribute attribute, Type type)
            {
                try
                {
                    if (!TryCreate_m(attribute, type, out var entry)) return false;
                    if (entry.Key is null) return false;
                    return _Entries.TryAdd(entry.Key, entry);
                }
                catch when (attribute is null) { throw new ArgumentNullException(nameof(attribute)); }
                catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
            }

            #endregion

            #region IEnumerable

            /// <summary>Gets an enumerator for the collection</summary>
            /// <returns>An enumerator for the collection</returns>
            public IEnumerator<TEntry> GetEnumerator() => _Entries.Values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            #endregion

            public Collection_()
            {
                _Entries = new Dictionary<TKey, TEntry>();
            }

            private readonly Dictionary<TKey, TEntry> _Entries;

			/// <summary>Attempts to create an instance of <typeparamref name="TEntry"/> using the specified attribute and type</summary>
			/// <param name="attribute">Attribute</param>
			/// <param name="type">Type</param>
			/// <param name="entry">Created instance of <typeparamref name="TEntry"/></param>
			/// <returns>Whether or not successful</returns>
			/// <exception cref="ArgumentNullException"><paramref name="type"/> is null</exception>
			private protected abstract bool TryCreate_m(TAttribute attribute, Type type, out TEntry entry);

            /// <summary>Numbers of entries in the collection</summary>
            public int Count => _Entries.Count;

            /// <summary>Attempts to get the entry with the specified key</summary>
            /// <param name="key">Key of the entry</param>
            /// <param name="entry">Retrieved entry</param>
            /// <returns>Whether or not successful</returns>
            /// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
            public bool TryGet(TKey key, out TEntry entry)
            {
                try { return _Entries.TryGetValue(key, out entry); }
                catch when (key is null) { throw new ArgumentNullException(nameof(key)); }
            }
        }
    }
}
