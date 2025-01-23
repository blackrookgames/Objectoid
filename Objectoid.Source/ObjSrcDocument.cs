using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid source document</summary>
    public class ObjSrcDocument
    {
        /// <summary>Creates an instance of <see cref="ObjSrcDocument"/></summary>
        public ObjSrcDocument()
        {
            _HeaderStatements = new ObjSrcHeaderStatementList(this);
            _Root = new ObjSrcRoot(this);
        }

        /// <summary>Loads data from the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support reading</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        public void Load(Stream stream) => Load(stream, Encoding.UTF8);

        /// <summary>Loads data from the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <param name="encoding">Character encoding</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null<br/>or<br/><paramref name="encoding"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support reading</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        public void Load(Stream stream, Encoding encoding)
        {

            ObjSrcReader reader = null;
            try
            {
                reader = new ObjSrcReader(stream, encoding, true);

                _HeaderStatements.Clear();
            header:
                reader.Read();
                if (reader.Token.Type == ObjSrcReaderTokenType.None) goto header;
                if (reader.Token.Type == ObjSrcReaderTokenType.Keyword)
                {
                    if (reader.Token.Text == ObjSrcKeyword._Object) goto root;
                    if (ObjSrcAttributeUtility.Heads.TryGet(reader.Token.Text, out var head))
                    {
                        var statement = head.Create();
                        statement.Load_m(reader);
                        _HeaderStatements.Add(statement);
                        goto header;
                    }
                    ObjSrcException.ThrowUnexpectedKeyword_m(reader.Token);
                }
                ObjSrcException.ThrowUnexpectedKeyword_m(reader.Token);

            root:
                _Root.Load_m(reader);
                //Ensure only whitespace follows root
                while (reader.State != ObjSrcReaderState.End)
                {
                    reader.Read();
                    reader.Token.ThrowIfNotEOL_m();
                }
            }
            catch when (stream is null) { throw new ArgumentNullException(nameof(stream)); }
            catch when (encoding is null) { throw new ArgumentNullException(nameof(encoding)); }
            catch when (!stream.CanRead) { throw new ArgumentException("The specified stream does not support reading.", nameof(stream)); }
            catch (ObjectDisposedException) { throw new ObjectDisposedException("The specified stream has already been disposed."); }
            catch (EndOfStreamException) { throw new ObjSrcException("Stream ends before all data is found."); }
            finally { reader?.Dispose(); }
        }

        /// <summary>Saves data to the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support writing</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void Save(Stream stream) => Save(stream, Encoding.UTF8);

        /// <summary>Saves data to the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <param name="encoding">Character encoding</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null<br/>or<br/><paramref name="encoding"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support writing</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        public void Save(Stream stream, Encoding encoding)
        {
            ObjSrcWriter writer = null;
            try
            {
                writer = new ObjSrcWriter(stream, encoding, true);

                //Header
                foreach (var statement in _HeaderStatements)
                    statement.Save_m(writer);
                
                //Root
                _Root.Save_m(writer);
            }
            catch when (stream is null) { throw new ArgumentNullException(nameof(stream)); }
            catch when (encoding is null) { throw new ArgumentNullException(nameof(encoding)); }
            catch when (!stream.CanWrite) { throw new ArgumentException("The specified stream does not support writing.", nameof(stream)); }
            catch (ObjectDisposedException) { throw new ObjectDisposedException("The specified stream has already been disposed."); }
            finally { writer?.Dispose(); }
        }

        private readonly ObjSrcRoot _Root;
        /// <summary>Root object</summary>
        public ObjSrcRoot Root => _Root;

        private readonly ObjSrcHeaderStatementList _HeaderStatements;
        /// <summary>Header statements</summary>
        public ObjSrcHeaderStatementList HeaderStatements => _HeaderStatements;
    }
}
