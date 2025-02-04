using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents a collection of encoding results of properties within an instance of <see cref="ObjSrcImport"/></summary>
    public class ObjSrcImportEncodedPropertyCollection : IEnumerable<ObjSrcImportEncodedProperty>
    {
        #region IEnumerable

        /// <summary>Gets an enumerator for the collection</summary>
        /// <returns>An enumerator for the collection</returns>
        public IEnumerator<ObjSrcImportEncodedProperty> GetEnumerator() =>
            _Properties.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

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
                _Source = source;
                _Properties = new Dictionary<ObjNTString, ObjSrcImportEncodedProperty>(source.Count);
                foreach (var srcProperty in source)
                {
                    try
                    {
                        var property = new ObjSrcImportEncodedProperty(
                            srcProperty.Name,
                            srcProperty.Value,
                            srcProperty.Value.CreateElement_m(options));
                        _Properties.Add(property.Name, property);
                    }
                    catch (ObjSrcSrcElementException e)
                    {
                        var path = ObjSrcElementPath.Create(ObjSrcElementPath.Create(_Source), e.Path); 
                        throw new ObjSrcSrcElementException(_Source, path, e.BaseMessage_p);
                    }
                    catch (ObjSrcException e)
                    {
                        throw new ObjSrcSrcElementException(_Source, e.BaseMessage_p);
                    }
                }
            }
            catch when (source is null) { throw new ArgumentNullException(nameof(source)); }
            catch when (options is null) { throw new ArgumentNullException(nameof(options)); }
        }

        private readonly ObjSrcImport _Source;
        private readonly Dictionary<ObjNTString, ObjSrcImportEncodedProperty> _Properties;

        /// <summary>Number of properties within the collection</summary>
        public int Count => _Properties.Count;

        /// <summary>Import source</summary>
        public ObjSrcImport Source => _Source;

        /// <summary>Attempts to get the property with the specified name</summary>
        /// <param name="name">Name of the property</param>
        /// <param name="property">Retrieved property</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool TryGet(ObjNTString name, out ObjSrcImportEncodedProperty property)
        {
            try { return _Properties.TryGetValue(name, out property); }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }
    }
}
