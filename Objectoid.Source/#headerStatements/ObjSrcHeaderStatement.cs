using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents a statement within a objectoid source header</summary>
    public abstract class ObjSrcHeaderStatement : IObjSrcLoadSave
    {
        #region IObjSrcLoadSave

        void IObjSrcLoadSave.Load(ObjSrcReader reader) => Load_m(reader);

        void IObjSrcLoadSave.Save(ObjSrcWriter writer) => Save_m(writer);

        #endregion

        /// <inheritdoc cref="IObjSrcLoadSave.Load"/>
        internal abstract void Load_m(ObjSrcReader reader);

        /// <inheritdoc cref="IObjSrcLoadSave.Save"/>
        internal abstract void Save_m(ObjSrcWriter writer);

        private ObjSrcDocument _Document;
        /// <summary>Document the statement of a part of</summary>
        public ObjSrcDocument Document => _Document;

        /// <summary>"Adds" the statement to the specified source document</summary>
        /// <param name="document">Document</param>
        /// <exception cref="InvalidOperationException">Statement is already part of a source document</exception>
        internal void AddToDocument_m(ObjSrcDocument document)
        {
            if (_Document is null) _Document = document;
            else throw new InvalidOperationException("Statement is currently part of a source document.");
        }

        /// <summary>"Removes" the statement from the source document it is "a part of"</summary>
        internal void RemoveFromDocument_m() => _Document = null;
    }
}
