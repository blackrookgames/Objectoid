using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Thrown when a source-related error occurs</summary>
    public class ObjSrcException : Exception
    {
        #region helper

        private static string H_SyntaxErrorMessage_m(string message, int rowIndex, int colIndex) =>
            $"{message}  Line {(rowIndex + 1)}  Position {(colIndex + 1)}.";

        #endregion

        internal ObjSrcException(string message, string baseMessage,
            int rowIndex = -1,
            int colIndex = -1,
            ObjSrcElement srcElement = null,
            ObjSrcReaderToken token = default) :
            base(message)
        {
            BaseMessage = baseMessage;
            RowIndex = rowIndex;
            ColIndex = colIndex;
            SrcElement = srcElement;
            Token_p = token;
        }

        /// <summary>Creates an instance of <see cref="ObjSrcException"/></summary>
        /// <param name="message">Message explaining the exception</param>
        public ObjSrcException(string message) : 
            this(message, message)
        { }

        /// <summary>Base message</summary>
        public string BaseMessage { get; }

        /// <summary>Row index within the text source in which a syntax error occurs</summary>
        public int RowIndex { get; }

        /// <summary>Column index within the text source in which a syntax error occurs</summary>
        public int ColIndex { get; }

        /// <summary>Source element that caused the error</summary>
        public ObjSrcElement SrcElement { get; }

        /// <summary>Token related to the error</summary>
        internal ObjSrcReaderToken Token_p { get; }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining a syntax error that occured at the specified row and column within a text source</summary>
        /// <param name="message">Message explaining the syntax error</param>
        /// <param name="rowIndex">Row index within text source in which syntax error occurs</param>
        /// <param name="colIndex">Column index within text source in which syntax error occurs</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowSyntaxError_m(string message, int rowIndex, int colIndex)
        {
            throw new ObjSrcException(H_SyntaxErrorMessage_m(message, rowIndex, colIndex), message,
                rowIndex: rowIndex,
                colIndex: colIndex);
        }

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining a syntax error that involves the specified token</summary>
        /// <param name="message">Message explaining the syntax error</param>
        /// <param name="token">Token related to the syntax error</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        internal static ObjSrcException ThrowSyntaxError_m(string message, ObjSrcReaderToken token)
        {
            throw new ObjSrcException(H_SyntaxErrorMessage_m(message, token.Row, token.Column), message,
                rowIndex: token.Row,
                colIndex: token.Column,
                token: token);
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

        /// <summary>Throws an <see cref="ObjSrcException"/> explaining an invalid source element</summary>
        /// <param name="srcElement">Source element</param>
        /// <param name="message">Message explaining the invalid source element</param>
        /// <exception cref="ObjSrcException">Expected outcome</exception>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        internal static ObjSrcException ThrowInvalidSource_m(ObjSrcElement srcElement, string message)
        {
            object determineID(ObjSrcElement e)
            {
                if (e.Collection is ObjSrcObject)
                {
                    var srcObject = (ObjSrcObject)e.Collection;
                    foreach (var property in srcObject)
                    {
                        if (e == property.Value) return property.Name;
                    }
                }
                if (e.Collection is ObjSrcList)
                {
                    var srcList = (ObjSrcList)e.Collection;
                    for (int i = 0; i < srcList.Count; i++)
                    {
                        if (e == srcList[i]) return i;
                    }
                }
                return null;
            }

            try
            {
                var path = new StringBuilder($"{determineID(srcElement)}");
                ObjSrcElement collection = srcElement.Collection;
                while ((!(collection is null)) && (!(collection is ObjSrcRoot)))
                {
                    path.Insert(0, $"{determineID(collection)}/");
                    collection = collection.Collection;
                }
                throw new ObjSrcException($"{message}  {path}.", message, 
                    srcElement: srcElement);
            }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }
    }
}
