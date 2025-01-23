using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a property within an objectoid object source</summary>
    public struct ObjSrcObjectProperty
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="name"/> is not null<br/>
        /// <paramref name="value"/> is not null <br/>
        /// <br/>
        /// Called by <see cref="ObjSrcObject"/>
        /// </remarks>
        internal ObjSrcObjectProperty(ObjNTString name, ObjSrcElement value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>Name of the property</summary>
        public ObjNTString Name { get; }

        /// <summary>Element value of the property</summary>
        public ObjSrcElement Value { get; }
    }
}
