using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Objectoid
{
    /// <summary>Represents an Objectoid document</summary>
    public class ObjDocument
    {
        /// <summary>Creates an instance of <see cref="ObjDocument"/></summary>
        public ObjDocument()
        {
            _RootObject = new ObjRootObject(this);
        }

        #region RootObject

        private readonly ObjRootObject _RootObject;

        /// <summary>The root object of the document</summary>
        public ObjRootObject RootObject => _RootObject;

        #endregion

        /// <summary>Saves the document to the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <param name="intIsLittleEndian">Whether or not integers are stored as little-endian</param>
        /// <param name="floatIsLittleEndian">Whether or not floats are stored as little-endian</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking
        /// <br/>or
        /// <br/><paramref name="stream"/> does not support writing</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="OverflowException">It was attempted to reference a stream position greater than <see cref="int.MaxValue"/></exception>
        public void Save(Stream stream, bool intIsLittleEndian, bool floatIsLittleEndian)
        {
            long returnPos;

            ObjWriter writer = new ObjWriter(stream, intIsLittleEndian, floatIsLittleEndian);

            //Flag byte
            byte flagByte = 0;
            if (intIsLittleEndian) flagByte |= 0b_0001;
            if (floatIsLittleEndian) flagByte |= 0b_0010;
            writer.WriteUInt8(flagByte);

            //Root object (placeholder)
            long mainObjRefPosition = stream.Position;
            writer.WriteInt32(0);

            //Find all collections, comparables, and property names
            HashSet<ObjCollection> collections = new HashSet<ObjCollection>(); //Collections will be ordered by dependency
            List<ObjComparable> comparables = new List<ObjComparable>();
            List<ObjNTString> propertyNames = new List<ObjNTString>();
            void addCollection(ObjCollection collection)
            {
                //First, loop thru the collection's elements
                foreach (ObjElement element in collection.Elements)
                {
                    //If element is collection
                    if (element is ObjCollection)
                    {
                        addCollection((ObjCollection)element);
                    }
                    //If element is comparable
                    else if (element is ObjComparable)
                    {
                        ObjComparable comparable = (ObjComparable)element;
                        //Find position to insert comparable
                        int index = -1;
                        bool dontInsert = false;
                        while ((++index) < comparables.Count)
                        {
                            int compare = comparable.CompareTo_m(comparables[index]);
                            if (compare > 0) continue;
                            if (compare == 0) dontInsert = true;
                            break;
                        }
                        //Insert comparable
                        if (!dontInsert) comparables.Insert(index, comparable);
                    }
                }
                //If collection is document-object
                if (collection is ObjDocObject)
                {
                    foreach (ObjDocObjectProperty property in ((ObjDocObject)collection))
                    {
                        ObjNTString propertyName = property.Name;
                        //Find position to insert property name
                        int index = -1;
                        bool dontInsert = false;
                        while ((++index) < propertyNames.Count)
                        {
                            int compare = propertyName.CompareTo(propertyNames[index]);
                            if (compare > 0) continue;
                            if (compare == 0) dontInsert = true;
                            break;
                        }
                        //Insert property name
                        if (!dontInsert) propertyNames.Insert(index, propertyName);
                    }
                }
                //Add collection
                collections.Add(collection);
            }
            addCollection(_RootObject);

            //Write all property names
            foreach (ObjNTString propertyName in propertyNames)
            {
                writer.RegisterPropertyName(propertyName);
                foreach (byte c in propertyName)
                    writer.WriteUInt8(c);
                writer.WriteUInt8(0);
            }

            //Write all comparables
            foreach (ObjComparable comparable in comparables)
            {
                writer.RegisterAddressable(comparable);
                comparable.Write_m(writer);
            }

            //Write all collections
            foreach (ObjCollection collection in collections)
            {
                writer.RegisterAddressable(collection);
                collection.Write_m(writer);
            }

            //Fix Root object placeholder
            returnPos = stream.Position;
            stream.Position = mainObjRefPosition;
            writer.WriteAddressable(_RootObject);
            stream.Position = returnPos;
        }

        /// <summary>Loads the document from the specified stream</summary>
        /// <param name="stream">Stream</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking
        /// <br/>or
        /// <br/><paramref name="stream"/> does not support reading</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        /// <exception cref="InvalidDataException">Stream contains invalid data</exception>
        public void Load(Stream stream)
        {
            ObjReader reader = new ObjReader(stream);

            //Flag byte
            byte flagByte = reader.ReadUInt8();
            reader.SetIntIsLittleEndian((flagByte & 0b_0001) != 0);
            reader.SetFloatIsLittleEndian((flagByte & 0b_0010) != 0);

            //Root object
            int firstAddress = reader.ReadAddress();
            
            //Objects
            reader.Stream.Position = firstAddress;
            _RootObject.Read_m(reader);
        }
    }
}
