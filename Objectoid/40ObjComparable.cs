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
        #region helper

        /// <summary>Checks if the two elements have values that are equal</summary>
        /// <param name="a">Element A</param>
        /// <param name="b">Element B</param>
        /// <returns>Whether or not the two elements have values that are equal</returns>
        /// <exception cref="ArgumentNullException"><paramref name="a"/> is null
        /// <br/>or<br/><paramref name="b"/> is null</exception>
        private static bool Equals_m(TElement a, TElement b)
        {
            try
            {
                if (a.Value_p is null) return b.Value_p is null;
                return a.Value_p.Equals(b.Value_p);
            }
            catch when (a is null) { throw new ArgumentNullException(nameof(a)); }
            catch when (b is null) { throw new ArgumentNullException(nameof(b)); }
        }

        /// <summary>Compares Element A with Element B and determines whether or not 
        /// the Element A precedes, follows, or occurs in the same sorting position as Element B</summary>
        /// <param name="a">Element A</param>
        /// <param name="b">Element B</param>
        /// <returns>Less than zero: Element A precedes Element B
        /// <br/>Equal to zero: Element A occurs in the same position as Element B
        /// <br/>Greater than zero: Element A follows Element B</returns>
        /// <exception cref="ArgumentNullException"><paramref name="a"/> is null
        /// <br/>or<br/><paramref name="b"/> is null</exception>
        private static int Compare_m(TElement a, TElement b)
        {
            try
            {
                if (a.Value_p is null) return (b.Value_p is null) ? 0 : -1;
                return a.Value_p.CompareTo(b.Value_p);
            }
            catch when (a is null) { throw new ArgumentNullException(nameof(a)); }
            catch when (b is null) { throw new ArgumentNullException(nameof(b)); }
        }

        #endregion

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
            return Equals_m((TElement)this, (TElement)other);
        }

        /// <inheritdoc/>
        private protected override sealed int GetHashCode_m()
        {
            if (Value_p is null) return -1;
            return Value_p.GetHashCode();
        }

        #endregion

        #region ObjComparable

        /// <inheritdoc/>
        public override sealed object Value => Value_p;

        /// <inheritdoc/>
        internal override sealed int CompareTo_m(ObjComparable other)
        {
            return Compare_m((TElement)this, (TElement)other);
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjComparable{TElement,TValue}"/></summary>
        /// <param name="type">Data type</param>
        private protected ObjComparable(ObjType type) : base(type) { }

        /// <inheritdoc cref="ObjComparable.Value"/>
        private protected TValue Value_p { get; set; }

        /// <summary>Writes the element using the specified writer
        /// <br/>NOTE: It is assumed <paramref name="objWriter"/> is not null</summary>
        /// <param name="objWriter">Writer</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        private protected abstract void Write__m(ObjWriter objWriter);
    }
}
