using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents encoding result of a property within an instance of <see cref="ObjSrcImport"/></summary>
    public struct ObjSrcImportEncodedProperty
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="name"/> is not null<br/>
        /// <paramref name="sourceValue"/> is not null<br/>
        /// <paramref name="encodedValue"/> is not null<br/>
        /// <br/>
        /// Called by <see cref="ObjSrcImportEncodedPropertyCollection"/>
        /// </remarks>
        internal ObjSrcImportEncodedProperty(ObjNTString name, ObjSrcElement sourceValue, ObjElement encodedValue)
        {
            Name = name;
            SourceValue = sourceValue;
            EncodedValue = encodedValue;
        }

        /// <summary>Name of the property</summary>
        public ObjNTString Name { get; }

        /// <summary>Source value of the property</summary>
        public ObjSrcElement SourceValue { get; }

        /// <summary>Encoded value of the property</summary>
        public ObjElement EncodedValue { get; }
    }
}
