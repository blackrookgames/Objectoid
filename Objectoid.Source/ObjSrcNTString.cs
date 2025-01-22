using System;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid null-terminated string source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._NTString)]
    public class ObjSrcNTString : ObjSrcElement
    {
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


        /// <summary>Value</summary>
        public ObjNTString Value { get; set; }
    }
}
