using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Thrown when a source-related error occurs</summary>
    public class ObjSrcException : Exception
    {
        /// <summary>Creates an instance of <see cref="ObjSrcException"/></summary>
        /// <param name="message">Message explaining the exception</param>
        public ObjSrcException(string message) : 
            base(message)
        { }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining a syntax error that occured at the specified row and column within a text source</summary>
        /// <param name="message">Message explaining the syntax error</param>
        /// <param name="rowIndex">Row index within text source in which syntax error occurs</param>
        /// <param name="colIndex">Column index within text source in which syntax error occurs</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowSyntaxError_m(string message, int rowIndex, int colIndex)
        {
            throw new ObjSrcException($"{message}  Line {(rowIndex + 1)}  Position {(colIndex + 1)}.");
        }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining a syntax error that involves the specified token</summary>
        /// <param name="message">Message explaining the syntax error</param>
        /// <param name="token">Token related to the syntax error</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowSyntaxError_m(string message, ObjSrcReaderToken token)
        {
            throw ThrowSyntaxError_m(message, token.Row, token.Column);
        }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining that the specified token was unexpected</summary>
        /// <param name="token">unexpected token</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowUnexpectedToken_m(ObjSrcReaderToken token)
        {
            if (token.Type == ObjSrcReaderTokenType.None)
                ThrowSyntaxError_m($"The line ends unexpectedly.", token);
            throw ThrowSyntaxError_m($"The token {token.Text} was unexpected.", token);
        }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining that the keyword is unexpected</summary>
        /// <param name="keyword">Keyword token</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowUnexpectedKeyword_m(ObjSrcReaderToken keyword)
        {
            throw ThrowSyntaxError_m($"The keyword {keyword.Text} was unexpected.", keyword);
        }
    }
}
