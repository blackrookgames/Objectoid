using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal interface IObjSrcDecodable
    {
        /// <summary>Decodes data from the specified element</summary>
        /// <param name="element">Element to decode</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="element"/> is not of a supported type</exception>
        void Decode(ObjElement element);
    }

    internal interface IObjSrcDecodable<T> : IObjSrcDecodable
        where T : ObjElement
    {
        void IObjSrcDecodable.Decode(ObjElement element)
        {
            try { Decode((T)element); }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
            catch when (!(element is T)) { throw new ArgumentException($"The specified element is not an instance of {typeof(T).Name}.", nameof(element)); }
        }

        /// <summary>Decodes data from the specified element</summary>
        /// <param name="element">Element to decode</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        void Decode(T element);
    }
}
