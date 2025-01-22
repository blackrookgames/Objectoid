using System;
using Rookie;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid boolean value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Bool)]
    public class ObjSrcBool : ObjSrcValuable<bool>
    {
        /// <inheritdoc/>
        private protected override ObjSrcReaderTokenType TokenType_p => ObjSrcReaderTokenType.Constant;

        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Bool;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out bool result) => Parser.TryToBool(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "boolean";
    }

    /// <summary>Represents an objectoid 8-bit unsigned value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._UInt8)]
    public class ObjSrcUInt8 : ObjSrcValuable<byte>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt8;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out byte result) => Parser.TryToUInt8(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "8-bit unsigned";
    }

    /// <summary>Represents an objectoid 8-bit signed value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Int8)]
    public class ObjSrcInt8 : ObjSrcValuable<sbyte>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int8;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out sbyte result) => Parser.TryToInt8(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "8-bit signed";
    }

    /// <summary>Represents an objectoid 16-bit unsigned value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._UInt16)]
    public class ObjSrcUInt16 : ObjSrcValuable<ushort>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt16;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out ushort result) => Parser.TryToUInt16(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "16-bit unsigned";
    }

    /// <summary>Represents an objectoid 16-bit signed value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Int16)]
    public class ObjSrcInt16 : ObjSrcValuable<short>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int16;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out short result) => Parser.TryToInt16(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "16-bit signed";
    }

    /// <summary>Represents an objectoid 32-bit unsigned value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._UInt32)]
    public class ObjSrcUInt32 : ObjSrcValuable<uint>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt32;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out uint result) => Parser.TryToUInt32(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "32-bit unsigned";
    }

    /// <summary>Represents an objectoid 32-bit signed value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Int32)]
    public class ObjSrcInt32 : ObjSrcValuable<int>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int32;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out int result) => Parser.TryToInt32(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "32-bit signed";
    }

    /// <summary>Represents an objectoid 64-bit unsigned value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._UInt64)]
    public class ObjSrcUInt64 : ObjSrcValuable<ulong>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt64;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out ulong result) => Parser.TryToUInt64(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "64-bit unsigned";
    }

    /// <summary>Represents an objectoid 64-bit signed value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Int64)]
    public class ObjSrcInt64 : ObjSrcValuable<long>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int64;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out long result) => Parser.TryToInt64(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "64-bit signed";
    }

    /// <summary>Represents an objectoid single-precision floating-point value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Single)]
    public class ObjSrcSingle : ObjSrcValuable<float>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Single;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out float result) => Parser.TryToSingle(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "single-precision floating-point";
    }

    /// <summary>Represents an objectoid double-precision floating-point value source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Double)]
    public class ObjSrcDouble : ObjSrcValuable<double>
    {
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Double;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out double result) => Parser.TryToDouble(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "double-precision floating-point";
    }
}


