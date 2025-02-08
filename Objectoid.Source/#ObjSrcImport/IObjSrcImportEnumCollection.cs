using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of supported enumerations</summary>
    public interface IObjSrcImportEnumCollection : IEnumerable<ObjSrcImportEnum>
    {
        #region IEnumerable

        IEnumerator<ObjSrcImportEnum> IEnumerable<ObjSrcImportEnum>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        new IEnumerator<ObjSrcImportEnum> GetEnumerator();

        /// <summary>Number of supported enumerations within the collection</summary>
        int Count { get; }

        /// <summary>Attempts to get the supported enumeration with the specified name</summary>
        /// <param name="name">Name of the supported enumeration</param>
        /// <param name="enumeration">Retrieved supported enumeration</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        bool TryGet(string name, out ObjSrcImportEnum enumeration);
    }
}
