using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcSrcElementException : ObjSrcException
    {
        #region helper

        private static string H_CreateMessage_m(ObjSrcElementPath path, string message)
        {
            if (path is null) return message;
            if (path.Length == 0) return message;
            return $"{message}  {path}.";
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcSrcElementException"/></summary>
        /// <param name="srcElement">Source element related to the error</param>
        /// <param name="message">Message explaining the error</param>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        public ObjSrcSrcElementException(ObjSrcElement srcElement, string message)
        {
            try
            {
                SrcElement = srcElement;
                Path = ObjSrcElementPath.Create(srcElement);
                Message = H_CreateMessage_m(Path, message);
                BaseMessage_p = message;
            }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }

        /// <summary>Creates an instance of <see cref="ObjSrcSrcElementException"/></summary>
        /// <param name="srcElement">Source element related to the error</param>
        /// <param name="path">Path to the source element</param>
        /// <param name="message">Message explaining the error</param>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        public ObjSrcSrcElementException(ObjSrcElement srcElement, ObjSrcElementPath path, string message)
        {
            try
            {
                SrcElement = srcElement;
                Path = path;
                Message = H_CreateMessage_m(Path, message);
                BaseMessage_p = message;
            }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }

        /// <summary>Source element that caused the error</summary>
        public ObjSrcElement SrcElement { get; }

        /// <summary>Path to the source element</summary>
        public ObjSrcElementPath Path { get; }

        /// <inheritdoc/>
        public override string Message { get; }

        /// <inheritdoc/>
        internal override string BaseMessage_p { get; }
    }
}
