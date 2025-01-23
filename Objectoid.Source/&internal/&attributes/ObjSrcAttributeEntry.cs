using System;
using System.Collections.Generic;
using System.Reflection;

namespace Objectoid.Source
{
    internal static class ObjSrcAttributeEntry
    {
        static ObjSrcAttributeEntry()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                Attribute attribute;
            }
        }
    }

    internal abstract class ObjSrcAttributeEntry<TKey, TAttribute, TEntry>
        where TAttribute : Attribute
        where TEntry : ObjSrcAttributeEntry<TKey, TAttribute, TEntry>
    {
        #region static

        private static readonly Dictionary<TKey, TEntry> _Registry = new Dictionary<TKey, TEntry>();

        /// <summary>Gets an enumerable collection of registered instances of <typeparamref name="TEntry"/></summary>
        /// <returns>An enumerable collection of registered instances of <typeparamref name="TEntry"/></returns>
        public static IEnumerable<TEntry> EnumerateEntries()
        {
            foreach (var entry in _Registry.Values)
                yield return entry;
        }

        /// <summary>Number of registered instances of <typeparamref name="TEntry"/></summary>
        public static int EntryCount => _Registry.Count;

        /// <summary>Attempts to get the registered instance of <typeparamref name="TEntry"/> with the specified key</summary>
        /// <param name="key">Key</param>
        /// <param name="entry">Retrieved registered instance of <typeparamref name="TEntry"/></param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
        public static bool TryGetEntry(TKey key, out TEntry entry)
        {
            try { return _Registry.TryGetValue(key, out entry); }
            catch when (key is null) { throw  new ArgumentNullException(nameof(key)); }
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjSrcAttributeEntry{TKey, TAttribute, TEntry}"/></summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type that contains the attribute</param>
        /// <param name="attribute">Attribute</param>
        private protected ObjSrcAttributeEntry(TKey key, Type type, TAttribute attribute)
        {
            Key = key;
            Type = type;
            Attribute = attribute;

            if (key is null) Registered = false;
            else Registered = _Registry.TryAdd(key, (TEntry)this);
        }

        /// <summary>Whether or not the instance is registered</summary>
        public bool Registered { get; }

        /// <summary>Key</summary>
        public TKey Key { get; }

        /// <summary>Type that contains the attribute</summary>
        public Type Type { get; }

        /// <summary>Attribute</summary>
        public TAttribute Attribute { get; }
    }
}
