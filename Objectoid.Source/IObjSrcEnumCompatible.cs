using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal interface IObjSrcEnumCompatible
    {
        /// <summary>Value</summary>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not of a supported type</exception>
        object Value { get; set; }
    }

    internal interface IObjSrcEnumCompatible<T> : IObjSrcEnumCompatible
    {
        object IObjSrcEnumCompatible.Value
        {
            get => Value;
            set
            {
                try { Value = (T)value; }
                catch when (!(value is T)) { throw new ArgumentException($"Value is not an instance of {typeof(T).Name}.", nameof(value)); }
            }
        }

        /// <summary>Value</summary>
        new T Value { get; set; }
    }
}
