using System;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    internal enum ObjSrcReaderTokenType
    {
        /// <summary>None (used for end of parse block)</summary>
        None, //Leave at zero
        /// <summary>Keyword</summary>
        Keyword,
        /// <summary>Numeric value</summary>
        Numeric,
        /// <summary>String value</summary>
        String,
        /// <summary>Constant value (first character is alphabetical)</summary>
        Constant,
    }
}
