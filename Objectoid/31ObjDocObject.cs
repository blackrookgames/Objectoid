using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Objectoid
{
    /// <summary>Represents a property of an object inside an Objectoid document</summary>
    public struct ObjDocObjectProperty
    {
        /// <summary>Constructor for <see cref="ObjDocObjectProperty"/>
        /// <br/>NOTE: It is assumed neither <paramref name="name"/> nor <paramref name="value"/> are null</summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        internal ObjDocObjectProperty(ObjNTString name, ObjElement value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>Property name</summary>
        public ObjNTString Name { get; }

        /// <summary>Property value</summary>
        public ObjElement Value { get; }
    }

    /// <summary>Represents an object inside an Objectoid document</summary>
    public class ObjDocObject : ObjCollection, IEnumerable<ObjDocObjectProperty>
    {
        #region ObjCollection

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            //Property count
            objWriter.WriteInt32(Count);
            //Properties
            foreach (ObjDocObjectProperty property in _Properties.Values)
            {
                //Name
                objWriter.WritePropertyName(property.Name);
                //Element
                WriteElement_m(objWriter, property.Value);
            }
        }

        /// <inheritdoc/>
        private protected override void Read__m(ObjReader objReader)
        {
            var prev = _Properties;
            _Properties = new Dictionary<ObjNTString, ObjDocObjectProperty>();
            NewState_m();
            try
            {
                //Property count
                int propertyCount = objReader.ReadInt32();
                //Properties
                for (int i = 0; i < propertyCount; i++)
                {
                    long returnPos;
                    //Name
                    int nameAddress = objReader.ReadAddress();
                    returnPos = objReader.Stream.Position;
                    objReader.Stream.Position = nameAddress;
                    ObjNTString name = objReader.ReadPropertyName();
                    objReader.Stream.Position = returnPos;
                    //Element
                    ObjElement element = ReadElement_m(objReader);
                    //Add property
                    _Properties.Add(name, new ObjDocObjectProperty(name, element));
                    Add_m(element);
                }
            }
            catch
            {
                _Properties = prev;
                RevertState_m();
                throw;
            }
            ForgetState_m();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => _Properties.Values.GetEnumerator();

        /// <summary>Gets an enumerator for the object's properties</summary>
        /// <returns>An enumerator for the object's properties</returns>
        public IEnumerator<ObjDocObjectProperty> GetEnumerator() => _Properties.Values.GetEnumerator();

        #endregion

        /// <summary>Constructor for <see cref="ObjDocObject"/></summary>
        /// <param name="isRoot">Whether or not the object is the root object of an Objectoid document</param>
        private protected ObjDocObject(bool isRoot) :
            base(ObjType.DocObject, !isRoot)
        {
            _Properties = new Dictionary<ObjNTString, ObjDocObjectProperty>();
            _IsRoot = isRoot;
        }

        /// <summary>Creates an instance of <see cref="ObjDocObject"/></summary>
        public ObjDocObject() : this(false)
        { }

        #region IsRoot

        private readonly bool _IsRoot;

        /// <summary>Whether or not the object is the root object of an Objectoid document</summary>
        public bool IsRoot => _IsRoot;

        #endregion

        private Dictionary<ObjNTString, ObjDocObjectProperty> _Properties; //Do NOT make readonly

        /// <summary>Attempts to get the value of the property with the specified name</summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the found property</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool TryGetValue(ObjNTString name, out ObjElement value)
        {
            try
            {
                if (_Properties.TryGetValue(name, out ObjDocObjectProperty property))
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
            catch when (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>Adds a property</summary>
        /// <param name="name">Name of property</param>
        /// <param name="value">Value of property</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null
        /// <br/>or
        /// <br/><paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException">Object already contains a property with the specified name
        /// <br/>or
        /// <paramref name="value"/> cannot be part of a collection
        /// <br/>or
        /// <br/><br/><paramref name="value"/> is already part of a collection</exception>
        public void Add(ObjNTString name, ObjElement value)
        {
            try
            {
                //Ensure name is not null
                if (name == null) throw new ArgumentException(nameof(name));
                //Ensure name is unique
                if (_Properties.ContainsKey(name)) throw new ArgumentException(
                    "The object already contains a property with the specified name.", nameof(name));
                //Add to elements first
                Add_m(value);
                //Then add to dictionary
                _Properties.Add(name, new ObjDocObjectProperty(name, value));
            }
            catch (ArgNullException)
            {
                throw new ArgumentNullException(nameof(value));
            }
            catch (ArgException e)
            {
                throw new ArgumentException(e.MainMessage, nameof(value));
            }
        }

        /// <summary>Attempts to remove the property with the specified name</summary>
        /// <param name="name">Name of the property</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public bool Remove(ObjNTString name)
        {
            try
            {
                //Find property
                if (!_Properties.TryGetValue(name, out ObjDocObjectProperty property)) return false;
                //Remove from elements
                Remove_m(property.Value);
                //Remove from dictionary
                _Properties.Remove(name);
                //Success
                return true;
            }
            catch when (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>Removes all properties</summary>
        public void Clear()
        {
            Clear_m();
            _Properties.Clear();
        }
    }
}
