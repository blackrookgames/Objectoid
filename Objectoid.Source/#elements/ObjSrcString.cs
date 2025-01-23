using System;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid string source</summary>
    [ObjSrcReadable(ObjSrcKeyword._String)]
    [ObjSrcDecodable(typeof(ObjStringElement))]
    public class ObjSrcString : ObjSrcElement, IObjSrcDecodable<ObjStringElement>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjStringElement>.Decode(ObjStringElement element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcString"/></summary>
        public ObjSrcString() { }

        /// <inheritdoc/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                var stringBuilder = new StringBuilder();

                while (true)
                {
                    reader.Read();
                    if (reader.Token.Type == ObjSrcReaderTokenType.None) break;
                    if (reader.Token.Type == ObjSrcReaderTokenType.String)
                    {
                        stringBuilder.Append(reader.Token.Text);
                        continue;
                    }
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                }

                Value = stringBuilder.ToString();
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._String} ");
                WriteStringToken(writer, Value);
                writer.WriteLine();
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjStringElement(Value);
        }

        /// <summary>Value</summary>
        public string Value { get; set; }
    }
}
