using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of import protocols</summary>
    public interface IObjSrcImportProtocolCollection : IEnumerable<ObjSrcImportProtocol>
    {
        #region IEnumerable

        IEnumerator<ObjSrcImportProtocol> IEnumerable<ObjSrcImportProtocol>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        new IEnumerator<ObjSrcImportProtocol> GetEnumerator();

        /// <summary>Number of protocols within the collection</summary>
        int Count { get; }

        /// <summary>Attempts to get the protocol with the specified name</summary>
        /// <param name="name">Name of the protocol</param>
        /// <param name="protocol">Retrieved protocol</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        bool TryGet(string name, out ObjSrcImportProtocol protocol);
    }

    /// <summary>Represents a generic collection of import protocols</summary>
    /// <typeparam name="TOptions">Option type</typeparam>
    /// <typeparam name="TProtocol">Protocol base type</typeparam>
    public interface IObjSrcImportProtocolCollection<TOptions, TProtocol> : IObjSrcImportProtocolCollection
        where TOptions: IObjSrcImportOptions<TOptions, TProtocol>
        where TProtocol: ObjSrcImportProtocol<TOptions, TProtocol>
    {
        #region IObjSrcImportProtocolCollection

        IEnumerator<ObjSrcImportProtocol> IObjSrcImportProtocolCollection.GetEnumerator() => GetEnumerator();

        bool IObjSrcImportProtocolCollection.TryGet(string name, out ObjSrcImportProtocol protocol)
        {
            if (TryGet(name, out var rawProtocol))
            {
                protocol = rawProtocol;
                return true;
            }
            else
            {
                protocol = null;
                return false;
            }
        }

        #endregion

        /// <inheritdoc cref="IObjSrcImportProtocolCollection.GetEnumerator"/>
        new IEnumerator<TProtocol> GetEnumerator();

        /// <inheritdoc cref="IObjSrcImportProtocolCollection.TryGet"/>
        bool TryGet(string name, out TProtocol protocol);
    }
}
