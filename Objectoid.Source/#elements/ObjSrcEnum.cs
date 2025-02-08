using System;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid enumeration source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Enum)]
    public class ObjSrcEnum : ObjSrcElement
    {
        /// <summary>Creates an instance of <see cref="ObjSrcEnum"/></summary>
        public ObjSrcEnum() { }

        /// <inheritdoc/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.String)
                    ObjSrcReaderException.ThrowUnexpectedToken(reader.Token);
                Type = reader.Token.Text;

                reader.Read();
                if (reader.Token.Type != ObjSrcReaderTokenType.String)
                    ObjSrcReaderException.ThrowUnexpectedToken(reader.Token);
                Value = reader.Token.Text;

                reader.Read();
                reader.Token.ThrowIfNotEOL_m();
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._Enum} ");
                IObjSrcLoadSave.WriteStringToken(writer, Type);

                writer.Write(' ');
                IObjSrcLoadSave.WriteStringToken(writer, Value);

                writer.WriteLine();
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            bool tryGetEnum(out ObjSrcImportEnum @enum)
            {
                foreach (var enumName in EnumeratePotentialProtocols_m(Type))
                {
                    if (options.Enumerations.TryGet(enumName, out @enum))
                        return true;
                }
                @enum = default;
                return false;
            }

            try
            {
                if (tryGetEnum(out var @enum))
                {
                    if (!Enum.TryParse(@enum.EnumType, Value, false, out var value))
                        throw new ObjSrcSrcElementException(this, $"\"{Value}\" is not a valid {Type} value.");

                    var srcElement = @enum.Create_m();
                    srcElement.Value = value;
                    return ((ObjSrcElement)srcElement).CreateElement_m(options);
                }
                else
                {
                    throw new ObjSrcSrcElementException(this, $"The enumeration \"{Type}\" is not supported.");
                }
            }
            catch when (options is null) { throw new ArgumentNullException(nameof(options)); }
        }

        /// <summary>Name of the enum type</summary>
        public string Type { get; set; }

        /// <summary>String value</summary>
        public string Value { get; set; }
    }
}
