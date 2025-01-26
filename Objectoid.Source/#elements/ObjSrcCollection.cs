using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid collection element source</summary>
    public abstract class ObjSrcCollection : ObjSrcElement, IEnumerable<ObjSrcElement>
    {
        #region helper

        private protected static ArgumentException H_ThrowArgumentNotCollectible_m(string paramName) =>
            throw new ArgumentException("The specified element cannot be part of a collection.", nameof(paramName));

        private protected static ArgumentException H_ThrowArgumentPartOfCollection_m(string paramName) =>
            throw new ArgumentException("The specified element is already part of a collection.", nameof(paramName));

        /// <summary>Loads an element from the specified objectoid-source reader</summary>
        /// <param name="reader">Objectoid-source reader</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="reader"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        /// <exception cref="EndOfStreamException">Stream ends before all data is found</exception>
        private protected ObjSrcElement LoadElement_m(ObjSrcReader reader)
        {
            try
            {
                //Identify element type
                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.Keyword)
                    throw ObjSrcReaderException.ThrowUnexpectedToken(reader.Token);
                if (!ObjSrcAttributeUtility.Readables.TryGet(reader.Token.Text, out var readable))
                    throw ObjSrcReaderException.ThrowUnexpectedKeyword(reader.Token);

                //Create and load
                var instance = readable.Create();
                instance.Load_m(reader);

                //Return
                return instance;
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <summary>Creates an element source and decodes data from the specified element to the created element source</summary>
        /// <param name="decodable">Decodable entry</param>
        /// <param name="element">Element for decoding</param>
        /// <returns>Created element source</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decodable"/> is null
        /// <br/>or<br/>
        /// <paramref name="element"/> is null
        /// </exception>
        /// 
        private protected ObjSrcElement CreateAndDecode_m(ObjSrcDecodable decodable, ObjElement element)
        {
            try
            {
                var iDecodable = decodable.Create();
                iDecodable.Decode(element);
                return (ObjSrcElement)iDecodable;
            }
            catch when (decodable is null) { throw new ArgumentNullException(nameof(decodable)); }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        public IEnumerator<ObjSrcElement> GetEnumerator() => GetEnumerator_m();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        private protected abstract IEnumerator<ObjSrcElement> GetEnumerator_m();

        /// <summary>Number of elements within the collection</summary>
        public abstract int Count { get; }
    }
}
