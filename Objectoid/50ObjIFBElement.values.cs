using System;
using System.IO;

namespace Objectoid
{
    /// <summary>Represents an element with an unsigned 8-bit integer value</summary>
    public class ObjUInt8Element : ObjIFBElement<byte>
    {
        /// <summary>Creates an instance of <see cref="ObjUInt8Element"/></summary>
        public ObjUInt8Element() : base(ObjType.UInt8) { }
        
        /// <summary>Creates an instance of <see cref="ObjUInt8Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjUInt8Element(byte initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>An unsigned 8-bit integer value</summary>
        public new byte Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteUInt8(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadUInt8();
        }
    }

    /// <summary>Represents an element with a signed 8-bit integer value</summary>
    public class ObjInt8Element : ObjIFBElement<sbyte>
    {
        /// <summary>Creates an instance of <see cref="ObjInt8Element"/></summary>
        public ObjInt8Element() : base(ObjType.Int8) { }
        
        /// <summary>Creates an instance of <see cref="ObjInt8Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjInt8Element(sbyte initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A signed 8-bit integer value</summary>
        public new sbyte Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteInt8(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadInt8();
        }
    }

    /// <summary>Represents an element with an unsigned 16-bit integer value</summary>
    public class ObjUInt16Element : ObjIFBElement<ushort>
    {
        /// <summary>Creates an instance of <see cref="ObjUInt16Element"/></summary>
        public ObjUInt16Element() : base(ObjType.UInt16) { }
        
        /// <summary>Creates an instance of <see cref="ObjUInt16Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjUInt16Element(ushort initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>An unsigned 16-bit integer value</summary>
        public new ushort Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteUInt16(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadUInt16();
        }
    }

    /// <summary>Represents an element with a signed 16-bit integer value</summary>
    public class ObjInt16Element : ObjIFBElement<short>
    {
        /// <summary>Creates an instance of <see cref="ObjInt16Element"/></summary>
        public ObjInt16Element() : base(ObjType.Int16) { }
        
        /// <summary>Creates an instance of <see cref="ObjInt16Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjInt16Element(short initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A signed 16-bit integer value</summary>
        public new short Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteInt16(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadInt16();
        }
    }

    /// <summary>Represents an element with an unsigned 32-bit integer value</summary>
    public class ObjUInt32Element : ObjIFBElement<uint>
    {
        /// <summary>Creates an instance of <see cref="ObjUInt32Element"/></summary>
        public ObjUInt32Element() : base(ObjType.UInt32) { }
        
        /// <summary>Creates an instance of <see cref="ObjUInt32Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjUInt32Element(uint initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>An unsigned 32-bit integer value</summary>
        public new uint Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteUInt32(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadUInt32();
        }
    }

    /// <summary>Represents an element with a signed 32-bit integer value</summary>
    public class ObjInt32Element : ObjIFBElement<int>
    {
        /// <summary>Creates an instance of <see cref="ObjInt32Element"/></summary>
        public ObjInt32Element() : base(ObjType.Int32) { }
        
        /// <summary>Creates an instance of <see cref="ObjInt32Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjInt32Element(int initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A signed 32-bit integer value</summary>
        public new int Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteInt32(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadInt32();
        }
    }

    /// <summary>Represents an element with an unsigned 64-bit integer value</summary>
    public class ObjUInt64Element : ObjIFBElement<ulong>
    {
        /// <summary>Creates an instance of <see cref="ObjUInt64Element"/></summary>
        public ObjUInt64Element() : base(ObjType.UInt64) { }
        
        /// <summary>Creates an instance of <see cref="ObjUInt64Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjUInt64Element(ulong initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>An unsigned 64-bit integer value</summary>
        public new ulong Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteUInt64(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadUInt64();
        }
    }

    /// <summary>Represents an element with a signed 64-bit integer value</summary>
    public class ObjInt64Element : ObjIFBElement<long>
    {
        /// <summary>Creates an instance of <see cref="ObjInt64Element"/></summary>
        public ObjInt64Element() : base(ObjType.Int64) { }
        
        /// <summary>Creates an instance of <see cref="ObjInt64Element"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjInt64Element(long initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A signed 64-bit integer value</summary>
        public new long Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteInt64(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadInt64();
        }
    }

    /// <summary>Represents an element with a single-precision floating-point value</summary>
    public class ObjSingleElement : ObjIFBElement<float>
    {
        /// <summary>Creates an instance of <see cref="ObjSingleElement"/></summary>
        public ObjSingleElement() : base(ObjType.Single) { }
        
        /// <summary>Creates an instance of <see cref="ObjSingleElement"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjSingleElement(float initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A single-precision floating-point value</summary>
        public new float Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteSingle(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadSingle();
        }
    }

    /// <summary>Represents an element with a double-precision floating-point value</summary>
    public class ObjDoubleElement : ObjIFBElement<double>
    {
        /// <summary>Creates an instance of <see cref="ObjDoubleElement"/></summary>
        public ObjDoubleElement() : base(ObjType.Double) { }
        
        /// <summary>Creates an instance of <see cref="ObjDoubleElement"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjDoubleElement(double initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A double-precision floating-point value</summary>
        public new double Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteDouble(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadDouble();
        }
    }

    /// <summary>Represents an element with a boolean value</summary>
    public class ObjBoolElement : ObjIFBElement<bool>
    {
        /// <summary>Creates an instance of <see cref="ObjBoolElement"/></summary>
        public ObjBoolElement() : base(ObjType.Bool) { }
        
        /// <summary>Creates an instance of <see cref="ObjBoolElement"/> with a specified initial value</summary>
        /// <param name="initialValue">Initial value</param>
        public ObjBoolElement(bool initialValue) : this()
        {
            Value_p = initialValue;
        }
        
        /// <summary>A boolean value</summary>
        public new bool Value { get => Value_p; set => Value_p = value; }
        
        /// <inheritdoc/>
        internal override void Write_m(ObjWriter objWriter)
        {
            objWriter.WriteBool(Value_p);
        }
        
        /// <inheritdoc/>
        internal override void Read_m(ObjReader objReader)
        {
            Value_p = objReader.ReadBool();
        }
    }
}

