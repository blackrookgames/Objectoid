using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    /// <summary>Thrown when a source-related error occurs</summary>
    public class ObjSrcException : Exception
    {
        /// <summary>Constructor for <see cref="ObjSrcException"/></summary>
        private protected ObjSrcException() : base() { }

        /// <summary>Creates an instance of <see cref="ObjSrcException"/></summary>
        /// <param name="message">Message explaining the exception</param>
        public ObjSrcException(string message) : base(message)
        {
            BaseMessage_p = message;
        }

        /// <summary>Base message</summary>
        internal virtual string BaseMessage_p { get; }
    }
}
