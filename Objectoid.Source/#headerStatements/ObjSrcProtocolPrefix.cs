using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents a protocol prefix statement</summary>
    [ObjSrcHead(ObjSrcKeyword._ProtocolPrefix)]
    public class ObjSrcProtocolPrefix : ObjSrcHeaderStatement
    {
        /// <inheritdoc cref="IObjSrcLoadSave.Load"/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.String)
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                var value = reader.Token.Text;

                reader.Read();
                reader.Token.ThrowIfNotEOL_m();

                Value = value;
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc cref="IObjSrcLoadSave.Save"/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._ProtocolPrefix} ");
                IObjSrcLoadSave.WriteStringToken(writer, Value);
                writer.WriteLine();
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <summary>Value of the protocol prefix</summary>
        public string Value { get; set; }
    }
}
