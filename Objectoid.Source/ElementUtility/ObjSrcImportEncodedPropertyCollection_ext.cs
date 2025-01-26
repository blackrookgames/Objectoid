using System;

namespace Objectoid.Source.ElementUtility
{
    /// <summary>Extension methods for <see cref="ObjSrcImportEncodedPropertyCollection"/></summary>
    public static class ObjSrcImportEncodedPropertyCollection_ext
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

        /// <summary>
        /// Throws an <see cref="ObjSrcSrcElementException"/> explaining that an encoded property collection does not 
        /// contain a property with the specified name
        /// </summary>
        /// <param name="name">Name of missing property</param>
        /// <exception cref="ObjSrcSrcElementException">Expected outcome</exception>
        private static Exception ThrowPropertyNotFound(ObjNTString name)
        {
            throw new ObjSrcException($"Could not find a property of the name \"{name}\".");
        }

        #endregion

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
            catch when (ThrowIfArgumentNull_m(collection, name)) { throw; }
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
        /// <exception cref="ObjSrcException"><paramref name="collection"/> does not contain a property with the name <paramref name="name"/></exception>
        /// 
        public static ObjSrcImportEncodedProperty GetProperty(this ObjSrcImportEncodedPropertyCollection collection, ObjNTString name)
        {
            try
            {
                if (TryGetProperty(collection, name, out var property))
                    return property;
                throw ThrowPropertyNotFound(name);
            }
            catch when (ThrowIfArgumentNull_m(collection, name)) { throw; }
        }
    }
}
