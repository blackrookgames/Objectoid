using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents an element with a null-terminated string value</summary>
    public class ObjNTStringElement : ObjComparable<ObjNTStringElement, ObjNTString>
    {
        #region ObjElement

        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = RWUtility.ReadNTString(objReader);
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            RWUtility.WriteNTString(objWriter, Value_p);
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjNTStringElement"/></summary>
        public ObjNTStringElement() : base(ObjType.NullTerminatedString) { }

        /// <summary>Creates an instance of <see cref="ObjNTStringElement"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjNTStringElement(ObjNTString initialValue) : this()
        {
            Value_p = initialValue;
        }

        /// <summary>A null-terminated string value</summary>
        public new ObjNTString Value
        {
            get => Value_p;
            set => Value_p = value;
        }
    }
}
