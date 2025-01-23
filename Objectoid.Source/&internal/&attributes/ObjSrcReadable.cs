using System;
using System.Reflection;
using static Objectoid.Source.ObjSrcAttributeUtility;

namespace Objectoid.Source
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ObjSrcReadableAttribute : ObjSrcAttribute
    {
        /// <summary>Creates an instance of <see cref="ObjSrcReadableAttribute"/></summary>
        /// <param name="keyword">Keyword</param>
        public ObjSrcReadableAttribute(string keyword) { Keyword = keyword; }

        /// <summary>Keyword</summary>
        public string Keyword { get; }
    }

    internal class ObjSrcReadableCollection : Collection_<string, ObjSrcReadableAttribute, ObjSrcReadable>
    {
        /// <inheritdoc/>
        private protected override bool TryCreate_m(ObjSrcReadableAttribute attribute, Type type, out ObjSrcReadable entry) =>
            ObjSrcReadable.TryCreate(attribute, type, out entry);
    }

    internal class ObjSrcReadable : Entry_<string, ObjSrcReadableAttribute>
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="type"/> is not null<br/>
        /// <paramref name="attribute"/> is not null<br/>
        /// <paramref name="constructor"/> is not null
        /// </remarks>
        private ObjSrcReadable(Type type, ObjSrcReadableAttribute attribute, ConstructorInfo constructor) : 
            base(attribute.Keyword, type, attribute)
        {
            _Constructor = constructor;
        }

        /// <inheritdoc cref="ObjSrcReadableCollection.TryCreate_m"/>
        public static bool TryCreate(ObjSrcReadableAttribute attribute, Type type, out ObjSrcReadable entry)
        {
            try
            {
                if (attribute is null) goto fail;

                if (type.IsAbstract) goto fail;
                if (!type.IsSubclassOf(typeof(ObjSrcElement))) goto fail;
                
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null) goto fail;

                entry = new ObjSrcReadable(type, attribute, constructor);
                return true;
            fail:
                entry = null;
                return false;
            }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        private readonly ConstructorInfo _Constructor;

        public ObjSrcElement Create() => (ObjSrcElement)_Constructor.Invoke(new object[0]);
    }
}
