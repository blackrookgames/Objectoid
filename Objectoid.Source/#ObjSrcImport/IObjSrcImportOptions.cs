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
}
