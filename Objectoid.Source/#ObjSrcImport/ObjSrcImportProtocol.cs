using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Base class for representing an import protocol</summary>
    public abstract class ObjSrcImportProtocol : INamedCollectionItem<string>
    {
        #region helper

        /// <summary>
        /// Validates the arguments for <see cref="Import"/><br/>
        /// If any argument is invalid, an excpetion is thrown
        /// </summary>
        /// <param name="encodedProperties">Encoding results of properties within an instance of <see cref="ObjSrcImport"/></param>
        /// <param name="options">Import options</param>
        /// <returns>True if no exception is thrown</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encodedProperties"/> is null
        /// <br/>or<br/>
        /// <paramref name="options"/> is null
        /// </exception>
        /// 
        protected static bool ValidateImportArgs_m(ObjSrcImportEncodedPropertyCollection encodedProperties, IObjSrcImportOptions options)
        {
            if (encodedProperties is null) throw new ArgumentNullException(nameof(encodedProperties));
            if (options is null) throw new ArgumentNullException(nameof(options));
            return true;
        }

        #endregion

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
        /// <exception cref="ObjSrcException">
        /// An import source contains invalid data
        /// </exception>
        /// 
        public abstract ObjElement Import(ObjSrcImportEncodedPropertyCollection encodedProperties, IObjSrcImportOptions options);
    }
}
