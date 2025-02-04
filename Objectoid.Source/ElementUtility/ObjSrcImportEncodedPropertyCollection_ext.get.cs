using System;

namespace Objectoid.Source.ElementUtility
{
    public static partial class ObjSrcImportEncodedPropertyCollection_ext
    {
        #region SourceElement

        #region GetSourceElement(ObjNTString)

        /// <summary>Attempts to get the source element of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="sourceElement">Retrieved source element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetSourceElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, out ObjSrcElement sourceElement)
        {
            try
            {
                if (TryGetProperty(collection, name, out var property))
                {
                    sourceElement = property.SourceValue;
                    return true;
                }
                else
                {
                    sourceElement = null;
                    return false;
                }
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        /// <summary>Gets the source element of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>Retrieved source element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// </exception>
        /// 
        public static ObjSrcElement GetSourceElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
        {
            try
            {
                var property = GetProperty(collection, name);
                return property.SourceValue;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #region GetSourceElement(ObjNTString, Type)

        /// <summary>Attempts to get the source element of the specified type and of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Source element type</param>
        /// <param name="sourceElement">Retrieved source element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        public static bool TryGetSourceElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, Type type, out ObjSrcElement sourceElement)
        {
            try
            {
                if (!TryGetSourceElement(collection, name, out sourceElement))
                {
                    if (type is null) throw new ArgumentNullException(); //Null check
                    goto fail;
                }
                if (!sourceElement.IsOfType(type))
                    goto fail;
                return true;
            fail:
                sourceElement = null;
                return false;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name, type), out var e)) { throw e; }
        }

        /// <summary>Gets the source element of the specified type and of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Source element type</param>
        /// <returns>Retrieved source element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// <br/>or<br/>
        /// Retrieved source element is not of the type <paramref name="type"/>
        /// </exception>
        /// 
        public static ObjSrcElement GetSourceElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, Type type)
        {
            try
            {
                var sourceElement = GetSourceElement(collection, name);
                if (!sourceElement.IsOfType(type)) throw ObjSrcElement_ext.ThrowElementNotOfType_m(sourceElement, type);
                return sourceElement;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name, type), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #region GetSourceElement<T>(ObjNTString)

        /// <summary>Attempts to get the source element of the specified type and of a property with the specified name</summary>
        /// <typeparam name="T">Source element type</typeparam>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="sourceElement">Retrieved source element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetSourceElement<T>(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, out T sourceElement)
            where T : ObjSrcElement
        {
            try
            {
                if (TryGetSourceElement(collection, name, typeof(T), out var raw))
                {
                    sourceElement = (T)raw;
                    return true;
                }
                else
                {
                    sourceElement = null;
                    return false;
                }
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        /// <summary>Gets the source element of the specified type and of a property with the specified name</summary>
        /// <typeparam name="T">Source element type</typeparam>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>Retrieved source element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// <br/>or<br/>
        /// Retrieved source element is not of the type <typeparamref name="T"/>
        /// </exception>
        /// 
        public static T GetSourceElement<T>(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
            where T : ObjSrcElement
        {
            try { return (T)GetSourceElement(collection, name, typeof(T)); }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #endregion
        
        #region EncodedElement

        #region GetEncodedElement(ObjNTString)

        /// <summary>Attempts to get the encoded element of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="encodedElement">Retrieved encoded element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetEncodedElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, out ObjElement encodedElement)
        {
            try
            {
                if (TryGetProperty(collection, name, out var property))
                {
                    encodedElement = property.EncodedValue;
                    return true;
                }
                else
                {
                    encodedElement = null;
                    return false;
                }
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        /// <summary>Gets the encoded element of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>Retrieved encoded element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// </exception>
        /// 
        public static ObjElement GetEncodedElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
        {
            try
            {
                var property = GetProperty(collection, name);
                return property.EncodedValue;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #region GetEncodedElement(ObjNTString, Type)

        /// <summary>Attempts to get the encoded element of the specified type and of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Source element type</param>
        /// <param name="encodedElement">Retrieved encoded element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        public static bool TryGetEncodedElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, Type type, out ObjElement encodedElement)
        {
            try
            {
                if (!TryGetEncodedElement(collection, name, out encodedElement))
                {
                    if (type is null) throw new ArgumentNullException(); //Null check
                    goto fail;
                }
                if (!encodedElement.IsOfType(type))
                    goto fail;
                return true;
            fail:
                encodedElement = null;
                return false;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name, type), out var e)) { throw e; }
        }

        /// <summary>Gets the encoded element of the specified type and of a property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Source element type</param>
        /// <returns>Retrieved encoded element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// <br/>or<br/>
        /// Retrieved encoded element is not of the type <paramref name="type"/>
        /// </exception>
        /// 
        public static ObjElement GetEncodedElement(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, Type type)
        {
            try
            {
                var encodedElement = GetEncodedElement(collection, name);
                if (!encodedElement.IsOfType(type)) throw ObjElement_ext.ThrowElementNotOfType_m(encodedElement, type);
                return encodedElement;
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name, type), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #region GetEncodedElement<T>(ObjNTString)

        /// <summary>Attempts to get the encoded element of the specified type and of a property with the specified name</summary>
        /// <typeparam name="T">Source element type</typeparam>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="encodedElement">Retrieved encoded element</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetEncodedElement<T>(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, out T encodedElement)
            where T : ObjElement
        {
            try
            {
                if (TryGetEncodedElement(collection, name, typeof(T), out var raw))
                {
                    encodedElement = (T)raw;
                    return true;
                }
                else
                {
                    encodedElement = null;
                    return false;
                }
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        /// <summary>Gets the encoded element of the specified type and of a property with the specified name</summary>
        /// <typeparam name="T">Source element type</typeparam>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>Retrieved encoded element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// <paramref name="collection"/> does not contain a property with the name <paramref name="name"/>
        /// <br/>or<br/>
        /// Retrieved encoded element is not of the type <typeparamref name="T"/>
        /// </exception>
        /// 
        public static T GetEncodedElement<T>(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
            where T : ObjElement
        {
            try { return (T)GetEncodedElement(collection, name, typeof(T)); }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
            catch (ObjSrcException e) { throw e; }
        }

        #endregion

        #endregion
    }
}

