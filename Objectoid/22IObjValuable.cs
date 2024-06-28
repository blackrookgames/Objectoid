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
}
