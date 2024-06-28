using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objectoid
{
    /// <summary>Base class for Objectoid collection elements</summary>
    public abstract class ObjCollection : ObjAddressable
    {
        #region ObjElement

        /// <inheritdoc/>
        internal override sealed void Write_m(ObjWriter objWriter) => Write__m(objWriter);

        /// <inheritdoc/>
        internal override sealed void Read_m(ObjReader objReader) => Read__m(objReader);

        #endregion

        #region ObjAddressableElement

        private protected override bool Equals_m(ObjAddressable other) => Equals(other);

        private protected override int GetHashCode_m() => GetHashCode();

        #endregion

        /// <summary>Constructor for <see cref="ObjCollection"/></summary>
        /// <param name="type">Data type</param>
        /// <param name="isCollectable">Whether or not the element can be part of a collection</param>
        private protected ObjCollection(ObjType type, bool isCollectable) : 
            base(type, isCollectable)
        {
            _Elements = new HashSet<ObjElement>();
        }

        #region Elements

        private HashSet<ObjElement> _Elements;
        private HashSet<ObjElement> _PrevElements;

        /// <summary>Elements in the collection</summary>
        public IEnumerable<ObjElement> Elements => _Elements;

        /// <summary>Creates a new elements state, storing the previous state in case an exception occurs</summary>
        private protected void NewState_m()
        {
            _PrevElements = _Elements;
            _Elements = new HashSet<ObjElement>();
        }

        /// <summary>Reverts back to the previous state, useful for catching exceptions
        /// <br/>NOTE: It is assumed the collection has a previous state stored
        /// <br/>NOTE: Once this method is called, the previous state is forgotten</summary>
        private protected void RevertState_m()
        {
            _Elements = _PrevElements;
            _PrevElements = null;
        }

        /// <summary>Forgets the previous state, hopefully freeing up memory</summary>
        private protected void ForgetState_m()
        {
            _PrevElements = null;
        }

        #endregion

        /// <summary>Number of elements in the collection</summary>
        public int Count => _Elements.Count;

        /// <summary>Adds the element to the collection</summary>
        /// <param name="element">Element</param>
        /// <exception cref="ArgNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ArgException"><paramref name="element"/> cannot be part of a collection
        /// <br/>or
        /// <br/><paramref name="element"/> is already part of a collection</exception>
        private protected void Add_m(ObjElement element)
        {
            try
            {
                if (!element.IsCollectable) throw new ArgException(
                    "The specified element cannot be part of a collection.", nameof(element));
                if (element.Collection != null) throw new ArgException(
                    "The specified element is already part of a collection.", nameof(element));
                element.SetCollection_m(this);
                _Elements.Add(element);
            }
            catch when (element == null)
            {
                throw new ArgNullException(nameof(element));
            }
        }

        /// <summary>Attempts to remove the specified element from the collection</summary>
        /// <param name="element">Element</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgNullException"><paramref name="element"/> is null</exception>
        private protected bool Remove_m(ObjElement element)
        {
            if (element == null) throw new ArgNullException(nameof(element));
            if (!_Elements.Remove(element)) return false;
            element.SetCollection_m(null);
            return true;
        }

        /// <summary>Removes all elements from the collection</summary>
        private protected void Clear_m()
        {
            foreach (ObjElement element in _Elements)
                element.SetCollection_m(null);
            _Elements.Clear();
        }

        /// <summary>Writes the specified element using the specified writer
        /// <br/>NOTE: It is assumed neither <paramref name="objWriter"/> nor <paramref name="objElement"/> are null
        /// <br/>NOTE: It is assumed the writer has already written the 
        /// collections, comparables, and property names the specified element depends on</summary>
        /// <param name="objWriter">Writer</param>
        /// <param name="objElement">Element</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        private protected void WriteElement_m(ObjWriter objWriter, ObjElement objElement)
        {
            //If element is collection
            if (objElement is ObjCollection)
            {
                ObjCollection collection = (ObjCollection)objElement;
                objWriter.WriteUInt8((byte)collection.Type);
                objWriter.WriteAddressable(collection);
            }
            //If element is comparable
            else if (objElement is ObjComparable)
            {
                ObjComparable comparable = (ObjComparable)objElement;
                if (comparable.Value is null)
                {
                    objWriter.WriteUInt8((byte)ObjType.Null);
                }
                else
                {
                    objWriter.WriteUInt8((byte)comparable.Type);
                    objWriter.WriteAddressable(comparable);
                }
            }
            //If element is integer, float, or boolean
            else if (objElement is ObjIFBElement)
            {
                ObjIFBElement ifbElement = (ObjIFBElement)objElement;
                objWriter.WriteUInt8((byte)ifbElement.Type);
                ifbElement.Write_m(objWriter);
            }
            //Assume element is null
            else
            {
                objWriter.WriteUInt8((byte)ObjType.Null);
            }
        }

        /// <summary>Reads the specified element using the specified reader
        /// <br/>NOTE: It is assumed <paramref name="objReader"/> is not null</summary>
        /// <param name="objReader">Reader</param>
        /// <returns>The element read</returns>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        private protected ObjElement ReadElement_m(ObjReader objReader)
        {
            //Type
            ObjType objType = (ObjType)objReader.ReadUInt8();
            //Create element
            ObjElement element;
            if (!objType.TryCreate(out element)) throw objReader.InvalidData(objReader.Stream.Position - 1,
                $"The type 0x{((byte)objType):X2} was unexpected.");
            //If element is addressable
            if (element is ObjAddressable)
            {
                long returnPos;
                //Read address
                int address = objReader.ReadAddress();
                //Goto address
                returnPos = objReader.Stream.Position;
                objReader.Stream.Position = address;
                //Read data
                element.Read_m(objReader);
                //Return
                objReader.Stream.Position = returnPos;
            }
            //Else
            else
            {
                //Read data
                element.Read_m(objReader);
            }
            //Return
            return element;
        }

        /// <summary>Writes the collection using the specified writer
        /// <br/>NOTE: It is assumed <paramref name="objWriter"/> is not null
        /// <br/>NOTE: It is assumed the writer has already written the 
        /// collections, comparables, and property names the current collections depends on</summary>
        /// <param name="objWriter">Writer</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        private protected abstract void Write__m(ObjWriter objWriter);

        /// <summary>Reads element data using the specified reader
        /// <br/>NOTE: It is assumed <paramref name="objReader"/> is not null
        /// <br/>NOTE: It is assumed the collection has a means of returning to its previous state should an exception occur</summary>
        /// <param name="objReader">Reader</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        /// <exception cref="InvalidDataException">Stream contains invalid data</exception>
        private protected abstract void Read__m(ObjReader objReader);
    }
}
