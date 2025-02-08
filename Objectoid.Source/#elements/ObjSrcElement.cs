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

        /// <summary>Sets the value of <see cref="__Document"/> for the specified element and it's descendents</summary>
        /// <param name="element">Element</param>
        /// <param name="document">Document</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        private static void SetDocument_m(ObjSrcElement element, ObjSrcDocument document)
        {
            try
            {
                element.__Document = document;
                if (element is ObjSrcCollection)
                {
                    foreach (var child in (ObjSrcCollection)element)
                        SetDocument_m(child, document);
                }
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        /// <summary>Creates an enumerable collection of names of potential protocols with the specified suffix</summary>
        /// <returns>An enumerable collection of potential protocols with the specified suffix</returns>
        /// <exception cref="InvalidOperationException">Element is not part of a document</exception>
        private protected IEnumerable<string> EnumeratePotentialProtocols_m(string suffix)
        {
            int count;
            try { count = __Document.HeaderStatements.Count; }
            catch when (__Document is null) { throw new InvalidOperationException("Element is not part of a document."); }

            yield return suffix;
            for (int i = 0; i < count; i++)
            {
                var statement = __Document.HeaderStatements[i];
                if (!(statement is ObjSrcProtocolPrefix)) continue;
                var protocolPrefix = (ObjSrcProtocolPrefix)statement;
                yield return $"{protocolPrefix.Value}{suffix}";
            }
        }

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

        private ObjSrcDocument __Document;
        /// <summary>Document the element of a part of</summary>
        public virtual ObjSrcDocument Document => __Document;

        private ObjSrcCollection __Collection;
        /// <summary>Collection the element of a part of</summary>
        public ObjSrcCollection Collection => __Collection;

        /// <summary>"Adds" the element to the specified collection</summary>
        /// <param name="collection">Collection</param>
        /// <exception cref="NotSupportedException">Element cannot be a part of a collection</exception>
        /// <exception cref="InvalidOperationException">Element is already part of a collection</exception>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null</exception>
        internal virtual void AddToCollection_m(ObjSrcCollection collection)
        {
            if (!(__Collection is null)) throw new InvalidOperationException("Element is currently part of a collection.");
            try
            {
                SetDocument_m(this, collection.Document);
                __Collection = collection;

            }
            catch when (collection is null) { throw new ArgumentNullException(nameof(collection)); }
        }

        /// <summary>"Removes" the element from the collection it is "a part of"</summary>
        internal void RemoveFromCollection_m()
        {
            SetDocument_m(this, null);
            __Collection = null;
        }

        /// <summary>Forces the specified element to update it's information</summary>
        /// <param name="element">Element to update</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        private protected static void UpdateElement_m(ObjSrcElement element)
        {
            Console.WriteLine($"{ObjSrcElementPath.Create(element)} {(element.__Document is null)} {(element.__Collection?.__Document is null)}");
            try { element.__Document = element.__Collection?.__Document; }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }
    }
}
