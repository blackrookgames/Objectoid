using System;
using System.Linq;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid object source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Import)]
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
                    ObjSrcReaderException.ThrowUnexpectedToken(reader.Token);
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
                IObjSrcLoadSave.WriteStringToken(writer, Protocol);
                writer.WriteLine();
                WriteEntries_m(writer, ObjSrcKeyword._EndImport);
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <inheritdoc/>
        internal sealed override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            bool tryGetProtocol(out ObjSrcImportProtocol protocol)
            {
                if (options.Protocols.TryGet(Protocol, out protocol))
                    return true;
                var prefixes = (
                    from statement in Document.HeaderStatements
                    where statement is ObjSrcProtocolPrefix
                    let protocolPrefix = (ObjSrcProtocolPrefix)statement
                    select protocolPrefix.Value);
                foreach (var prefix in prefixes)
                {
                    if (options.Protocols.TryGet($"{prefix}{Protocol}", out protocol))
                        return true;
                }
                return false;
            }

            try
            {
                if (tryGetProtocol(out var protocol))
                {
                    try
                    {
                        var encodedProperties = new ObjSrcImportEncodedPropertyCollection(this, options);
                        return protocol.Import(encodedProperties, options);
                    }
                    catch (ObjSrcException e)
                    {
                        throw new ObjSrcSrcElementException(this, e.Message);
                    }
                }
                else
                {
                    if (options.ThrowIfUnknownProtocol)
                        throw new ObjSrcSrcElementException(this, $"The protocol \"{Protocol}\" is not supported.");
                    return base.CreateElement_m(options);
                }
            }
            catch when (options is null) { throw new ArgumentNullException(nameof(options)); }
        }

        /// <summary>Name of import protocol</summary>
        public string Protocol { get; set; }
    }
}
