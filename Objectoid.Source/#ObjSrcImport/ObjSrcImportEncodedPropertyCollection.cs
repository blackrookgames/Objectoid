using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of encoding results of properties within an instance of <see cref="ObjSrcImport"/></summary>
    public class ObjSrcImportEncodedPropertyCollection : NamedCollectionReadonly<ObjNTString, ObjSrcImportEncodedProperty>
    {
        /// <summary>Constructor for <see cref="ObjSrcImportEncodedPropertyCollection"/></summary>
        /// <param name="source">Import source</param>
        /// <param name="options">Import options</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null
        /// <br/>or<br/>
        /// <paramref name="options"/> is null
        /// </exception>
        /// 
        /// <exception cref="ObjSrcException">A child import source contains invalid data</exception>
        /// 
        internal ObjSrcImportEncodedPropertyCollection(ObjSrcImport source, IObjSrcImportOptions options)
        {
            try
            {
                Source = source;
                foreach (var srcProperty in source)
                {
                    try
                    {
                        var property = new ObjSrcImportEncodedProperty(
                            srcProperty.Name,
                            srcProperty.Value,
                            srcProperty.Value.CreateElement_m(options));
                        Add_m(property);
                    }
                    catch (ObjSrcSrcElementException e)
                    {
                        var path = ObjSrcElementPath.Create(ObjSrcElementPath.Create(Source), e.Path); 
                        throw new ObjSrcSrcElementException(Source, path, e.BaseMessage_p);
                    }
                    catch (ObjSrcException e)
                    {
                        throw new ObjSrcSrcElementException(Source, e.BaseMessage_p);
                    }
                }
            }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
            catch when (options is null) { throw new ArgumentNullException(nameof(options)); }
        }

        /// <summary>Import source</summary>
        public ObjSrcImport Source { get; }
    }
}
