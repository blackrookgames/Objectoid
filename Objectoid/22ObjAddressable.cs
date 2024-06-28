using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Base class for all Objectoid elements that can be addressed</summary>
    public abstract class ObjAddressable : ObjElement
    {
        /// <summary>Represents an equality comparer for instances of <see cref="ObjAddressable"/></summary>
        internal class EqualityComparer_ : IEqualityComparer<ObjAddressable>
        {
            bool IEqualityComparer<ObjAddressable>.Equals(ObjAddressable x, ObjAddressable y)
            {
                if (x is null) return (y is null);
                return x.Equals_m(y);
            }

            int IEqualityComparer<ObjAddressable>.GetHashCode(ObjAddressable obj)
            {
                try
                {
                    return obj.GetHashCode_m();
                }
                catch when (obj == null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }
            }
        }

        /// <summary>Constructor for <see cref="ObjAddressable"/></summary>
        /// <param name="type">Data type</param>
        /// <param name="isCollectable">Whether or not the element can be part of a collection</param>
        private protected ObjAddressable(ObjType type, bool isCollectable) :
            base(type, isCollectable)
        { }

        /// <summary>Checks if the specified addressable element is equal in value to the current addressable element</summary>
        /// <param name="other">Other addressable element</param>
        /// <returns>Whether or not the specified addressable element is equal in value to the current addressable element</returns>
        private protected abstract bool Equals_m(ObjAddressable other);

        /// <summary>Gets a hashcode for the addressable element</summary>
        /// <returns>A hashcode for the addressable element</returns>
        private protected abstract int GetHashCode_m();
    }
}
