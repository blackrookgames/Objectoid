using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcWriter : IDisposable
    {
        #region helper

        protected virtual void H_Dispose_m(bool disposing)
        {
            if (__Disposed) return;

            if (disposing && !_LeaveOpen)
            {
                _BaseStream.Close();
            }
            __Disposed = true;
        }

        private void H_ThrowIfDisposed_m()
        {
            if (__Disposed) throw new ObjectDisposedException("The writer has already been disposed.");
        }

        #endregion

        #region IDisposable

        /// <summary>Disposes the writer</summary>
        public void Dispose() => H_Dispose_m(true);

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcWriter"/></summary>
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
        /// <paramref name="stream"/> does not support writing
        /// </exception>
        /// 
        public ObjSrcWriter(Stream stream, Encoding encoding, bool leaveOpen)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (encoding is null) throw new ArgumentNullException(nameof(encoding));

            if (!stream.CanWrite) throw new ArgumentException("The specified stream does not support writing.", nameof(stream));

            _BaseStream = stream;
            _Encoding = encoding;
            _LeaveOpen = leaveOpen;

            char @char = '\0';
            unsafe { _CharPtr = &@char; }

            _ByteBuffer = new byte[(encoding is UnicodeEncoding) ? 2 : 1];
            unsafe { fixed (byte* byteBufferPtr = &_ByteBuffer[0]) _ByteBufferPtr = byteBufferPtr; }

        }

        #region fields

        private bool __Disposed;

        private readonly Stream _BaseStream;
        private readonly Encoding _Encoding;
        private readonly bool _LeaveOpen;

        private readonly byte[] _ByteBuffer;
        private readonly unsafe char* _CharPtr;
        private readonly unsafe byte* _ByteBufferPtr;

        private int __Indent = 0;

        private bool __StartOfLine = true;

        #endregion

        #region properties

        /// <summary>Base stream</summary>
        public Stream BaseStream => _BaseStream;

        /// <summary>Indent value</summary>
        public int Indent => __Indent;

        #endregion

        #region methods

        #region helper

        /// <summary>Indents if the writer is at the start of a line</summary>
        private void IndentIfStart_m()
        {
            try
            {
                if (!__StartOfLine) return;
                for (int i = __Indent * 4; i > 0; i--) Write_m(' ');
                __StartOfLine = false;
            }
            catch (IOException e) { throw e; }
        }

        /// <summary>Writes a character to the stream</summary>
        /// <exception cref="IOException">An I/O error occurs</exception>
        private void Write_m(char c)
        {
            try
            {
                unsafe
                {
                    *_CharPtr = c;
                    _Encoding.GetBytes(_CharPtr, 1, _ByteBufferPtr, _ByteBuffer.Length);
                    _BaseStream.Write(_ByteBuffer);
                }
            }
            catch (IOException e) { throw e; }
        }

        /// <summary>Writes a string to the stream</summary>
        /// <exception cref="IOException">An I/O error occurs</exception>
        private void Write_m(string s)
        {
            if (s is null) return;
            foreach (var c in s) Write_m(c);
        }

        #endregion

        /// <summary>Increases the indent</summary>
        public void IncIndent()
        {
            __Indent++;
        }

        /// <summary>Decreases the indent</summary>
        public void DecIndent()
        {
            if (__Indent > 0) __Indent--;
        }

        /// <summary>Writes a character to the stream</summary>
        /// <param name="c">Character to write</param>
        /// <exception cref="ObjectDisposedException">Writer has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void Write(char c)
        {
            H_ThrowIfDisposed_m();
            IndentIfStart_m();
            Write_m(c);
        }

        /// <summary>Writes characters to the stream</summary>
        /// <param name="chars">Characters to write</param>
        /// <exception cref="ObjectDisposedException">Writer has already been disposed</exception>
        /// <exception cref="ArgumentNullException"><paramref name="chars"/> is null</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void Write(IEnumerable<char> chars)
        {
            H_ThrowIfDisposed_m();
            if (chars is null) throw new ArgumentNullException(nameof(chars));

            IndentIfStart_m();
            foreach (var c in chars) Write_m(c);
        }

        /// <summary>Writes text to the stream</summary>
        /// <param name="text">Text to write</param>
        /// <exception cref="ObjectDisposedException">Writer has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void Write(string text)
        {
            H_ThrowIfDisposed_m();

            IndentIfStart_m();
            Write_m(text);
        }

        /// <summary>Writes a line of text to the stream</summary>
        /// <param name="text">Text to write</param>
        /// <exception cref="ObjectDisposedException">Writer has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void WriteLine(string text)
        {
            H_ThrowIfDisposed_m();

            IndentIfStart_m();
            Write_m(text);
            Write_m('\r');
            Write_m('\n');

            __StartOfLine = true;
        }

        /// <summary>Writes a line to the stream</summary>
        /// <exception cref="ObjectDisposedException">Writer has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void WriteLine() => WriteLine(null);

        #endregion
    }
}
