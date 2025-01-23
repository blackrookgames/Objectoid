using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of import protocols</summary>
    public class ObjSrcImportProtocolCollection : IObjSrcImportProtocolCollection
    {
        #region IObjSrcImportProtocolCollection

        /// <inheritdoc/>
        public IEnumerator<ObjSrcImportProtocol> GetEnumerator() => _Protocols.Values.GetEnumerator();

        /// <inheritdoc/>
        public int Count => _Protocols.Count;

        /// <inheritdoc/>
        public bool TryGet(string name, out ObjSrcImportProtocol protocol)
        {
            try { return _Protocols.TryGetValue(name, out protocol); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection"/></summary>
        public ObjSrcImportProtocolCollection() : this(0) { }

        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public ObjSrcImportProtocolCollection(int capacity)
        {
            try { _Protocols = new Dictionary<string, ObjSrcImportProtocol>(capacity); }
            catch when (capacity < 0) { throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative."); }
        }

        private readonly Dictionary<string, ObjSrcImportProtocol> _Protocols;

        /// <summary>Adds the specified protocol to the collection</summary>
        /// <param name="protocol">Protocol to add</param>
        /// <exception cref="ArgumentNullException"><paramref name="protocol"/> is null</exception>
        /// <exception cref="ArgumentException">Collection already contains a protocol with the same name as <paramref name="protocol"/></exception>
        public void Add(ObjSrcImportProtocol protocol)
        {
            try { _Protocols.Add(protocol.Name, protocol); }
            catch when (protocol is null) { throw new ArgumentNullException(nameof(protocol)); }
            catch when (_Protocols.ContainsKey(protocol.Name)) { throw new ArgumentException("Collection already contains a protocol with the same name.", nameof(protocol)); }
        }

        /// <summary>Attempts to remove the specified protocol from the collection</summary>
        /// <param name="protocol">Protocol to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="protocol"/> is null</exception>
        public bool Remove(ObjSrcImportProtocol protocol)
        {
            try
            {
                if (!_Protocols.TryGetValue(protocol.Name, out var found)) return false;
                if (protocol != found) return false;
                _Protocols.Remove(protocol.Name);
                return true;
            }
            catch when (protocol is null) { throw new ArgumentNullException(nameof(protocol)); }
        }

        /// <summary>Attempts to remove the protocol with the specified name from the collection</summary>
        /// <param name="name">Name of the protocol to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool Remove(string name)
        {
            try { return _Protocols.Remove(name); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <summary>Removes all protocols from the collection</summary>
        public void Clear()
        {
            _Protocols.Clear();
        }
    }

    /// <summary>Represents a generic collection of import protocols</summary>
    /// <typeparam name="TOptions">Option type</typeparam>
    /// <typeparam name="TProtocol">Protocol base type</typeparam>
    public class ObjSrcImportProtocolCollection<TOptions, TProtocol> : IObjSrcImportProtocolCollection<TOptions, TProtocol>
        where TOptions : IObjSrcImportOptions<TOptions, TProtocol>
        where TProtocol : ObjSrcImportProtocol<TOptions, TProtocol>
    {
        #region IObjSrcImportProtocolCollection

        /// <inheritdoc/>
        public IEnumerator<TProtocol> GetEnumerator() => _Protocols.Values.GetEnumerator();

        /// <inheritdoc/>
        public int Count => _Protocols.Count;

        /// <inheritdoc/>
        public bool TryGet(string name, out TProtocol protocol)
        {
            try { return _Protocols.TryGetValue(name, out protocol); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection{TOptions, TProtocol}"/></summary>
        public ObjSrcImportProtocolCollection() : this(0) { }

        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection{TOptions, TProtocol}"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public ObjSrcImportProtocolCollection(int capacity)
        {
            try { _Protocols = new Dictionary<string, TProtocol>(capacity); }
            catch when (capacity < 0) { throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative."); }
        }

        private readonly Dictionary<string, TProtocol> _Protocols;

        /// <summary>Adds the specified protocol to the collection</summary>
        /// <param name="protocol">Protocol to add</param>
        /// <exception cref="ArgumentNullException"><paramref name="protocol"/> is null</exception>
        /// <exception cref="ArgumentException">Collection already contains a protocol with the same name as <paramref name="protocol"/></exception>
        public void Add(TProtocol protocol)
        {
            try { _Protocols.Add(protocol.Name, protocol); }
            catch when (protocol is null) { throw new ArgumentNullException(nameof(protocol)); }
            catch when (_Protocols.ContainsKey(protocol.Name)) { throw new ArgumentException("Collection already contains a protocol with the same name.", nameof(protocol)); }
        }

        /// <summary>Attempts to remove the specified protocol from the collection</summary>
        /// <param name="protocol">Protocol to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="protocol"/> is null</exception>
        public bool Remove(TProtocol protocol)
        {
            try
            {
                if (!_Protocols.TryGetValue(protocol.Name, out var found)) return false;
                if (protocol != found) return false;
                _Protocols.Remove(protocol.Name);
                return true;
            }
            catch when (protocol is null) { throw new ArgumentNullException(nameof(protocol)); }
        }

        /// <summary>Attempts to remove the protocol with the specified name from the collection</summary>
        /// <param name="name">Name of the protocol to remove</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool Remove(string name)
        {
            try { return _Protocols.Remove(name); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <summary>Removes all protocols from the collection</summary>
        public void Clear()
        {
            _Protocols.Clear();
        }
    }
}
