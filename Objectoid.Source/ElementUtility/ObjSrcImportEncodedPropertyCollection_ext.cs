using System;

namespace Objectoid.Source.ElementUtility
{
    /// <summary>Extension methods for <see cref="ObjSrcImportEncodedPropertyCollection"/></summary>
    public static partial class ObjSrcImportEncodedPropertyCollection_ext
    {
        #region helper

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
        {
            if (collection is null) throw new ArgumentNullException(nameof(collection));
            if (name is null) throw new ArgumentNullException(nameof(name));
            return true;
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if any of the arguments are null</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of element</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        private static bool ThrowIfArgumentNull_m(ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, Type type)
        {
            if (collection is null) throw new ArgumentNullException(nameof(collection));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (type is null) throw new ArgumentNullException(nameof(type));
            return true;
        }

        /// <summary>
        /// Throws an <see cref="ObjSrcSrcElementException"/> explaining that an encoded property collection does not 
        /// contain a property with the specified name
        /// </summary>
        /// <param name="name">Name of missing property</param>
        /// <param name="source">Import source</param>
        /// <exception cref="ObjSrcSrcElementException">Expected outcome</exception>
        private static Exception ThrowPropertyNotFound_m(ObjSrcImport source, ObjNTString name)
        {
            try { throw new ObjSrcSrcElementException(source, $"Could not find a property of the name \"{name}\"."); }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
        }

        #endregion

        #region GetProperty(ObjNTString)

        /// <summary>Attempts to get the property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <param name="property">Retrieved property</param>
        /// <returns>Whether or not successful</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null
        /// <br/>or<br/>
        /// <paramref name="name"/> is null
        /// </exception>
        /// 
        public static bool TryGetProperty(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name, out ObjSrcImportEncodedProperty property)
        {
            try { return collection.TryGet(name, out property); }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        /// <summary>Gets the property with the specified name</summary>
        /// <param name="collection">Collection of encoded properties</param>
        /// <param name="name">Name of property</param>
        /// <returns>Retrieved property</returns>
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
        public static ObjSrcImportEncodedProperty GetProperty(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
        {
            try
            {
                if (collection.TryGet(name, out var property))
                    return property;
                throw ThrowPropertyNotFound_m(collection.Source, name);
            }
            catch when (!ThrowUtility.Test(() => ThrowIfArgumentNull_m(collection, name), out var e)) { throw e; }
        }

        #endregion
    }
}
