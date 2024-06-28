using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents the root object in an Objectoid document</summary>
    public class ObjRootObject : ObjDocObject
    {
        /// <summary>Constructor for <see cref="ObjRootObject"/>
        /// <br/>NOTE: It is assumed <paramref name="document"/> is not null
        /// <br/>CALLED BY: <see cref="ObjDocument"/></summary>
        /// <param name="document">The document in which this object is rooted</param>
        internal ObjRootObject(ObjDocument document) : 
            base(false)
        {
            _Document = document;
        }

        #region Document

        private readonly ObjDocument _Document;

        /// <summary>The document in which this object is rooted</summary>
        public ObjDocument Document => _Document;

        #endregion
    }
}
