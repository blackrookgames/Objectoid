using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Objectoid
{
    /// <summary>Represents a list inside an Objectoid document</summary>
    public class ObjList : ObjCollection, IEnumerable<ObjElement>
    {
        #region ObjCollection

        /// <inheritdoc/>
        private protected override void Write__m(ObjWriter objWriter)
        {
            //Property count
            objWriter.WriteInt32(Count);
            //Properties
            foreach (ObjElement element in _Elements)
                WriteElement_m(objWriter, element);
        }

        /// <inheritdoc/>
        private protected override void Read__m(ObjReader objReader)
        {
            var prev = _Elements;
            _Elements = new List<ObjElement>();
            NewState_m();
            try
            {
                //Element count
                int elementCount = objReader.ReadInt32();
                //Elements
                for (int i = 0; i < elementCount; i++)
                {
                    //Element
                    ObjElement element = ReadElement_m(objReader);
                    //Add element
                    _Elements.Add(element);
                    Add_m(element);
                }
            }
            catch
            {
                _Elements = prev;
                RevertState_m();
                throw;
            }
            ForgetState_m();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => _Elements.GetEnumerator();

        /// <summary>Gets an enumerator for the object's properties</summary>
        /// <returns>An enumerator for the object's properties</returns>
        public IEnumerator<ObjElement> GetEnumerator() => _Elements.GetEnumerator();

        #endregion

        /// <summary>Creates an instance of <see cref="ObjList"/> with a default initial capacity</summary>
        public ObjList() : base(ObjType.List, true)
        {
            _Elements = new List<ObjElement>();
        }

        /// <summary>Creates an instance of <see cref="ObjList"/> with the specified initial capacity</summary>
        /// <param name="capacity">Initial capacity</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero</exception>
        public ObjList(int capacity) : base(ObjType.List, true)
        {
            try
            {
                _Elements = new List<ObjElement>(capacity);
            }
            catch when (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than or equal to zero.");
            }
        }

        private List<ObjElement> _Elements; //Do NOT make readonly

        /// <summary>Gets the element at the specified index</summary>
        /// <param name="index">Index of the element</param>
        /// <returns>The element at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public ObjElement this[int index]
        {
            get
            {
                try
                {
                    return _Elements[index];
                }
                catch when (index < 0 || index >= _Elements.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <summary>Adds an element</summary>
        /// <param name="element">Element</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="element"/> cannot be part of a collection
        /// <br/>or
        /// <br/><paramref name="element"/> is already part of a collection</exception>
        public void Add(ObjElement element)
        {
            try
            {
                //Add to elements first
                Add_m(element);
                //Then add to list
                _Elements.Add(element);
            }
            catch (ArgNullException)
            {
                throw new ArgumentNullException(nameof(element));
            }
            catch (ArgException e)
            {
                throw new ArgumentException(e.MainMessage, nameof(element));
            }
        }

        /// <summary>Inserts an element at the specified index</summary>
        /// <param name="index">Index to insert the element</param>
        /// <param name="element">Element</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="element"/> cannot be part of a collection
        /// <br/>or
        /// <br/><paramref name="element"/> is already part of a collection</exception>
        public void Insert(int index, ObjElement element)
        {
            try
            {
                if (index < 0 || index > _Elements.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                //Add to elements first
                Add_m(element);
                //Then insert into list
                _Elements.Insert(index, element);
            }
            catch (ArgNullException)
            {
                throw new ArgumentNullException(nameof(element));
            }
            catch (ArgException e)
            {
                throw new ArgumentException(e.MainMessage, nameof(element));
            }
        }

        /// <summary>Attempts to remove the specified element</summary>
        /// <param name="element">Element</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        public bool Remove(ObjElement element)
        {
            try
            {
                //Remove from elements
                if (!Remove_m(element)) return false;
                //Remove from list
                _Elements.Remove(element);
                //Success
                return true;
            }
            catch when (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
        }

        /// <summary>Removes the element at the specified index</summary>
        /// <param name="index">Index of the element</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _Elements.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            //Find element
            ObjElement element = _Elements[index];
            //Remove from elements
            Remove_m(element);
            //Remove from list
            _Elements.RemoveAt(index);
        }

        /// <summary>Removes all elements</summary>
        public void Clear()
        {
            Clear_m();
            _Elements.Clear();
        }
    }
}
