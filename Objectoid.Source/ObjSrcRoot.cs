using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid root object source</summary>
    public class ObjSrcRoot : ObjSrcObject
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="document"/> is not null<br/>
        /// <br/>
        /// Called by <see cref="ObjSrcDocument"/>
        /// </remarks>
        internal ObjSrcRoot(ObjSrcDocument document)
        {
            _Document = document;
        }

        /// <inheritdoc/>
        public override bool IsCollectible => false;

        /// <inheritdoc/>
        internal override void AddToCollection_m(ObjSrcCollection collection) =>
            ThrowNotCollectible_m();

        private readonly ObjSrcDocument _Document;
        /// <summary>Document that contains the root</summary>
        public ObjSrcDocument Document => _Document;
    }
}
