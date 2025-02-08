using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Base class for representing a named item within a collection of named items</summary>
    /// <typeparam name="TName">Data type of item name</typeparam>
    public interface INamedCollectionItem<TName>
    {
        /// <summary>Name of the item</summary>
        /// <remarks>
        /// It is assumed<br/>
        /// <see cref="Name"/> is not null<br/>
        /// Value of <see cref="Name"/> does not change
        /// </remarks>
        TName Name { get; }
    }
}
