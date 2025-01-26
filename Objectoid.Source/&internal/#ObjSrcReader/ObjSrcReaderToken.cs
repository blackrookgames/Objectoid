using System;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    internal struct ObjSrcReaderToken
    {
        /// <summary>Creates an instance of <see cref="ObjSrcReaderToken"/></summary>
        /// <param name="type">Type</param>
        /// <param name="row">Row</param>
        /// <param name="column">Column</param>
        /// <param name="text">Token text</param>
        public ObjSrcReaderToken(ObjSrcReaderTokenType type, int row, int column, string text)
        {
            Type = type;
            Row = row;
            Column = column;
            Text = text;
        }

        #region properties

        /// <summary>Type of token</summary>
        public ObjSrcReaderTokenType Type { get; }

        /// <summary>Row index of token</summary>
        public int Row { get; }

        /// <summary>Column index of first character of token</summary>
        public int Column { get; }

        /// <summary>Token text</summary>
        public string Text { get; }

        #endregion

        #region exception

        /// <summary>Throws an <see cref="ObjSrcException"/> if the token does not indicate an end of line</summary>
        /// <exception cref="ObjSrcException">Token does not indicate an end of line</exception>
        internal void ThrowIfNotEOL_m()
        {
            if (Type != ObjSrcReaderTokenType.None)
                ObjSrcReaderException.ThrowUnexpectedToken(this);
        }

        #endregion
    }
}
