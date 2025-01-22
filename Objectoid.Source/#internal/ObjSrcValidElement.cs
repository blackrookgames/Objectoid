using System;
using System.Reflection;

namespace Objectoid.Source
{
    internal class ObjSrcValidElement
    {
        private ObjSrcValidElement(ObjSrcValidElementAttribute attribute, Type type, ConstructorInfo constructor)
        {
            _Attribute = attribute;
            _Type = type;
            _Constructor = constructor;
        }

        /// <summary>Attempts to create an instance of <see cref="ObjSrcValidElement"/> out of the specified type</summary>
        /// <param name="type">Type</param>
        /// <param name="validElement">Created instnace of <see cref="ObjSrcValidElement"/></param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null</exception>
        public static bool TryCreate(Type type, out ObjSrcValidElement validElement)
        {
            try
            {
                if (type.IsAbstract) goto fail;
                if (!type.IsSubclassOf(typeof(ObjSrcElement))) goto fail;
                
                //Attribute
                var attribute = type.GetCustomAttribute<ObjSrcValidElementAttribute>();
                if (attribute is null) goto fail;

                //Constructor
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null) goto fail;

                validElement = new ObjSrcValidElement(attribute, type, constructor);
                return true;
            fail:
                validElement = null;
                return false;
            }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        private readonly ObjSrcValidElementAttribute _Attribute;
        private readonly Type _Type;
        private readonly ConstructorInfo _Constructor;

        /// <summary>Attribute</summary>
        public ObjSrcValidElementAttribute Attribute => _Attribute;

        /// <summary>Type</summary>
        public Type Type => _Type;

        /// <summary>Creates an objectoid element of the type <see cref="Type"/></summary>
        /// <returns>Created objectoid element</returns>
        public ObjSrcElement Create() => (ObjSrcElement)_Constructor.Invoke(new object[0]);
    }
}
