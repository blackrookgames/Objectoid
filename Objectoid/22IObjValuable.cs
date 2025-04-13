using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid
{
    /// <summary>Interface for elements that contain a value</summary>
    public interface IObjValuable
    {
        /// <summary>Value</summary>
        object Value { get; }
    }

    /// <summary>Generic derivative of <see cref="IObjValuable"/></summary>
    ///  <typeparam name="TValue">Value</typeparam>
    public interface IObjValuable<TValue>
    {
        /// <summary>Value</summary>
        TValue Value { get; }
    }
}
