using System;

namespace Objectoid.Source.ElementUtility
{
    /// <summary>Extension methods for <see cref="ObjDocObject"/></summary>
    public static class ObjDocObject_ext
    {
        #region helper

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="object">Object element</param>
        /// <param name="name">Name of property</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="object"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjDocObject @object, ObjNTString name)
        {
            if (@object is null) throw new ArgumentNullException(nameof(@object));
            if (name is null) throw new ArgumentNullException(nameof(name));
            return true;
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="object">Object element</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of element value within property</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="object"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjDocObject @object, ObjNTString name, Type type)
        {
            if (@object is null) throw new ArgumentNullException(nameof(@object));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (type is null) throw new ArgumentNullException(nameof(type));
            return true;
        }

        /// <summary>
        /// Throws an <see cref="ObjSrcElementException"/> explaining that the specified object element does not 
        /// contain a property with the specified name
        /// </summary>
        /// <param name="object">Object element</param>
        /// <param name="name">Name of missing property</param>
        /// <exception cref="ObjSrcElementException">Expected outcome</exception>
        /// <exception cref="ArgumentNullException"><paramref name="object"/> is null</exception>
        private static Exception ThrowElementNotFound_m(ObjDocObject @object, ObjNTString name)
        {
            throw new ObjSrcElementException(@object, $"Object does not contain a property with the name \"{name}\".");
        }

        #endregion

        #region (ObjNTString name, out ObjElement element)

        /// <summary>Attempts to get the element of the property with the specified name</summary>
        /// <param name="object">Objectoid object element</param>
        /// <param name="name">Name of the property</param>
        /// <param name="element">Element value of the retrieved property</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="object"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement(this ObjDocObject @object, ObjNTString name, out ObjElement element)
        {
            try { return @object.TryGetValue(name, out element); }
            catch when (ThrowIfArgumentNull_m(@object, name)) { throw; }
        }

        /// <summary>Gets the element of the property with the specified name</summary>
        /// <param name="object">Objectoid object element</param>
        /// <param name="name">Name of the property</param>
        /// <returns>Element value of the retrieved property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="object"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcSrcElementException"><paramref name="object"/> does not contain a property with the name <paramref name="name"/></exception>
        /// 
        public static ObjElement GetElement(this ObjDocObject @object, ObjNTString name)
        {
            try
            {
                if (@object.TryGetElement(name, out var element))
                    return element;
                throw ThrowElementNotFound_m(@object, name);
            }
            catch when (ThrowIfArgumentNull_m(@object, name)) { throw; }
        }

        #endregion

        #region (ObjNTString name, Type type, out ObjElement element)

        /// <summary>Searches the object element for a property that has the specified name and contains an element value of the specified type</summary>
        /// <param name="object">Object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of element</param>
        /// <param name="element">Element of the found property</param>
        /// <returns>Whether or not a property was found</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement(this ObjDocObject @object, ObjNTString name, Type type, out ObjElement element)
        {
            ThrowIfArgumentNull_m(@object, name, type);
            if (!TryGetElement(@object, name, out element)) goto fail;
            if (!ObjElement_ext.IsOfType(element, type)) goto fail;
            return true;
        fail:
            element = null;
            return false;
        }

        /// <summary>
        /// Searches the object element for a property that has the specified name and contains an element value of the specified type<br/>
        /// If a valid property is not found, an <see cref="ObjSrcException"/> is thrown
        /// </summary>
        /// <param name="object">Object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of element</param>
        /// <returns>Element of the found property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">A valid property could not be found</exception>
        /// 
        public static ObjElement GetElement(this ObjDocObject @object, ObjNTString name, Type type)
        {
            try
            {
                if (TryGetElement(@object, name, type, out var element))
                    return element;
                if (TryGetElement(@object, name, out element))
                    throw ObjElement_ext.ThrowElementNotOfType_m(element, type);
                throw ThrowElementNotFound_m(@object, name);
            }
            catch when (ThrowIfArgumentNull_m(@object, name, type)) { throw; }
        }

        #endregion

        #region <T>(ObjNTString name, out T element)

        /// <summary>Searches the object element for a property that has the specified name and contains an element value of the specified type</summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="object">Object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="element">Element of the found property</param>
        /// <returns>Whether or not a property was found</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement<T>(this ObjDocObject @object, ObjNTString name, out T element)
            where T : ObjElement
        {
            try
            {
                if (TryGetElement(@object, name, typeof(T), out var raw))
                {
                    element = (T)raw;
                    return true;
                }
                else
                {
                    element = null;
                    return false;
                }
            }
            catch when (ThrowIfArgumentNull_m(@object, name)) { throw; }
        }

        /// <summary>
        /// Searches the object element for a property that has the specified name and contains an element value of the specified type<br/>
        /// If a valid property is not found, an <see cref="ObjSrcException"/> is thrown
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="object">Object to search</param>
        /// <param name="name">Name of property</param>
        /// <returns>Element of the found property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">A valid property could not be found</exception>
        /// 
        public static T GetElement<T>(this ObjDocObject @object, ObjNTString name)
            where T : ObjElement
        {
            try
            {
                if (TryGetElement<T>(@object, name, out var element))
                    return element;
                if (TryGetElement(@object, name, out var raw))
                    throw ObjElement_ext.ThrowElementNotOfType_m(raw, typeof(T));
                throw ThrowElementNotFound_m(@object, name);
            }
            catch when (ThrowIfArgumentNull_m(@object, name)) { throw; }
        }

        #endregion
    }
}