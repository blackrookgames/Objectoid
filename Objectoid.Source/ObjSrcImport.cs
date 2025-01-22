using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid object source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Import)]
    public class ObjSrcImport : ObjSrcObject
    {
        /// <summary>Creates an instance of <see cref="ObjSrcImport"/></summary>
        public ObjSrcImport() { }

        /// <inheritdoc/>
        internal sealed override void Load_m(ObjSrcReader reader)
        {
            try
            {
                Clear();

                //Find protocol
                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.String)
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                Protocol = reader.Token.Text;

                //Ensure only whitespace follows protocol
                reader.Read();
                reader.Token.ThrowIfNotEOL_m();

                //Look for entries
                ReadEntries_m(reader, ObjSrcKeyword._EndImport);
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal sealed override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._Import} ");
                WriteStringToken(writer, Protocol);
                writer.WriteLine();
                WriteEntries_m(writer, ObjSrcKeyword._EndImport);
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <summary>Name of protocol to use for importing</summary>
        public string Protocol { get; set; }
    }
}
