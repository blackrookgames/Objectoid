using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid element source</summary>
    public abstract class ObjSrcElement : IObjSrcLoadSave
    {
        #region helper

        /// <summary>Throws an <see cref="NotSupportedException"/> explaining the element type cannot be part of a collection</summary>
        /// <exception cref="NotSupportedException">Expected outcome</exception>
        private protected NotSupportedException ThrowNotCollectible_m() =>
            throw new NotSupportedException($"{GetType().Name} cannot be part of a collection.");

        #endregion

        #region IObjSrcLoadSave

        void IObjSrcLoadSave.Load(ObjSrcReader reader) => Load_m(reader);

        void IObjSrcLoadSave.Save(ObjSrcWriter writer) => Save_m(writer);

        #endregion

        /// <inheritdoc cref="IObjSrcLoadSave.Load"/>
        internal abstract void Load_m(ObjSrcReader reader);

        /// <inheritdoc cref="IObjSrcLoadSave.Save"/>
        internal abstract void Save_m(ObjSrcWriter writer);

        /// <summary>Creates an objectoid element</summary>
        /// <param name="options">Import options</param>
        /// <returns>Created objectoid element</returns>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null</exception>
        /// <exception cref="ObjSrcException">An import source contains invalid data</exception>
        internal abstract ObjElement CreateElement_m(IObjSrcImportOptions options);

        /// <summary>Whether or not the element can be a part of a collection</summary>
        public virtual bool IsCollectible => true;

        private ObjSrcDocument _Document;
        /// <summary>Document the element of a part of</summary>
        public virtual ObjSrcDocument Document => _Document;

        private ObjSrcCollection _Collection;
        /// <summary>Collection the element of a part of</summary>
        public ObjSrcCollection Collection => _Collection;

        /// <summary>"Adds" the element to the specified collection</summary>
        /// <param name="collection">Collection</param>
        /// <exception cref="NotSupportedException">Element cannot be a part of a collection</exception>
        /// <exception cref="InvalidOperationException">Element is already part of a collection</exception>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null</exception>
        internal virtual void AddToCollection_m(ObjSrcCollection collection)
        {
            if (!(_Collection is null)) throw new InvalidOperationException("Element is currently part of a collection.");
            try
            {
                _Document = collection.Document;
                _Collection = collection;
            }
            catch when (collection is null) { throw new ArgumentNullException(nameof(collection)); }
        }

        /// <summary>"Removes" the element from the collection it is "a part of"</summary>
        internal void RemoveFromCollection_m()
        {
            _Document = null;
            _Collection = null;
        }
    }
}
