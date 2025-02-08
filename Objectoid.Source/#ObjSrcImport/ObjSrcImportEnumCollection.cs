using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of import protocols</summary>
    public class ObjSrcImportEnumCollection : NamedCollection<string, ObjSrcImportEnum>, IObjSrcImportEnumCollection
    {
        /// <summary>Creates an instance of <see cref="ObjSrcImportEnumCollection"/></summary>
        public ObjSrcImportEnumCollection() : base() { }

        /// <summary>Creates an instance of <see cref="ObjSrcImportEnumCollection"/></summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is negative</exception>
        public ObjSrcImportEnumCollection(int capacity) : base(capacity) { }
    }
}
