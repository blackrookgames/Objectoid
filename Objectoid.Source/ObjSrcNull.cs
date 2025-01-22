using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid object source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Null)]
    public class ObjSrcNull : ObjSrcElement
    {
        /// <summary>Creates an instance of <see cref="ObjSrcNull"/></summary>
        public ObjSrcNull() { }

        /// <inheritdoc/>
        internal sealed override void Load_m(ObjSrcReader reader)
        {
            try
            {
                //Ensure there's only whitespace after @Null
                reader.Read();
                reader.Token.ThrowIfNotEOL_m();
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal sealed override void Save_m(ObjSrcWriter writer)
        {
            try { writer.WriteLine(ObjSrcKeyword._Null); }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }
    }
}
