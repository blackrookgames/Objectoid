using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Extension methods for <see cref="ObjDocument"/></summary>
    public static class ObjDocument_ext
    {
        #region DefaultImportOptions

        private class DefaultImportOptions_ : IObjSrcImportOptions
        {
            private readonly DefaultImportProtocolCollection_ _Protocols = new DefaultImportProtocolCollection_();

            bool IObjSrcImportOptions.ThrowIfUnknownProtocol => false;

            IObjSrcImportProtocolCollection IObjSrcImportOptions.Protocols => _Protocols;
        }

        private class DefaultImportProtocolCollection_ : IObjSrcImportProtocolCollection
        {
            int IObjSrcImportProtocolCollection.Count => 0;

            IEnumerator<ObjSrcImportProtocol> IObjSrcImportProtocolCollection.GetEnumerator()
            {
                yield break;
            }

            bool IObjSrcImportProtocolCollection.TryGet(string name, out ObjSrcImportProtocol protocol)
            {
                protocol = default;
                return false;
            }
        }

        #endregion

        #region helper

        /// <summary>Resets the document header</summary>
        /// <param name="document">Objectoid document</param>
        /// <exception cref="ArgumentNullException"><paramref name="document"/> is null</exception>
        private static void ResetHeader_m(ObjDocument document)
        {
            try
            {
                document.Identifier = null;
            }
            catch when (document is null) { throw new ArgumentNullException(nameof(document)); }
        }

        /// <summary>Decodes the document header</summary>
        /// <param name="document">Objectoid document</param>
        /// <param name="source">Objectoid source document</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null
        /// <br/>or<br/>
        /// <paramref name="source"/> is null
        /// </exception>
        /// 
        private static void DecodeHeader(this ObjDocument document, ObjSrcDocument source)
        {
            try
            {
                source.HeaderStatements.Clear();

                //Identifier
                var srcIdentifier = new ObjSrcIdentifier();
                srcIdentifier.Value = document.Identifier;
                source.HeaderStatements.Add(srcIdentifier);
            }
            catch when (document is null) { throw new ArgumentNullException(nameof(document)); }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
        }

        /// <summary>Encodes the document header</summary>
        /// <param name="document">Objectoid document</param>
        /// <param name="source">Objectoid source document</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null
        /// <br/>or<br/>
        /// <paramref name="source"/> is null
        /// </exception>
        /// 
        private static void EncodeHeader(this ObjDocument document, ObjSrcDocument source)
        {
            try
            {
                ResetHeader_m(document);

                foreach (var statement in source.HeaderStatements)
                {
                    //Identifier
                    if (statement is ObjSrcIdentifier)
                    {
                        var srcIdentifier = (ObjSrcIdentifier)statement;
                        document.Identifier = srcIdentifier.Value;
                    }
                }
            }
            catch when (document is null) { throw new ArgumentNullException(nameof(document)); }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
        }

        #endregion

        private readonly static DefaultImportOptions_ _DefaultImportOptions = new DefaultImportOptions_();

        /// <summary>Decodes the objectoid document to the specified objectoid source document</summary>
        /// <param name="document">Objectoid document</param>
        /// <param name="source">Objectoid source document</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null
        /// <br/>or<br/>
        /// <paramref name="source"/> is null
        /// </exception>
        /// 
        public static void Decode(this ObjDocument document, ObjSrcDocument source)
        {
            try
            {
                DecodeHeader(document, source);
                ((IObjSrcDecodable)source.Root).Decode(document.RootObject);
            }
            catch when (document is null) { throw new ArgumentNullException(nameof(document)); }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
        }

        /// <summary>Encodes the objectoid document from the specified objectoid source document</summary>
        /// <param name="document">Objectoid document</param>
        /// <param name="source">Objectoid source document</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null
        /// <br/>or<br/>
        /// <paramref name="source"/> is null
        /// </exception>
        /// 
        public static void Encode(this ObjDocument document, ObjSrcDocument source) =>
            Encode(document, source, null);

        /// <summary>Encodes the objectoid document from the specified objectoid source document</summary>
        /// <param name="document">Objectoid document</param>
        /// <param name="source">Objectoid source document</param>
        /// <param name="options">Import options</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is null
        /// <br/>or<br/>
        /// <paramref name="source"/> is null
        /// <br/>or<br/>
        /// <paramref name="options"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">An import source contains invalid data</exception>
        /// 
        public static void Encode(this ObjDocument document, ObjSrcDocument source, IObjSrcImportOptions options)
        {
            if (options is null) options = _DefaultImportOptions;
            try
            {
                EncodeHeader(document, source);
                source.Root.Encode_m(document.RootObject, options);
            }
            catch when (document is null) { throw new ArgumentNullException(nameof(document)); }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
        }
    }
}
