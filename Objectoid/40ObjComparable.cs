using System;
using System.IO;

namespace Objectoid
{
    /// <summary>Base class for addressable elements that can be compared to other addressable elements</summary>
    public abstract class ObjComparable : ObjAddressable, IComparable<ObjComparable>, IObjValuable
    {
        #region IComparable

        int IComparable<ObjComparable>.CompareTo(ObjComparable other)
        {
            if (other == null) return 1;
            if (other.Type != Type) return Type - other.Type;
            return CompareTo_m(other);
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjComparable"/></summary>
        /// <param name="type">Data type</param>
        private protected ObjComparable(ObjType type) : base(type, true) { }

        /// <inheritdoc/>
        public abstract object Value { get; }

        /// <summary>Compares the current element with a specified element and determines whether or not 
        /// the current element precedes, follows, or occurs in the same sorting position as the other element
        /// <br/>NOTE: It is assumed the other element is of the same type and is not null</summary>
        /// <param name="other">Other element</param>
        /// <returns>Less than zero: Current element precedes the other element
        /// <br/>Equal to zero: Current element occurs in the same position as the other element
        /// <br/>Greater than zero: Current element follows the other element</returns>
        internal abstract int CompareTo_m(ObjComparable other);
    }

    /// <summary>Generic derivative of <see cref="ObjComparable"/></summary>
    ///  <typeparam name="TElement">Element</typeparam>
    ///  <typeparam name="TValue">Value</typeparam>
    public abstract class ObjComparable<TElement, TValue> : ObjComparable
        where TElement : ObjComparable<TElement, TValue>
        where TValue : class, IEquatable<TValue>, IComparable<TValue>
    {
        #region ObjElement

        /// <inheritdoc/>
        internal override sealed void Write_m(ObjWriter objWriter) => Write__m(objWriter);

        #endregion

        #region ObjAddressable

        /// <inheritdoc/>
        private protected override sealed bool Equals_m(ObjAddressable other)
        {
            if (other is null) return false;
            if (!(other is TElement)) return false;
            return Value_p.Equals(((TElement)other).Value_p);
        }

        /// <inheritdoc/>
        private protected override sealed int GetHashCode_m()
        {
            return Value_p.GetHashCode();
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        public override sealed object Value => Value_p;

        /// <inheritdoc/>
        internal override sealed int CompareTo_m(ObjComparable other)
        {
            return Value_p.CompareTo(((TElement)other).Value_p);
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjComparable{TElement,TValue}"/></summary>
        /// <param name="type">Data type</param>
        private protected ObjComparable(ObjType type) : base(type) { }

        /// <inheritdoc cref="ObjComparable.Value"/>
        private protected TValue Value_p { get; set; }

        /// <summary>Writes the element using the specified writer
        /// <br/>NOTE: It is assumed <paramref name="objWriter"/> is not null
        /// <br/>NOTE: It is assumed <see cref="Value_p"/> is not null</summary>
        /// <param name="objWriter">Writer</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        private protected abstract void Write__m(ObjWriter objWriter);
    }
}
