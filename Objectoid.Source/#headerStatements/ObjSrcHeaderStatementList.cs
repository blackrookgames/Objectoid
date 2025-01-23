using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents a list of statements within a objectoid source document</summary>
    public class ObjSrcHeaderStatementList : IEnumerable<ObjSrcHeaderStatement>
    {
        #region helper

        private protected static ArgumentException H_ThrowArgumentPartOfDocument_m(string paramName) =>
            throw new ArgumentException("The specified statment is already part of a source document.", nameof(paramName));

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator for the list</summary>
        /// <returns>An enumerator for the list</returns>
        public IEnumerator<ObjSrcHeaderStatement> GetEnumerator() => _Statements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Statements.GetEnumerator();

        #endregion

        /// <remarks>
        /// It is assumed<br/>
        /// <paramref name="document"/> is not null<br/>
        /// <br/>
        /// Called by <see cref="ObjSrcDocument"/>
        /// </remarks>
        internal ObjSrcHeaderStatementList(ObjSrcDocument document)
        {
            _Document = document;
            _Statements = new List<ObjSrcHeaderStatement>();
        }

        private readonly ObjSrcDocument _Document;
        private readonly List<ObjSrcHeaderStatement> _Statements;

        /// <summary>Document that contains the list</summary>
        public ObjSrcDocument Document => _Document;

        /// <summary>Number of statements within the list</summary>
        public int Count => _Statements.Count;

        /// <summary>Gets the statement at the specified index</summary>
        /// <param name="index">Index of the statement</param>
        /// <returns>The statement at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public ObjSrcHeaderStatement this[int index]
        {
            get
            {
                try { return _Statements[index]; }
                catch when (index < 0 || index >= _Statements.Count) { throw new ArgumentOutOfRangeException(nameof(index)); }
            }
        }

        /// <summary>Adds the specified statement to the list</summary>
        /// <param name="statement">Statement to add</param>
        /// <exception cref="ArgumentNullException"><paramref name="statement"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="statement"/> refers to an statement that is already part of a source document</exception>
        public void Add(ObjSrcHeaderStatement statement)
        {
            try
            {
                try { statement.AddToDocument_m(_Document); }
                catch (InvalidOperationException) { H_ThrowArgumentPartOfDocument_m(nameof(statement)); }

                _Statements.Add(statement);
            }
            catch when (statement is null) { throw new ArgumentNullException(nameof(statement)); }
        }

        /// <summary>Inserts the specified statement into the list at the specified index</summary>
        /// <param name="index">Index to insert statement</param>
        /// <param name="statement">Statement to add</param>
        /// <exception cref="ArgumentNullException">paramref name="statement"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="statement"/> refers to an statement that is already part of a source document</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public void Insert(int index, ObjSrcHeaderStatement statement)
        {
            try
            {
                try { statement.AddToDocument_m(_Document); }
                catch (InvalidOperationException) { H_ThrowArgumentPartOfDocument_m(nameof(statement)); }

                try { _Statements.Insert(index, statement); }
                catch { statement.RemoveFromDocument_m(); throw; }
            }
            catch when (statement is null) { throw new ArgumentNullException(nameof(statement)); }
            catch when (index < 0 || index > _Statements.Count) { throw new ArgumentOutOfRangeException(nameof(index)); }
        }

        /// <summary>Attempts to remove the specified statement from the list</summary>
        /// <param name="statement">Statement to remove</param>
        /// <returns>Whether or not successful</returns>
        public bool Remove(ObjSrcHeaderStatement statement)
        {
            var index = _Statements.IndexOf(statement);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>Removes the statement at the specified index from the list</summary>
        /// <param name="index">Index of statement</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public void RemoveAt(int index)
        {
            try
            {
                var statement = _Statements[index];
                statement.RemoveFromDocument_m();
                _Statements.RemoveAt(index);
            }
            catch when (index < 0 || index >= _Statements.Count) { throw new ArgumentOutOfRangeException(nameof(index)); }
        }

        /// <summary>Removes all statements from the list</summary>
        public void Clear()
        {
            for (int i = _Statements.Count - 1; i >= 0; i--)
                RemoveAt(i);
        }
    }
}
