using System;
using Rookie;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid boolean value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Bool)]
    [ObjSrcDecodable(typeof(ObjBoolElement))]
    public class ObjSrcBool : ObjSrcValuable<bool>, IObjSrcDecodable<ObjBoolElement>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjBoolElement>.Decode(ObjBoolElement element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcBool"/></summary>
        public ObjSrcBool() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjBoolElement(Value);
        }
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
    [ObjSrcReadable(ObjSrcKeyword._UInt8)]
    [ObjSrcDecodable(typeof(ObjUInt8Element))]
    public class ObjSrcUInt8 : ObjSrcValuable<byte>, IObjSrcDecodable<ObjUInt8Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjUInt8Element>.Decode(ObjUInt8Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcUInt8"/></summary>
        public ObjSrcUInt8() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjUInt8Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt8;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out byte result) => Parser.TryToUInt8(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "8-bit unsigned";
    }

    /// <summary>Represents an objectoid 8-bit signed value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Int8)]
    [ObjSrcDecodable(typeof(ObjInt8Element))]
    public class ObjSrcInt8 : ObjSrcValuable<sbyte>, IObjSrcDecodable<ObjInt8Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjInt8Element>.Decode(ObjInt8Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcInt8"/></summary>
        public ObjSrcInt8() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjInt8Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int8;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out sbyte result) => Parser.TryToInt8(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "8-bit signed";
    }

    /// <summary>Represents an objectoid 16-bit unsigned value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._UInt16)]
    [ObjSrcDecodable(typeof(ObjUInt16Element))]
    public class ObjSrcUInt16 : ObjSrcValuable<ushort>, IObjSrcDecodable<ObjUInt16Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjUInt16Element>.Decode(ObjUInt16Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcUInt16"/></summary>
        public ObjSrcUInt16() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjUInt16Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt16;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out ushort result) => Parser.TryToUInt16(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "16-bit unsigned";
    }

    /// <summary>Represents an objectoid 16-bit signed value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Int16)]
    [ObjSrcDecodable(typeof(ObjInt16Element))]
    public class ObjSrcInt16 : ObjSrcValuable<short>, IObjSrcDecodable<ObjInt16Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjInt16Element>.Decode(ObjInt16Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcInt16"/></summary>
        public ObjSrcInt16() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjInt16Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int16;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out short result) => Parser.TryToInt16(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "16-bit signed";
    }

    /// <summary>Represents an objectoid 32-bit unsigned value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._UInt32)]
    [ObjSrcDecodable(typeof(ObjUInt32Element))]
    public class ObjSrcUInt32 : ObjSrcValuable<uint>, IObjSrcDecodable<ObjUInt32Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjUInt32Element>.Decode(ObjUInt32Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcUInt32"/></summary>
        public ObjSrcUInt32() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjUInt32Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt32;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out uint result) => Parser.TryToUInt32(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "32-bit unsigned";
    }

    /// <summary>Represents an objectoid 32-bit signed value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Int32)]
    [ObjSrcDecodable(typeof(ObjInt32Element))]
    public class ObjSrcInt32 : ObjSrcValuable<int>, IObjSrcDecodable<ObjInt32Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjInt32Element>.Decode(ObjInt32Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcInt32"/></summary>
        public ObjSrcInt32() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjInt32Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int32;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out int result) => Parser.TryToInt32(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "32-bit signed";
    }

    /// <summary>Represents an objectoid 64-bit unsigned value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._UInt64)]
    [ObjSrcDecodable(typeof(ObjUInt64Element))]
    public class ObjSrcUInt64 : ObjSrcValuable<ulong>, IObjSrcDecodable<ObjUInt64Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjUInt64Element>.Decode(ObjUInt64Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcUInt64"/></summary>
        public ObjSrcUInt64() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjUInt64Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._UInt64;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out ulong result) => Parser.TryToUInt64(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "64-bit unsigned";
    }

    /// <summary>Represents an objectoid 64-bit signed value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Int64)]
    [ObjSrcDecodable(typeof(ObjInt64Element))]
    public class ObjSrcInt64 : ObjSrcValuable<long>, IObjSrcDecodable<ObjInt64Element>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjInt64Element>.Decode(ObjInt64Element element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcInt64"/></summary>
        public ObjSrcInt64() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjInt64Element(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Int64;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out long result) => Parser.TryToInt64(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "64-bit signed";
    }

    /// <summary>Represents an objectoid single-precision floating-point value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Single)]
    [ObjSrcDecodable(typeof(ObjSingleElement))]
    public class ObjSrcSingle : ObjSrcValuable<float>, IObjSrcDecodable<ObjSingleElement>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjSingleElement>.Decode(ObjSingleElement element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcSingle"/></summary>
        public ObjSrcSingle() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjSingleElement(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Single;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out float result) => Parser.TryToSingle(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "single-precision floating-point";
    }

    /// <summary>Represents an objectoid double-precision floating-point value source</summary>
    [ObjSrcReadable(ObjSrcKeyword._Double)]
    [ObjSrcDecodable(typeof(ObjDoubleElement))]
    public class ObjSrcDouble : ObjSrcValuable<double>, IObjSrcDecodable<ObjDoubleElement>
    {
        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjDoubleElement>.Decode(ObjDoubleElement element)
        {
            try { Value = element.Value; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcDouble"/></summary>
        public ObjSrcDouble() { }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjDoubleElement(Value);
        }
        /// <summary>Keyword</summary>
        private protected override string Keyword_p => ObjSrcKeyword._Double;

        /// <inheritdoc/>
        private protected override bool TryParse_m(string s, out double result) => Parser.TryToDouble(s, out result);

        /// <inheritdoc/>
        private protected override string Desc_p => "double-precision floating-point";
    }
}


