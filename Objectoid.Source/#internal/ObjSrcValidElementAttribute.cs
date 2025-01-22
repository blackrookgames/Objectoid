using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ObjSrcValidElementAttribute : Attribute
    {
        /// <summary>Creates an instance of <see cref="ObjSrcValidElementAttribute"/></summary>
        /// <param name="keyword">Keyword</param>
        public ObjSrcValidElementAttribute(string keyword)
        {
            Keyword = keyword;
        }

        /// <summary>Keyword</summary>
        public string Keyword { get; }
    }
}
