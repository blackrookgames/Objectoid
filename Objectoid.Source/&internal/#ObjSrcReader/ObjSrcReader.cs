using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcReader : IDisposable
    {
        #region helper

        protected virtual void H_Dispose_m(bool disposing)
        {
            if (!__Disposed) return;

            if (disposing && !_LeaveOpen)
            {
                _BaseStream.Close();
            }
            __Disposed = true;
        }

        private void H_ThrowIfDisposed_m()
        {
            if (__Disposed) throw new ObjectDisposedException("The reader has already been disposed.");
        }

        #endregion

        #region IDisposable

        /// <summary>Disposes the reader</summary>
        public void Dispose() => H_Dispose_m(true);

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcReader"/></summary>
        /// <param name="stream">Base stream</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="leaveOpen">Whether or not to leave the base stream open after disposing</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> is null
        /// <br/>or<br/>
        /// <paramref name="encoding"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="stream"/> does not support reading
        /// </exception>
        /// 
        public ObjSrcReader(Stream stream, Encoding encoding, bool leaveOpen)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (encoding is null) throw new ArgumentNullException(nameof(encoding));

            if (!stream.CanRead) throw new ArgumentException("The specified stream does not support reading.", nameof(stream));

            _BaseStream = stream;
            _Encoding = encoding;
            _Buffer = new byte[(encoding is UnicodeEncoding) ? 2 : 1];
            _LeaveOpen = leaveOpen;
        }

        #region fields

        private bool __Disposed;

        private readonly Stream _BaseStream;
        private readonly Encoding _Encoding;
        private readonly byte[] _Buffer;
        private readonly bool _LeaveOpen;
        private readonly StringBuilder _StringBuilder = new StringBuilder();

        private int __Row = 0;
        private int __Column = -1;
        private char __Char = '\0';

        private ObjSrcReaderState __State = ObjSrcReaderState.Start;
        private bool __End = false; //Whether or not the reader has read the last character in the stream

        private IEnumerator<ObjSrcReaderToken> __Tokens;
        private ObjSrcReaderToken __Token;

        private bool __DontAdvance = false;

        #endregion

        #region properties

        /// <summary>Base stream</summary>
        public Stream BaseStream => _BaseStream;

        /// <summary>State of the reader</summary>
        public ObjSrcReaderState State => __State;

        /// <summary>Current token</summary>
        public ObjSrcReaderToken Token => __Token;

        #endregion

        #region methods

        /// <summary>Tells the reader not to advance to the next token the next time <see cref="Read"/> is called.</summary>
        public void DontAdvance()
        {
            __DontAdvance = true;
        }

        /// <summary>Reads the next token in the stream</summary>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        /// <exception cref="ObjectDisposedException">Reader has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        public void Read()
        {
            H_ThrowIfDisposed_m();
            if (__State == ObjSrcReaderState.End) throw new EndOfStreamException("The end of the stream has already been reached.");

            if (__DontAdvance)
            {
                __DontAdvance = false;
                return;
            }

            if (__State == ObjSrcReaderState.Start)
                __State = ObjSrcReaderState.Reading;

            if (__Tokens is null)
                __Tokens = ReadBlock_m();
            if (__Tokens.MoveNext())
            {
                __Token = __Tokens.Current;
            }
            else
            {
                __Tokens = null;
                __Token = new ObjSrcReaderToken(ObjSrcReaderTokenType.None, __Row, __Column, null);
                if (__End) __State = ObjSrcReaderState.End;
            }
        }

        /// <summary>Reads the next character in the stream</summary>
        /// <exception cref="IOException">An I/O error occurs</exception>
        private void ReadNext_m()
        {
            try
            {
                char readChar()
                {
                    while (true)
                    {
                        if (_BaseStream.Read(_Buffer, 0, _Buffer.Length) < _Buffer.Length)
                            return '\0';
                        var @char = _Encoding.GetChars(_Buffer)[0];
                        if (@char != '\0') return @char;
                    }
                }

                var prevChar = __Char;
                //If previous char was \r or \n, start a new line
                if (prevChar == '\r' || prevChar == '\n') { __Row++; __Column = 0; }
                else __Column++;
                //Read character
                __Char = readChar();
                if (__Char == '\r') { if (prevChar == '\n') __Char = readChar(); }
                else if (__Char == '\n') { if (prevChar == '\r') __Char = readChar(); }
            }
            catch (IOException e) { throw e; }
        }

        /// <summary>Reads the next parse block</summary>
        /// <returns>Tokens within the parse block</returns>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        private IEnumerator<ObjSrcReaderToken> ReadBlock_m()
        {
            bool readNext(bool considerSplit)
            {
                ReadNext_m();
                if (__Char == '\0') { __End = true; return false; }
                if (__Char == '\r' || __Char == '\n') return false;
                if (considerSplit && __Char == ObjSrcSymbol._Split) return false;
                return true;
            } //Returns false if the end of the block has been reached; returns true otherwise

            var startRow = 0;
            var startColumn = 0;
            ObjSrcReaderToken createToken(ObjSrcReaderTokenType type)
            {
                var token = new ObjSrcReaderToken(type, startRow, startColumn, _StringBuilder.ToString());
                _StringBuilder.Clear();
                return token;
            }

            if (!readNext(true)) yield break;

            _StringBuilder.Clear();

        none:
            if (__Char > 0x20)
            {
                startRow = __Row;
                startColumn = __Column;
                //Comment
                if (__Char == ObjSrcSymbol._Comment) goto comment;
                //Stretch
                if (__Char == ObjSrcSymbol._Stretch) goto stretch;
                //Keyword
                if (__Char == ObjSrcSymbol._Keyword) goto keyword;
                //Numeric
                if (__Char == '-' || (__Char >= '0' && __Char <= '9')) goto numeric;
                //String
                if (__Char == '"') goto @string;
                //Constant
                if ((__Char >= 'A' && __Char <= 'Z') || (__Char >= 'a' && __Char <= 'z')) goto constant;
                //Unexpected
                new ObjSrcReaderException($"The character '{__Char}' was unexpected.", __Row, __Column);
            }
            if (!readNext(true)) yield break;
            goto none;

        comment:
            if (!readNext(false)) yield break;
            goto comment;

        stretch:
            if (!readNext(false)) goto none;
            goto stretch;

        keyword:
            _StringBuilder.Append(__Char);
            if (!readNext(true))
            {
                yield return createToken(ObjSrcReaderTokenType.Keyword);
                yield break;
            }
            if (__Char <= 0x20 || __Char == ObjSrcSymbol._Comment || __Char == ObjSrcSymbol._Stretch)
            {
                yield return createToken(ObjSrcReaderTokenType.Keyword);
                goto none;
            }
            goto keyword;

        numeric:
            _StringBuilder.Append(__Char);
            if (!readNext(true))
            {
                yield return createToken(ObjSrcReaderTokenType.Numeric);
                yield break;
            }
            if (__Char <= 0x20 || __Char == ObjSrcSymbol._Comment || __Char == ObjSrcSymbol._Stretch)
            {
                yield return createToken(ObjSrcReaderTokenType.Numeric);
                goto none;
            }
            goto numeric;

        @string:
            if (!readNext(false)) goto unexpectedEndOfLine;
            if (__Char == '"')
            {
                yield return createToken(ObjSrcReaderTokenType.String);
                if (!readNext(true)) yield break; //Make sure to consider splits
                goto none;
            }
            if (__Char == '\\')
            {
                if (!readNext(false)) goto unexpectedEndOfLine;
                //Escape sequences
                if (__Char == '\'' || __Char == '"' || __Char == '\\') { }
                else if (__Char == 'n') __Char = '\n';
                else if (__Char == 'r') __Char = '\r';
                else if (__Char == '0') __Char = (char)0x00;
                else if (__Char == 'a') __Char = (char)0x07;
                else if (__Char == 'b') __Char = (char)0x08;
                else if (__Char == 'e') __Char = (char)0x1B;
                else if (__Char == 'f') __Char = (char)0x0C;
                else if (__Char == 't') __Char = (char)0x09;
                else if (__Char == 'v') __Char = (char)0x0B;
                else if (__Char == 'u')
                {
                    var unicode = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (!readNext(false)) goto unexpectedEndOfLine;
                        unicode <<= 4;
                        if (__Char >= '0' && __Char <= '9') { unicode |= __Char - '0'; continue; }
                        if (__Char >= 'A' && __Char <= 'F') { unicode |= 0x10 + __Char - 'A'; continue; }
                        if (__Char >= 'a' && __Char <= 'f') { unicode |= 0x10 + __Char - 'a'; continue; }
                        throw new ObjSrcReaderException($"The character '{__Char}' was unexpected within the unicode sequence.", __Row, __Column);
                    }
                    __Char = (char)unicode;
                }
                else new ObjSrcReaderException($"The escape sequence '\\{__Char}' is unrecognized.", __Row, __Column);
            }
            _StringBuilder.Append(__Char);
            goto @string;

        constant:
            _StringBuilder.Append(__Char);
            if (!readNext(true))
            {
                yield return createToken(ObjSrcReaderTokenType.Constant);
                yield break;
            }
            if (__Char <= 0x20 || __Char == ObjSrcSymbol._Comment || __Char == ObjSrcSymbol._Stretch)
            {
                yield return createToken(ObjSrcReaderTokenType.Constant);
                goto none;
            }
            goto constant;

            unexpectedEndOfLine:
            new ObjSrcReaderException("The line ends unexpectedly.", __Row, __Column);
        }


        #endregion
    }
}
