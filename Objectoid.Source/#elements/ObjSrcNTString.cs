using System;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid null-terminated string source</summary>
    [ObjSrcReadable(ObjSrcKeyword._NTString)]
    [ObjSrcDecodable(typeof(ObjNTStringElement))]
    public class ObjSrcNTString : ObjSrcElement, IObjSrcDecodable<ObjNTStringElement>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjNTStringElement>.Decode(ObjNTStringElement element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcNTString"/></summary>
        public ObjSrcNTString() { }

        /// <inheritdoc/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.String)
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                if (!ObjNTString.TryParse(reader.Token.Text, out var value))
                    ObjSrcException.ThrowSyntaxError_m($"\"{reader.Token.Text}\" is not a valid null-terminated string value.", reader.Token);
                Value = value;
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._NTString} ");
                WriteStringToken(writer, Value.ToString());
                writer.WriteLine();
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjNTStringElement(Value);
        }

        /// <summary>Value</summary>
        public ObjNTString Value { get; set; }
    }
}
