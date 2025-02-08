using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of import protocols</summary>
    public class ObjSrcImportProtocolCollection : NamedCollection<string, ObjSrcImportProtocol>, IObjSrcImportProtocolCollection
    {
        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection"/></summary>
        public ObjSrcImportProtocolCollection() : base() { }

        /// <summary>Creates an instance of <see cref="ObjSrcImportProtocolCollection"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public ObjSrcImportProtocolCollection(int capacity) : base(capacity) { }
    }
}
