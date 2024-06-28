using System;
using System.IO;

namespace Objectoid
{
    /// <summary>Base class for all Objectoid elements</summary>
    public abstract class ObjElement
    {
        /// <summary>Constructor for <see cref="ObjElement"/></summary>
        /// <param name="type">Data type</param>
        /// <param name="isCollectable">Whether or not the element can be part of a collection</param>
        private protected ObjElement(ObjType type, bool isCollectable)
        {
            _Type = type;
            _IsCollectable = isCollectable;
        }

        #region Type

        private readonly ObjType _Type;
        
        /// <summary>Data type</summary>
        public ObjType Type => _Type;

        #endregion

        #region IsCollectable

        private readonly bool _IsCollectable;

        /// <summary>Whether or not the element can be part of a collection</summary>
        public bool IsCollectable => _IsCollectable;

        #endregion

        #region Collection

        private ObjCollection fCollection;

        /// <summary>Collection that contains the element</summary>
        public ObjCollection Collection => fCollection;

        /// <summary>Sets the collection of the element
        /// <br/>CALLED BY: <see cref="ObjCollection"/></summary>
        /// <param name="collection">Collection of the element</param>
        internal void SetCollection_m(ObjCollection collection)
        {
            fCollection = collection;
        }

        #endregion

        /// <summary>Writes the element using the specified writer
        /// <br/>NOTE: It is assumed <paramref name="objWriter"/> is not null</summary>
        /// <param name="objWriter">Writer</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        internal abstract void Write_m(ObjWriter objWriter);

        /// <summary>Reads element data using the specified reader
        /// <br/>NOTE: It is assumed <paramref name="objReader"/> is not null</summary>
        /// <param name="objReader">Reader</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        /// <exception cref="InvalidDataException">Stream contains invalid data</exception>
        internal abstract void Read_m(ObjReader objReader);
    }
}
