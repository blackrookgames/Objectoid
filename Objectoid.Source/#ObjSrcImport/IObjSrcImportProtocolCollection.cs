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
}
