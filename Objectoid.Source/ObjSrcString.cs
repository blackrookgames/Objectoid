using System;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid string source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._String)]
    public class ObjSrcString : ObjSrcElement
    {
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

        /// <summary>Value</summary>
        public string Value { get; set; }
    }
}
