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
            List<byte> chars = new List<byte>();
            while (true)
            {
                byte c = objReader.ReadUInt8();
                if (c == 0x00) break;
                chars.Add(c);
            }
            Value_p = new ObjNTString(chars.ToArray());
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            foreach (byte c in Value_p)
                objWriter.WriteUInt8(c);
            objWriter.WriteUInt8(0);
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
        public new ObjNTString Value { get => Value_p; set => Value_p = value; }
    }
}
