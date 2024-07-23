using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents the data type of an Objectoid element</summary>
    public enum ObjType : byte
    {
        #region Null

        ///<summary>Null</summary>
        Null = 0xFF,

        #endregion

        #region Collections

        ///<summary>Document object</summary>
        DocObject = 0x00,

        ///<summary>List</summary>
        List = 0x01,

        #endregion

        #region String

        ///<summary>Null terminated string</summary>
        NullTerminatedString = 0x10,

        ///<summary>String</summary>
        String = 0x11,

        #endregion

        #region Integer, Float, Boolean

        ///<summary>Unsigned 8-bit integer</summary>
        UInt8 = 0x80,

        ///<summary>Signed 8-bit integer</summary>
        Int8 = 0x81,

        ///<summary>Unsigned 16-bit integer</summary>
        UInt16 = 0x82,

        ///<summary>Signed 16-bit integer</summary>
        Int16 = 0x83,

        ///<summary>Unsigned 32-bit integer</summary>
        UInt32 = 0x84,

        ///<summary>Signed 32-bit integer</summary>
        Int32 = 0x85,

        ///<summary>Unsigned 64-bit integer</summary>
        UInt64 = 0x86,

        ///<summary>Signed 64-bit integer</summary>
        Int64 = 0x87,

        ///<summary>Single-precision floating-point</summary>
        Single = 0x88,

        ///<summary>Double-precision floating-point</summary>
        Double = 0x89,

        ///<summary>Boolean</summary>
        Bool = 0x8A,

        #endregion

        #region Misc

        /// <summary>Raw byte data</summary>
        RawBytes = 0x90,

        #endregion
    }
}
