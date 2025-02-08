using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    internal static class ObjSrcKeyword
    {
        #region header

        ///<summary>Declaration of protocol prefix</summary>
        public const string _ProtocolPrefix = "@ProtocolPrefix";

        ///<summary>Declaration of identifier</summary>
        public const string _Identifier = "@Identifier";

        #endregion

        #region element

        ///<summary>Declaration of string value</summary>
        public const string _String = "@String";

        ///<summary>Declaration of null-terminated string value</summary>
        public const string _NTString = "@NTString";

        ///<summary>Declaration of enumeration value</summary>
        public const string _Enum = "@Enum";

        ///<summary>Declaration of boolean value</summary>
        public const string _Bool = "@Bool";

        ///<summary>Declaration of 8-bit unsigned value</summary>
        public const string _UInt8 = "@UInt8";

        ///<summary>Declaration of 16-bit unsigned value</summary>
        public const string _UInt16 = "@UInt16";

        ///<summary>Declaration of 32-bit unsigned value</summary>
        public const string _UInt32 = "@UInt32";

        ///<summary>Declaration of 64-bit unsigned value</summary>
        public const string _UInt64 = "@UInt64";

        ///<summary>Declaration of 8-bit unsigned value</summary>
        public const string _Int8 = "@Int8";

        ///<summary>Declaration of 16-bit unsigned value</summary>
        public const string _Int16 = "@Int16";

        ///<summary>Declaration of 32-bit unsigned value</summary>
        public const string _Int32 = "@Int32";

        ///<summary>Declaration of 64-bit unsigned value</summary>
        public const string _Int64 = "@Int64";

        ///<summary>Declaration of single-precision floating-point value</summary>
        public const string _Single = "@Single";

        ///<summary>Declaration of double-precision floating-point value</summary>
        public const string _Double = "@Double";

        ///<summary>Declaration of an array of raw bytes</summary>
        public const string _RawBytes = "@RawBytes";

        ///<summary>Null declaration</summary>
        public const string _Null = "@Null";

        ///<summary>Declaration of an object block</summary>
        public const string _Object = "@Object";

        ///<summary>Declaration of a list block</summary>
        public const string _List = "@List";

        ///<summary>Declaration of an import block</summary>
        public const string _Import = "@Import";

        ///<summary>End of an object block</summary>
        public const string _EndObject = "@EndObject";

        ///<summary>End of an list block</summary>
        public const string _EndList = "@EndList";

        ///<summary>End of an import block</summary>
        public const string _EndImport = "@EndImport";

        #endregion
    }
}
