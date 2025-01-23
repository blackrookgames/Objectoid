using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Interface for representing the import options for encoding an objectoid-document</summary>
    public interface IObjSrcImportOptions
    {
        /// <summary>Whether or not the throw a <see cref="ObjSrcException"/> if unknown protocol is detected</summary>
        bool ThrowIfUnknownProtocol { get; }

        /// <summary>Import protocols</summary>
        IObjSrcImportProtocolCollection Protocols { get; }
    }

    /// <summary>Generic interface for representing the import options for encoding an objectoid-document</summary>
    /// <typeparam name="TOptions">Option type</typeparam>
    /// <typeparam name="TProtocol">Protocol base type</typeparam>
    public interface IObjSrcImportOptions<TOptions, TProtocol> : IObjSrcImportOptions
        where TOptions : IObjSrcImportOptions<TOptions, TProtocol>
        where TProtocol : ObjSrcImportProtocol<TOptions, TProtocol>
    {
        #region IObjSrcImportOptions

        IObjSrcImportProtocolCollection IObjSrcImportOptions.Protocols => Protocols;

        #endregion

        /// <inheritdoc cref="IObjSrcImportOptions.Protocols"/>
        new IObjSrcImportProtocolCollection<TOptions, TProtocol> Protocols { get; }
    }
}
