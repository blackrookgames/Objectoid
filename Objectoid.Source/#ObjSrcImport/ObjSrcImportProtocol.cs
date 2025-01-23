using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Base class for representing an import protocol</summary>
    public abstract class ObjSrcImportProtocol
    {
        /// <summary>Constructor for <see cref="ObjSrcImportProtocol"/></summary>
        /// <param name="name">Name of the protocol</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        protected ObjSrcImportProtocol(string name)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        /// <summary>Name of the protocol</summary>
        public string Name { get; }

        /// <summary>Creates an objectoid using the specified encoding results of properties within an instance of <see cref="ObjSrcImport"/></summary>
        /// <param name="encodedProperties">Encoding results of properties within an instance of <see cref="ObjSrcImport"/></param>
        /// <param name="options">Import options</param>
        /// <returns>Created objectoid element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encodedProperties"/> is null
        /// <br/>or<br/>
        /// <paramref name="options"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> is not of a supported type
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// An import source contains invalid data
        /// </exception>
        /// 
        public abstract ObjElement Import(ObjSrcImportEncodedPropertyCollection encodedProperties, IObjSrcImportOptions options);
    }

    /// <summary>Generic base class for representing an import protocol</summary>
    /// <typeparam name="TOptions">Option type</typeparam>
    /// <typeparam name="TProtocol">Protocol base type</typeparam>
    public abstract class ObjSrcImportProtocol<TOptions, TProtocol> : ObjSrcImportProtocol
        where TOptions : IObjSrcImportOptions<TOptions, TProtocol>
        where TProtocol : ObjSrcImportProtocol<TOptions, TProtocol>
    {
        #region ObjSrcImportProtocol

        /// <inheritdoc/>
        public sealed override ObjElement Import(ObjSrcImportEncodedPropertyCollection encodedProperties, IObjSrcImportOptions options)
        {
            try { return Import(encodedProperties, (TOptions)options); }
            catch when (options is null) { throw new ArgumentNullException(nameof(options)); }
            catch when (!(options is TOptions)) { throw new ArgumentException($"The specified options is not an instance of {typeof(TOptions).Name}.", nameof(options)); }
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjSrcImportProtocol{TOptions, TProtocol}"/></summary>
        /// <param name="name">Name of the protocol</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        protected ObjSrcImportProtocol(string name) : base(name) { }

        /// <summary>Creates an objectoid using the specified encoding results of properties within an instance of <see cref="ObjSrcImport"/></summary>
        /// <param name="encodedProperties">Encoding results of properties within an instance of <see cref="ObjSrcImport"/></param>
        /// <param name="options">Import options</param>
        /// <returns>Created objectoid element</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encodedProperties"/> is null
        /// <br/>or<br/>
        /// <paramref name="options"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">
        /// An import source contains invalid data
        /// </exception>
        /// 
        public abstract ObjElement Import(ObjSrcImportEncodedPropertyCollection encodedProperties, TOptions options);
    }
}
