using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcElementPath : IEnumerable<char>
    {
        #region object

        /// <summary>Gets a string representation of the path</summary>
        /// <returns>A string representation of the path</returns>
        public override string ToString() => _Path;

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator for the path</summary>
        /// <returns>An enumerator for the path</returns>
        public IEnumerator<char> GetEnumerator() => _Path.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Constructor for <see cref="ObjSrcElement"/></summary>
        /// <param name="srcElement">Source element</param>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        private ObjSrcElementPath(ObjSrcElement srcElement)
        {
            try
            {
                var path = new StringBuilder();
                var isRooted = false;

                while (true)
                {
                    var parent = srcElement.Collection;
                    if (parent is null) break;

                    string name;

                    //Object/Import
                    if (parent is ObjSrcObject)
                    {
                        var collection = (ObjSrcObject)parent;
                        foreach (var property in collection)
                        {
                            if (srcElement != property.Value) continue;
                            name = property.Name.ToString();
                            goto next;
                        }
                        goto @else;
                    }

                    //List
                    if (parent is ObjSrcList)
                    {
                        var collection = (ObjSrcList)parent;
                        for (int i = 0; i < collection.Count; i++)
                        {
                            if (srcElement != collection[i]) continue;
                            name = i.ToString();
                            goto next;
                        }
                        goto @else;
                    }

                @else:
                    name = srcElement.GetType().Name;
                next:
                    path.Insert(0, $"{name}/");
                    if (parent is ObjSrcRoot)
                    {
                        isRooted = true;
                        break;
                    }
                    srcElement = parent;
                }

                _IsRooted = isRooted;
                _Path = (path.Length == 0) ? "" : path.ToString()[..^1];
            }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }

        /// <summary>Constructor for <see cref="ObjSrcElement"/></summary>
        /// <param name="path2">First path</param>
        /// <param name="path1">Second path</param>
        /// <remarks>If <paramref name="path2"/> is rooted, <paramref name="path1"/> isn't used</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="path2"/> is null</exception>
        private ObjSrcElementPath(ObjSrcElementPath path1, ObjSrcElementPath path2)
        {
            try
            {
                if (path2._IsRooted || (path1 is null))
                {
                    _IsRooted = path2._IsRooted;
                    _Path = path2._Path;
                }
                else
                {
                    _IsRooted = path1._IsRooted;
                    _Path = path1._Path + ((path1._Path.Length > 0 && path2._Path.Length > 0) ? "/" : "") + path2._Path;
                }
            }
            catch when (path2 is null) { throw new ArgumentNullException(nameof(path2)); }
        }

        /// <summary>Creates an instance of <see cref="ObjSrcElement"/> using the specified source element</summary>
        /// <param name="srcElement">Source element</param>
        /// <returns>Created instance of <see cref="ObjSrcElement"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        public static ObjSrcElementPath Create(ObjSrcElement srcElement)
        {
            try { return new ObjSrcElementPath(srcElement); }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }

        /// <summary>Creates an instance of <see cref="ObjSrcElementPath"/> by combining the two paths</summary>
        /// <param name="path2">Path 1</param>
        /// <param name="path1">Path 2</param>
        /// <returns>Created instance of <see cref="ObjSrcElement"/></returns>
        /// <remarks>If <paramref name="path2"/> is rooted, <paramref name="path1"/> isn't used</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="path2"/> is null</exception>
        public static ObjSrcElementPath Create(ObjSrcElementPath path1, ObjSrcElementPath path2)
        {
            try { return new ObjSrcElementPath(path1, path2); }
            catch when (path2 is null) { throw new ArgumentNullException(nameof(path2)); }
        }

        #region fields

        private readonly bool _IsRooted;
        private readonly string _Path;

        #endregion

        #region Properties

        /// <summary>Whether or not the path is rooted</summary>
        public bool IsRooted => _IsRooted;

        /// <summary>Number of characters within the path</summary>
        public int Length => _Path.Length;

        /// <summary>Gets the character at the specified index</summary>
        /// <param name="index">Index of the character</param>
        /// <returns>The character at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public char this[int index]
        {
            get
            {
                try { return _Path[index]; }
                catch when (index < 0 || index >= _Path.Length) { throw new ArgumentOutOfRangeException(nameof(index)); }
            }
        }

        #endregion

        /// <summary>Casts the path to a string</summary>
        /// <param name="path">Path</param>
        public static explicit operator string(ObjSrcElementPath path) => path._Path;
    }
}
