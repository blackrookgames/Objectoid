using System;
using System.Reflection;
using static Objectoid.Source.ObjSrcAttributeUtility;

namespace Objectoid.Source
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ObjSrcHeadAttribute : ObjSrcAttribute
    {
        /// <summary>Creates an instance of <see cref="ObjSrcHeadAttribute"/></summary>
        /// <param name="keyword">Keyword</param>
        public ObjSrcHeadAttribute(string keyword) { Keyword = keyword; }

        /// <summary>Keyword</summary>
        public string Keyword { get; }
    }

    internal class ObjSrcHeadCollection : Collection_<string, ObjSrcHeadAttribute, ObjSrcHead>
    {
        /// <inheritdoc/>
        private protected override bool TryCreate_m(ObjSrcHeadAttribute attribute, Type type, out ObjSrcHead entry) =>
            ObjSrcHead.TryCreate(attribute, type, out entry);
    }

    internal class ObjSrcHead : Entry_<string, ObjSrcHeadAttribute>
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="type"/> is not null<br/>
        /// <paramref name="attribute"/> is not null<br/>
        /// <paramref name="constructor"/> is not null
        /// </remarks>
        private ObjSrcHead(Type type, ObjSrcHeadAttribute attribute, ConstructorInfo constructor) : 
            base(attribute.Keyword, type, attribute)
        {
            _Constructor = constructor;
        }

        /// <inheritdoc cref="ObjSrcHeadCollection.TryCreate_m"/>
        public static bool TryCreate(ObjSrcHeadAttribute attribute, Type type, out ObjSrcHead entry)
        {
            try
            {
                if (attribute is null) goto fail;

                if (type.IsAbstract) goto fail;
                if (!type.IsSubclassOf(typeof(ObjSrcHeaderStatement))) goto fail;
                
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null) goto fail;

                entry = new ObjSrcHead(type, attribute, constructor);
                return true;
            fail:
                entry = null;
                return false;
            }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        private readonly ConstructorInfo _Constructor;

        public ObjSrcHeaderStatement Create() => (ObjSrcHeaderStatement)_Constructor.Invoke(new object[0]);
    }
}
