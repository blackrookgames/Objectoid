using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid object source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._Object)]
    public class ObjSrcObject : ObjSrcCollection, IEnumerable<ObjSrcObjectProperty>
    {
        #region helper

        /// <summary>Reads entry data from the specified objectoid-source reader</summary>
        /// <param name="reader">Objectoid-source reader</param>
        /// <param name="endKeyword">End keyword</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="reader"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <exception cref="ObjSrcException">A syntax error was found</exception>
        /// <exception cref="EndOfStreamException">Stream ends before all data is found</exception>
        private protected void ReadEntries_m(ObjSrcReader reader, string endKeyword)
        {
            try
            {
                while (true)
                {
                    reader.Read();
                    if (reader.Token.Type == ObjSrcReaderTokenType.None) continue;
                    if (reader.Token.Type == ObjSrcReaderTokenType.String)
                    {
                        //Name
                        if (!ObjNTString.TryParse(reader.Token.Text, out var propertyName))
                            ObjSrcException.ThrowSyntaxError_m($"\"{reader.Token.Text}\" is not a valid property name.", reader.Token);
                        if (_Properties.ContainsKey(propertyName))
                            ObjSrcException.ThrowSyntaxError_m($"Object already contains a property with the name \"{propertyName}\".", reader.Token);
                        //Value
                        ObjSrcElement propertyValue = LoadElement_m(reader);
                        //Add
                        Add(propertyName, propertyValue);
                        //Next
                        continue;
                    }
                    if (reader.Token.Type == ObjSrcReaderTokenType.Keyword)
                    {
                        if (reader.Token.Text != endKeyword)
                            ObjSrcException.ThrowUnexpectedKeyword_m(reader.Token);
                        //Ensure only whitespace follows
                        reader.Read();
                        reader.Token.ThrowIfNotEOL_m();
                        //Break
                        break;
                    }
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                }
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <summary>Writes entry data to the specified objectoid-source writer</summary>
        /// <param name="writer">Objectoid-source writer</param>
        /// <param name="endKeyword">End keyword</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="writer"/> has already been disposed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        private protected void WriteEntries_m(ObjSrcWriter writer, string endKeyword)
        {
            try
            {
                writer.IncIndent();

                foreach (var property in _Properties.Values)
                {
                    //Name
                    WriteStringToken(writer, property.Name.ToString());
                    writer.Write(' ');
                    //Value
                    property.Value.Save_m(writer);
                }

                writer.DecIndent();
                writer.WriteLine(ObjSrcKeyword._EndObject);
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator thru the object element's properties</summary>
        /// <returns>An enumerator thru the object element's properties</returns>
        public new IEnumerator<ObjSrcObjectProperty> GetEnumerator() => _Properties.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region ObjSrcCollection

        /// <inheritdoc/>
        private protected sealed override IEnumerator<ObjSrcElement> GetEnumerator_m()
        {
            foreach (ObjSrcObjectProperty property in _Properties.Values)
                yield return property.Value;
        }

        /// <inheritdoc/>
        public sealed override int Count => _Properties.Count;

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcObject"/></summary>
        public ObjSrcObject()
        {
            _Properties = new Dictionary<ObjNTString, ObjSrcObjectProperty>();
        }

        /// <inheritdoc/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                Clear();

                //Ensure there's only whitespace after @Object
                reader.Read();
                reader.Token.ThrowIfNotEOL_m();

                //Look for entries
                ReadEntries_m(reader, ObjSrcKeyword._EndObject);
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.WriteLine(ObjSrcKeyword._Object);
                WriteEntries_m(writer, ObjSrcKeyword._EndObject);
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        private readonly Dictionary<ObjNTString, ObjSrcObjectProperty> _Properties;

        /// <summary>Attempts to get the element value of the property with the specified name</summary>
        /// <param name="name">Name of property</param>
        /// <param name="value">Retrieved element value</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool TryGetValue(ObjNTString name, out ObjSrcElement value)
        {
            try
            {
                if (_Properties.TryGetValue(name, out var property))
                {
                    value = property.Value;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <summary>Adds a property with the specified name and element value to the object</summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Element value of the property</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="value"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// Object already contains a property with the same name as <paramref name="name"/>
        /// <br/>or<br/>
        /// <paramref name="value"/> refers to an element that cannot be part of a collection
        /// <br/>or<br/>
        /// <paramref name="value"/> refers to an element that is already part of a collection
        /// </exception>
        /// 
        public void Add(ObjNTString name, ObjSrcElement value)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (value is null) throw new ArgumentNullException(nameof(value));

            try
            {
                var property = new ObjSrcObjectProperty(name, value);
                try { _Properties.Add(name, property); }
                catch (ArgumentException) { throw new ArgumentException("Object already contains a property with the same name.", nameof(name)); }

                try { value.AddToCollection_m(Collection); }
                catch { _Properties.Remove(name); throw; }
            }
            catch (NotSupportedException) { H_ThrowArgumentNotCollectible_m(nameof(value)); }
            catch (InvalidOperationException) { H_ThrowArgumentPartOfCollection_m(nameof(value)); }
        }

        /// <summary>Attempts to remove the property with the specified value from the object</summary>
        /// <param name="name">Name of the property</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool Remove(ObjNTString name)
        {
            try
            {
                if (!_Properties.TryGetValue(name, out var property)) return false;
                property.Value.RemoveFromCollection_m();
                _Properties.Remove(name);
                return true;
            }
            catch when (name is null) { throw new ArgumentNullException(nameof(name)); }
        }

        /// <summary>Removes all properties from the object</summary>
        public void Clear()
        {
            foreach (var property in _Properties.Values)
                property.Value.RemoveFromCollection_m();
            _Properties.Clear();
        }
    }
}
