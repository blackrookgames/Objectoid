using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    internal interface IObjSrcLoadSave
    {
        #region helper

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
        void Load(ObjSrcReader reader);

        /// <summary>Saves to the specified objectoid-source writer</summary>
        /// <param name="writer">Objectoid-source writer</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="writer"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        void Save(ObjSrcWriter writer);
    }
}
