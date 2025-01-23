using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal static partial class ObjSrcAttributeUtility
    {
        public abstract class Entry_<TKey, TAttribute>
            where TAttribute : ObjSrcAttribute
        {
            /// <summary>Constructor for <see cref="Entry_{TKey, TAttribute}"/></summary>
            /// <param name="key">Key</param>
            /// <param name="type">Type that contains the attribute</param>
            /// <param name="attribute">Attribute</param>
            private protected Entry_(TKey key, Type type, TAttribute attribute)
            {
                Key = key;
                Type = type;
                Attribute = attribute;
            }

            /// <summary>Key</summary>
            public TKey Key { get; }

            /// <summary>Type that contains the attribute</summary>
            public Type Type { get; }

            /// <summary>Attribute</summary>
            public TAttribute Attribute { get; }
        }
    }

}
