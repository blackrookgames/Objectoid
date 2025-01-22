using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid object source</summary>
    [ObjSrcValidElement(ObjSrcKeyword._List)]
    public class ObjSrcList : ObjSrcCollection, IEnumerable<ObjSrcElement>
    {
        #region IEnumerable

        /// <summary>Gets an enumerator thru the list's elements</summary>
        /// <returns>An enumerator thru the list's elements</returns>
        public new IEnumerator<ObjSrcElement> GetEnumerator() => _Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region ObjSrcCollection

        /// <inheritdoc/>
        private protected sealed override IEnumerator<ObjSrcElement> GetEnumerator_m() => _Elements.GetEnumerator();

        /// <inheritdoc/>
        public sealed override int Count => _Elements.Count;

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcList"/></summary>
        public ObjSrcList()
        {
            _Elements = new List<ObjSrcElement>();
        }

        /// <inheritdoc/>
        internal sealed override void Load_m(ObjSrcReader reader)
        {
            try
            {
                Clear();

                //Ensure there's only whitespace after @List
                reader.Read();
                reader.Token.ThrowIfNotEOL_m();

                //Look for entries
                while (true)
                {
                    reader.Read();
                    if (reader.Token.Type == ObjSrcReaderTokenType.None) continue;
                    if (reader.Token.Type == ObjSrcReaderTokenType.Keyword)
                    {
                        if (reader.Token.Text == ObjSrcKeyword._EndList)
                        {
                            //Ensure only whitespace follows
                            reader.Read();
                            reader.Token.ThrowIfNotEOL_m();
                            //Break
                            break;
                        }
                        reader.DontAdvance();
                        Add(LoadElement_m(reader));
                        continue;
                    }
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                }
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal sealed override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.WriteLine(ObjSrcKeyword._List);
                writer.IncIndent();

                foreach (var element in _Elements)
                    element.Save_m(writer);

                writer.DecIndent();
                writer.WriteLine(ObjSrcKeyword._EndList);
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        private readonly List<ObjSrcElement> _Elements;

        /// <summary>Gets the element at the specified index</summary>
        /// <param name="index">Index of the element</param>
        /// <returns>The element at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public ObjSrcElement this[int index]
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

        /// <summary>Adds the specified element to the list</summary>
        /// <param name="element">Element to add</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="element"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="element"/> refers to an element that cannot be part of a collection
        /// <br/>or<br/>
        /// <paramref name="element"/> refers to an element that is already part of a collection
        /// </exception>
        /// 
        public void Add(ObjSrcElement element)
        {
            try
            {
                try { element.AddToCollection_m(this); }
                catch (NotSupportedException) { H_ThrowArgumentNotCollectible_m(nameof(element)); }
                catch (InvalidOperationException) { H_ThrowArgumentPartOfCollection_m(nameof(element)); }

                _Elements.Add(element);
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        /// <summary>Inserts the specified element into the list at the specified index</summary>
        /// <param name="index">Index to insert element</param>
        /// <param name="element">Element to add</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="element"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="element"/> refers to an element that cannot be part of a collection
        /// <br/>or<br/>
        /// <paramref name="element"/> refers to an element that is already part of a collection
        /// </exception>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is out of range
        /// </exception>
        /// 
        public void Insert(int index, ObjSrcElement element)
        {
            try
            {
                try { element.AddToCollection_m(this); }
                catch (NotSupportedException) { H_ThrowArgumentNotCollectible_m(nameof(element)); }
                catch (InvalidOperationException) { H_ThrowArgumentPartOfCollection_m(nameof(element)); }

                try { _Elements.Insert(index, element); }
                catch { element.RemoveFromCollection_m(); throw; }
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
            catch when (index < 0 || index > _Elements.Count) { throw new ArgumentOutOfRangeException(nameof(index)); }
        }

        /// <summary>Attempts to remove the specified element from the list</summary>
        /// <param name="element">Element to remove</param>
        /// <returns>Whether or not successful</returns>
        public bool Remove(ObjSrcElement element)
        {
            var index = _Elements.IndexOf(element);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>Removes the element at the specified index from the list</summary>
        /// <param name="index">Index of element</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public void RemoveAt(int index)
        {
            try
            {
                var element = _Elements[index];
                element.RemoveFromCollection_m();
                _Elements.RemoveAt(index);
            }
            catch when (index < 0 || index >= _Elements.Count) { throw new ArgumentOutOfRangeException(nameof(index)); }
        }

        /// <summary>Removes all elements from the list</summary>
        public void Clear()
        {
            for (int i = _Elements.Count - 1; i >= 0; i--)
                RemoveAt(i);
        }
    }
}
