using System;
using System.Reflection;
using static Objectoid.Source.ObjSrcAttributeUtility;

namespace Objectoid.Source
{
    internal class ObjSrcDecodableAttribute : ObjSrcAttribute
    {
        /// <summary>Creates an instance of <see cref="ObjSrcDecodableAttribute"/></summary>
        /// <param name="elementType">Type of Objectoid element that can be decoded</param>
        public ObjSrcDecodableAttribute(Type elementType) { ElementType = elementType; }

        /// <summary>Type of Objectoid element that can be decoded</summary>
        public Type ElementType { get; }
    }

    internal class ObjSrcDecodableCollection : Collection_<Type, ObjSrcDecodableAttribute, ObjSrcDecodable>
    {
        /// <inheritdoc/>
        private protected override bool TryCreate_m(ObjSrcDecodableAttribute attribute, Type type, out ObjSrcDecodable entry) =>
            ObjSrcDecodable.TryCreate(attribute, type, out entry);
    }

    internal class ObjSrcDecodable : Entry_<Type, ObjSrcDecodableAttribute>
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="type"/> is not null<br/>
        /// <paramref name="attribute"/> is not null<br/>
        /// <paramref name="constructor"/> is not null
        /// </remarks>
        private ObjSrcDecodable(Type type, ObjSrcDecodableAttribute attribute, ConstructorInfo constructor) : 
            base(attribute.ElementType, type, attribute)
        {
            _Constructor = constructor;
        }

        /// <inheritdoc cref="ObjSrcDecodableCollection.TryCreate_m"/>
        public static bool TryCreate(ObjSrcDecodableAttribute attribute, Type type, out ObjSrcDecodable entry)
        {
            try
            {
                if (attribute is null) goto fail;

                if (type.IsAbstract) goto fail;
                if (!type.IsSubclassOf(typeof(ObjSrcElement))) goto fail;
                if (!(typeof(IObjSrcDecodable).IsAssignableFrom(type))) goto fail;

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null) goto fail;

                entry = new ObjSrcDecodable(type, attribute, constructor);
                return true;
            fail:
                entry = null;
                return false;
            }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        private readonly ConstructorInfo _Constructor;

        public IObjSrcDecodable Create() => (IObjSrcDecodable)_Constructor.Invoke(new object[0]);
    }
}
