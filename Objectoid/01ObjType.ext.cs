using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Objectoid
{
    /// <summary>Extension methods for <see cref="ObjType"/></summary>
    public static class ObjType_ext
    {
        /// <summary>Creates an element</summary>
        /// <returns>A created element</returns>
        private delegate ObjElement Constructor_();

        private static readonly Dictionary<ObjType, Constructor_> _Constructors =
            new Dictionary<ObjType, Constructor_>()
            {
                { ObjType.Null, () => new ObjNullElement() },

                { ObjType.DocObject, () => new ObjDocObject() },
                { ObjType.List, () => new ObjList() },

                { ObjType.NullTerminatedString, () => new ObjNTStringElement() },
                { ObjType.String, () => new ObjStringElement() },

                { ObjType.UInt8, () => new ObjUInt8Element() },
                { ObjType.Int8, () => new ObjInt8Element() },
                { ObjType.UInt16, () => new ObjUInt16Element() },
                { ObjType.Int16, () => new ObjInt16Element() },
                { ObjType.UInt32, () => new ObjUInt32Element() },
                { ObjType.Int32, () => new ObjInt32Element() },
                { ObjType.UInt64, () => new ObjUInt64Element() },
                { ObjType.Int64, () => new ObjInt64Element() },
                { ObjType.Single, () => new ObjSingleElement() },
                { ObjType.Double, () => new ObjDoubleElement() },
                { ObjType.Bool, () => new ObjBoolElement() },
            };

        /// <summary>Attempts to create an element of the specified type</summary>
        /// <param name="type">Type</param>
        /// <param name="element">Created element</param>
        /// <returns>Whether or not successful</returns>
        public static bool TryCreate(this ObjType type, out ObjElement element)
        {
            if (_Constructors.TryGetValue(type, out Constructor_ constructor))
            {
                element = constructor();
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }
    }
}
