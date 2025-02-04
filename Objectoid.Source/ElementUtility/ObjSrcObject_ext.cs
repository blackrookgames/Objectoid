using System;

namespace Objectoid.Source.ElementUtility
{
    /// <summary>Extension methods for <see cref="ObjSrcObject"/></summary>
    public static class ObjSrcObject_ext
    {
        #region helper

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="srcObject">Source object element</param>
        /// <param name="name">Name of property</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjSrcObject srcObject, ObjNTString name)
        {
            if (srcObject is null) throw new ArgumentNullException(nameof(srcObject));
            if (name is null) throw new ArgumentNullException(nameof(name));
            return true;
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="srcObject">Source object element</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of element value within property</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjSrcObject srcObject, ObjNTString name, Type type)
        {
            if (srcObject is null) throw new ArgumentNullException(nameof(srcObject));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (type is null) throw new ArgumentNullException(nameof(type));
            return true;
        }

        /// <summary>
        /// Throws an <see cref="ObjSrcSrcElementException"/> explaining that the specified source object element does not 
        /// contain a property with the specified name
        /// </summary>
        /// <param name="srcObject">Source object element</param>
        /// <param name="name">Name of missing property</param>
        /// <exception cref="ObjSrcSrcElementException">Expected outcome</exception>
        /// <exception cref="ArgumentNullException"><paramref name="srcObject"/> is null</exception>
        private static Exception ThrowElementNotFound_m(ObjSrcObject srcObject, ObjNTString name)
        {
            throw new ObjSrcSrcElementException(srcObject, $"Source object does not contain a property with the name \"{name}\".");
        }

        #endregion

        #region (ObjNTString name, out ObjSrcElement srcElement)

        /// <summary>Attempts to get the source element of the property with the specified name</summary>
        /// <param name="srcObject">Objectoid source object element</param>
        /// <param name="name">Name of the property</param>
        /// <param name="srcElement">Source element value of the retrieved property</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="srcObject"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement(this ObjSrcObject srcObject, ObjNTString name, out ObjSrcElement srcElement)
        {
            try { return srcObject.TryGetValue(name, out srcElement); }
            catch { ThrowIfArgumentNull_m(srcObject, name); throw; }
        }

        /// <summary>Gets the source element of the property with the specified name</summary>
        /// <param name="srcObject">Objectoid source object element</param>
        /// <param name="name">Name of the property</param>
        /// <returns>Source element value of the retrieved property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="srcObject"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcSrcElementException"><paramref name="srcObject"/> does not contain a property with the name <paramref name="name"/></exception>
        /// 
        public static ObjSrcElement GetElement(this ObjSrcObject srcObject, ObjNTString name)
        {
            try
            {
                if (srcObject.TryGetElement(name, out var element))
                    return element;
                throw ThrowElementNotFound_m(srcObject, name);
            }
            catch { ThrowIfArgumentNull_m(srcObject, name); throw; }
        }

        #endregion

        #region (ObjNTString name, Type type, out ObjSrcElement srcElement)

        /// <summary>Searches the source object element for a property that has the specified name and contains a source element value of the specified type</summary>
        /// <param name="srcObject">Source object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of source element</param>
        /// <param name="srcElement">Source element of the found property</param>
        /// <returns>Whether or not a property was found</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement(this ObjSrcObject srcObject, ObjNTString name, Type type, out ObjSrcElement srcElement)
        {
            ThrowIfArgumentNull_m(srcObject, name, type);
            if (!TryGetElement(srcObject, name, out srcElement)) goto fail;
            if (!ObjSrcElement_ext.IsOfType(srcElement, type)) goto fail;
            return true;
        fail:
            srcElement = null;
            return false;
        }

        /// <summary>
        /// Searches the source object element for a property that has the specified name and contains a source element value of the specified type<br/>
        /// If a valid property is not found, an <see cref="ObjSrcException"/> is thrown
        /// </summary>
        /// <param name="srcObject">Source object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of source element</param>
        /// <returns>Source element of the found property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">A valid property could not be found</exception>
        /// 
        public static ObjSrcElement GetElement(this ObjSrcObject srcObject, ObjNTString name, Type type)
        {
            try
            {
                if (TryGetElement(srcObject, name, type, out var element))
                    return element;
                if (TryGetElement(srcObject, name, out element))
                    throw ObjSrcElement_ext.ThrowElementNotOfType_m(element, type);
                throw ThrowElementNotFound_m(srcObject, name);
            }
            catch { ThrowIfArgumentNull_m(srcObject, name, type); throw; }
        }

        #endregion

        #region <T>(ObjNTString name, out T srcElement)

        /// <summary>Searches the source object element for a property that has the specified name and contains a source element value of the specified type</summary>
        /// <typeparam name="T">Type of source element</typeparam>
        /// <param name="srcObject">Source object to search</param>
        /// <param name="name">Name of property</param>
        /// <param name="srcElement">Source element of the found property</param>
        /// <returns>Whether or not a property was found</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetElement<T>(this ObjSrcObject srcObject, ObjNTString name, out T srcElement)
            where T : ObjSrcElement
        {
            try
            {
                if (TryGetElement(srcObject, name, typeof(T), out var raw))
                {
                    srcElement = (T)raw;
                    return true;
                }
                else
                {
                    srcElement = null;
                    return false;
                }
            }
            catch { ThrowIfArgumentNull_m(srcObject, name); throw; }
        }

        /// <summary>
        /// Searches the source object element for a property that has the specified name and contains a source element value of the specified type<br/>
        /// If a valid property is not found, an <see cref="ObjSrcException"/> is thrown
        /// </summary>
        /// <typeparam name="T">Type of source element</typeparam>
        /// <param name="srcObject">Source object to search</param>
        /// <param name="name">Name of property</param>
        /// <returns>Source element of the found property</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="srcObject"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">A valid property could not be found</exception>
        /// 
        public static T GetElement<T>(this ObjSrcObject srcObject, ObjNTString name)
            where T : ObjSrcElement
        {
            try
            {
                if (TryGetElement<T>(srcObject, name, out var element))
                    return element;
                if (TryGetElement(srcObject, name, out var raw))
                    throw ObjSrcElement_ext.ThrowElementNotOfType_m(raw, typeof(T));
                throw ThrowElementNotFound_m(srcObject, name);
            }
            catch { ThrowIfArgumentNull_m(srcObject, name); throw; }
        }

        #endregion
    }
}