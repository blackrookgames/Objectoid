using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents an element with a string value</summary>
    public class ObjStringElement : ObjComparable<ObjStringElement, string>
    {
        #region ObjElement

        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = RWUtility.ReadString(objReader);
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            RWUtility.WriteString(objWriter, Value_p);
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjStringElement"/></summary>
        public ObjStringElement() : base(ObjType.String) { }

        /// <summary>Creates an instance of <see cref="ObjStringElement"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjStringElement(string initialValue) : this()
        {
            Value_p = initialValue;
        }

        /// <summary>A string value</summary>
        public new string Value
        {
            get => Value_p;
            set => Value_p = value;
        }
    }
}
