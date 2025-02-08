using System;
using System.Reflection;
using static Objectoid.Source.ObjSrcAttributeUtility;

namespace Objectoid.Source
{
    internal class ObjSrcEnumCompatibleAttribute : ObjSrcAttribute
    {
        /// <summary>Creates an instance of <see cref="ObjSrcEnumCompatibleAttribute"/></summary>
        /// <param name="underlyingType">An enumeration's underlying type</param>
        public ObjSrcEnumCompatibleAttribute(Type underlyingType) { UnderlyingType = underlyingType; }

        /// <summary>An enumeration's underlying type</summary>
        public Type UnderlyingType { get; }
    }

    internal class ObjSrcEnumCompatibleCollection : Collection_<Type, ObjSrcEnumCompatibleAttribute, ObjSrcEnumCompatible>
    {
        /// <inheritdoc/>
        private protected override bool TryCreate_m(ObjSrcEnumCompatibleAttribute attribute, Type type, out ObjSrcEnumCompatible entry) =>
            ObjSrcEnumCompatible.TryCreate(attribute, type, out entry);
    }

    internal class ObjSrcEnumCompatible : Entry_<Type, ObjSrcEnumCompatibleAttribute>
    {
        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="type"/> is not null<br/>
        /// <paramref name="attribute"/> is not null<br/>
        /// <paramref name="constructor"/> is not null
        /// </remarks>
        private ObjSrcEnumCompatible(Type type, ObjSrcEnumCompatibleAttribute attribute, ConstructorInfo constructor) : 
            base(attribute.UnderlyingType, type, attribute)
        {
            _Constructor = constructor;
        }

        /// <inheritdoc cref="ObjSrcEnumCompatibleCollection.TryCreate_m"/>
        public static bool TryCreate(ObjSrcEnumCompatibleAttribute attribute, Type type, out ObjSrcEnumCompatible entry)
        {
            try
            {
                if (attribute is null) goto fail;

                if (type.IsAbstract) goto fail;
                if (!type.IsSubclassOf(typeof(ObjSrcElement))) goto fail;
                if (!(typeof(IObjSrcEnumCompatible).IsAssignableFrom(type))) goto fail;

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null) goto fail;

                entry = new ObjSrcEnumCompatible(type, attribute, constructor);
                return true;
            fail:
                entry = null;
                return false;
            }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        private readonly ConstructorInfo _Constructor;

        /// <summary>Creates an instance of <see cref="IObjSrcEnumCompatible"/></summary>
        /// <returns>Created instance of <see cref="IObjSrcEnumCompatible"/></returns>
        public IObjSrcEnumCompatible Create() => (IObjSrcEnumCompatible)_Constructor.Invoke(new object[0]);
    }
}
