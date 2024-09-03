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
            //Read length and character size
            int sizeLen = objReader.ReadInt32();
            bool is8bit = (sizeLen & int.MinValue) != 0;
            int length = sizeLen & int.MaxValue;
            //Read chracters
            char[] chars = new char[length];
            if (is8bit)
            {
                for (int i = 0; i < length; i++)
                    chars[i] = (char)objReader.ReadUInt8();
            }
            else
            {
                for (int i = 0; i < length; i++)
                    chars[i] = (char)objReader.ReadUInt16();
            }
            Value_p = new string(chars);
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            if (Value_p is null)
            {
                objWriter.WriteInt32(0);
            }
            else
            {
                //Determine character size
                bool is8bit = true;
                foreach (char c in Value_p)
                {
                    if ((c & 0xFF00) == 0) continue;
                    is8bit = false;
                    break;
                }
                //Write length and character size
                int sizelen = Value_p.Length;
                if (is8bit) sizelen |= int.MinValue;
                objWriter.WriteInt32(sizelen);
                //Write characters
                if (is8bit)
                {
                    foreach (char c in Value_p)
                        objWriter.WriteUInt8((byte)c);
                }
                else
                {
                    foreach (char c in Value_p)
                        objWriter.WriteUInt16(c);
                }
            }
            
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
