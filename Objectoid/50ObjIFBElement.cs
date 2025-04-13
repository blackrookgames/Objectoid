using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid
{
    /// <summary>Base class for elements the represent values such as integers, floats, booleans, etc</summary>
    public abstract class ObjIFBElement : ObjElement, IObjValuable
    {
        /// <summary>Constructor for <see cref="ObjIFBElement"/></summary>
        /// <param name="type">Data type</param>
        private protected ObjIFBElement(ObjType type) : base(type, true) { }

        /// <inheritdoc/>
        public abstract object Value { get; }
    }

    /// <summary>Generic derivative of <see cref="ObjIFBElement"/></summary>
    ///  <typeparam name="TValue">Value</typeparam>
    public abstract class ObjIFBElement<TValue> : ObjIFBElement, IObjValuable<TValue>
    {
        #region ObjIFBElement

        /// <inheritdoc/>
        public override object Value => Value_p;

        #endregion

        #region IObjValuable

        TValue IObjValuable<TValue>.Value => Value_p;

        #endregion

        /// <summary>Constructor for <see cref="ObjIFBElement{TValue}"/></summary>
        /// <param name="type">Data type</param>
        private protected ObjIFBElement(ObjType type) : base(type) { }

        /// <inheritdoc cref="ObjComparable.Value"/>
        private protected TValue Value_p { get; set; }
    }
}
