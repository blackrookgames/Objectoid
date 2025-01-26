using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcReaderException : ObjSrcException
    {
        /// <summary>Creates an instance of <see cref="ObjSrcReaderException"/></summary>
        /// <param name="message">Message explaining the exception</param>
        /// <param name="rowIndex">Row index within the text source in which a syntax error occurs</param>
        /// <param name="colIndex">Column index within the text source in which a syntax error occurs</param>
        public ObjSrcReaderException(string message, int rowIndex, int colIndex) :
            this(message, rowIndex, colIndex, default)
        { }

        /// <summary>Creates an instance of <see cref="ObjSrcReaderException"/></summary>
        /// <param name="message">Message explaining the exception</param>
        /// <param name="token">Token related to the exception</param>
        public ObjSrcReaderException(string message, ObjSrcReaderToken token) :
            this(message, token.Row, token.Column, token)
        { }

        private ObjSrcReaderException(string message, int rowIndex, int colIndex, ObjSrcReaderToken token) : base()
        {
            BaseMessage_p = message;
            Message = $"{message}  Line {(rowIndex + 1)}  Position {(colIndex + 1)}.";
            RowIndex = rowIndex;
            ColIndex = colIndex;
            Token = token;
        }

        #region throw

        /// <summary>Throws an <see cref="ObjSrcReaderException"/> explaining that the specified token was unexpected</summary>
        /// <param name="token">unexpected token</param>
        /// <exception cref="ObjSrcReaderException">Expected outcome</exception>
        public static Exception ThrowUnexpectedToken(ObjSrcReaderToken token)
        {
            if (token.Type == ObjSrcReaderTokenType.None)
                throw new ObjSrcReaderException($"The line ends unexpectedly.", token);
            throw new ObjSrcReaderException($"The token {token.Text} was unexpected.", token);
        }

        /// <summary>Throws an <see cref="ObjSrcReaderException"/> explaining that the keyword is unexpected</summary>
        /// <param name="keyword">Keyword token</param>
        /// <exception cref="ObjSrcReaderException">Expected outcome</exception>
        public static Exception ThrowUnexpectedKeyword(ObjSrcReaderToken keyword)
        {
            throw new ObjSrcReaderException($"The keyword {keyword.Text} was unexpected.", keyword);
        }

        #endregion

        /// <inheritdoc/>
        internal override string BaseMessage_p { get; }

        /// <inheritdoc/>
        public override string Message { get; }

        /// <summary>Row index within the text source in which a syntax error occurs</summary>
        public int RowIndex { get; }

        /// <summary>Column index within the text source in which a syntax error occurs</summary>
        public int ColIndex { get; }

        /// <summary>Token related to the error</summary>
        public ObjSrcReaderToken Token { get; }
    }
}
