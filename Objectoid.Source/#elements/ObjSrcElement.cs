using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid element source</summary>
    public abstract class ObjSrcElement
    {
        #region helper

        /// <summary>Throws an <see cref="NotSupportedException"/> explaining the element type cannot be part of a collection</summary>
        /// <exception cref="NotSupportedException">Expected outcome</exception>
        private protected NotSupportedException ThrowNotCollectible_m() =>
            throw new NotSupportedException($"{GetType().Name} cannot be part of a collection.");

        /// <summary>Writes a string token to the specified objectoid-source writer</summary>
        /// <param name="writer">Objectoid-source writer</param>
        /// <param name="str">String to write</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null<br/>or<br/><paramref name="str"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="writer"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        private protected static void WriteStringToken(ObjSrcWriter writer, string str)
        {
            try
            {
                char getDigitChar(int value)
                {
                    if (value >= 0 && value <= 9)
                        return (char)('0' + value);
                    if (value >= 10 && value <= 15)
                        return (char)('A' + value - 10);
                    return '\0';
                }
                IEnumerable<char> writeString()
                {
                    yield return '"';
                    foreach (char c in str)
                    {
                        //Whitespace/non-standard
                        if (c < ' ' || c >= 0x7F)
                        {
                            yield return '\\';
                            yield return 'u';
                            yield return getDigitChar((c >> 12) & 15);
                            yield return getDigitChar((c >> 8) & 15);
                            yield return getDigitChar((c >> 4) & 15);
                            yield return getDigitChar(c & 15);
                        }
                        //Special
                        else if (c == '\'' || c == '"' || c == '\\')
                        {
                            yield return '\\';
                            yield return c;
                        }
                        //Normal
                        else yield return c;
                    }
                    yield return '"';
                }
                writer.Write(writeString());
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
            catch when (str is null) { throw new ArgumentNullException(nameof(str)); }
        }

        #endregion

        /// <summary>Loads from the specified objectoid-source reader</summary>
        /// <param name="reader">Objectoid-source reader</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="reader"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        /// <exception cref="EndOfStreamException">Stream ends before all data is found</exception>
        internal abstract void Load_m(ObjSrcReader reader);

        /// <summary>Saves to the specified objectoid-source writer</summary>
        /// <param name="writer">Objectoid-source writer</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="writer"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        internal abstract void Save_m(ObjSrcWriter writer);

        /// <summary>Creates an objectoid element</summary>
        /// <param name="options">Import options</param>
        /// <returns>Created objectoid element</returns>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null</exception>
        /// <exception cref="ObjSrcException">An import source contains invalid data</exception>
        internal abstract ObjElement CreateElement_m(IObjSrcImportOptions options);

        /// <summary>Whether or not the element can be a part of a collection</summary>
        public virtual bool IsCollectible => true;

        private ObjSrcCollection _Collection;
        /// <summary>Collection the element of a part of</summary>
        public ObjSrcCollection Collection => _Collection;

        /// <summary>"Adds" the element to the specified collection</summary>
        /// <param name="collection">Collection</param>
        /// <exception cref="NotSupportedException">Element cannot be a part of a collection</exception>
        /// <exception cref="InvalidOperationException">Element is already part of a collection</exception>
        internal virtual void AddToCollection_m(ObjSrcCollection collection)
        {
            if (_Collection is null) _Collection = collection;
            else throw new InvalidOperationException("Element is currently part of a collection.");
        }

        /// <summary>"Removes" the element from the collection it is "a part of"</summary>
        internal void RemoveFromCollection_m() => _Collection = null;
    }
}
