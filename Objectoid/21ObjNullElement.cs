using System;
using System.IO;

namespace Objectoid
{
    /// <summary>Represents an element with a null value</summary>
    public class ObjNullElement : ObjElement
    {
        #region ObjElement

        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader) { }

        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter) { }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjNullElement"/></summary>
        public ObjNullElement() : base(ObjType.Null, true) { }
    }
}
