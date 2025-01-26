using System;

namespace Objectoid.Source.ElementUtility
{
    /// <summary>Extension methods for <see cref="ObjElement"/></summary>
    public static class ObjElement_ext
    {
        #region helper

        /// <summary>Throws an <see cref="ObjSrcElementException"/> explaining that the specified element is not of the specified type</summary>
        /// <param name="element">Element</param>
        /// <param name="type">Intended type</param>
        /// <exception cref="ObjSrcElementException">Expected outcome</exception>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        internal static Exception ThrowElementNotOfType_m(ObjElement element, Type type)
        {
            throw new ObjSrcElementException(element, $"Element is not of the type \"{type?.Name}\".");
        }

        #endregion

        /// <summary>Checks if the specified element is of the specified type</summary>
        /// <param name="element">Element</param>
        /// <param name="type">Type</param>
        /// <returns>Whether or not <paramref name="element"/> is of the type <paramref name="type"/></returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="element"/> is null
        /// <br/>or<br/>
        /// <paramref name="type"/> is null
        /// </exception>
        /// 
        public static bool IsOfType(this ObjElement element, Type type)
        {
            try { return type.IsAssignableFrom(element.GetType()); }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
            catch when (type is null) { throw new ArgumentNullException(nameof(type)); }
        }

        /// <summary>Attempts to cast the specified element to the specified type</summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="element">Element</param>
        /// <param name="result">Element casted to <typeparamref name="T"/></param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        public static bool TryCastAs<T>(this ObjElement element, out T result)
            where T : ObjElement
        {
            try
            {
                if (IsOfType(element, typeof(T)))
                {
                    result = (T)element;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        /// <summary>Casts the specified element to the specified type</summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="element">Element</param>
        /// <returns>Element casted to <typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ObjSrcException"><paramref name="element"/> is not of the type <typeparamref name="T"/></exception>
        public static T CastAs<T>(this ObjElement element)
            where T : ObjElement
        {
            try
            {
                if (TryCastAs<T>(element, out T result))
                    return result;
                throw new ObjSrcElementException(element, $"Element is not of the type \"{typeof(T).Name}\".");
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }
    }
}
