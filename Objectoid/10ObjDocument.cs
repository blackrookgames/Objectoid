using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

        #region Header

        private void ResetHeader_m()
        {
            Identifier = null;
        }

        private void SaveHeader_m(ObjWriter writer)
        {
            void fixOffset(long pos)
            {
                var offset = writer.Stream.Position;
                writer.Stream.Position = pos;
                writer.WriteInt32((int)offset);
                writer.Stream.Position = offset;
            }
            void fixLength(long pos)
            {
                var offset = writer.Stream.Position;
                writer.Stream.Position = pos;
                writer.WriteUInt16((ushort)(offset - pos));
                writer.Stream.Position = offset;
            }

            try
            {
                var pos_HeaderLength = writer.Stream.Position;
                writer.WriteUInt16(0);

                #region write header

                //Identifier
                var pos_Identifier = writer.Stream.Position;
                writer.WriteInt32(0);

                #endregion

                fixLength(pos_HeaderLength);

                #region fix header

                //Identifier
                fixOffset(pos_Identifier);
                RWUtility.WriteString(writer, Identifier);

                #endregion
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        private void LoadHeader_m(ObjReader reader)
        {
            try
            {
                ResetHeader_m();

                var headerStart = reader.Stream.Position;
                var headerLength = reader.ReadUInt16();
                var headerEnd = headerStart + headerLength;
                bool endOfHeader() //Returns true if the end of the header was surpassed
                {
                    if (reader.Stream.Position <= headerEnd)
                        return false;
                    reader.Stream.Position = headerEnd;
                    return true;
                }

                long returnPos = 0;
                bool readOffset() //Returns true if the end of the header was surpassed
                {
                    var offset = reader.ReadInt32();
                    if (endOfHeader()) return true;
                    returnPos = reader.Stream.Position;
                    reader.Stream.Position = offset;
                    return false;
                }

                #region read header

                //Identifier
                if (readOffset()) return;
                Identifier = RWUtility.ReadString(reader);
                reader.Stream.Position = returnPos;

                #endregion
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <summary>Identifier for the document</summary>
        public string Identifier { get; set; }

        #endregion

        #region RootObject

        private readonly ObjRootObject _RootObject;

        /// <summary>The root object of the document</summary>
        public ObjRootObject RootObject => _RootObject;

        #endregion

        private const byte _FB_IntLilEn = 0b_0000_0001;
        private const byte _FB_FloatLilEn = 0b_0000_0010;
        private const byte _FB_HasHeader = 0b_0000_0100;

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
            if (intIsLittleEndian) flagByte |= _FB_IntLilEn;
            if (floatIsLittleEndian) flagByte |= _FB_FloatLilEn;
            flagByte |= _FB_HasHeader;
            writer.WriteUInt8(flagByte);

            //Root object (placeholder)
            long mainObjRefPosition = stream.Position;
            writer.WriteInt32(0);

            //Header
            SaveHeader_m(writer);

            //Find all addressables and property names
            HashSet<ObjCollection> collections = new HashSet<ObjCollection>(); //Collections will be ordered by dependency
            List<ObjComparable> comparables = new List<ObjComparable>();
            List<ObjAddressable> miscAddressables = new List<ObjAddressable>();
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
                    //If element is addressable
                    else if (element is ObjAddressable)
                    {
                        miscAddressables.Add((ObjAddressable)element);
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

            //Write all misc addressables
            foreach (ObjAddressable addressable in miscAddressables)
            {
                writer.RegisterAddressable(addressable);
                addressable.Write_m(writer);
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
            reader.SetIntIsLittleEndian((flagByte & _FB_IntLilEn) != 0);
            reader.SetFloatIsLittleEndian((flagByte & _FB_FloatLilEn) != 0);
            var hasHeader = (flagByte & _FB_HasHeader) != 0;

            //Root object
            int firstAddress = reader.ReadAddress();

            //Header
            if (hasHeader) LoadHeader_m(reader);

            //Objects
            reader.Stream.Position = firstAddress;
            _RootObject.Read_m(reader);
        }
    }
}
