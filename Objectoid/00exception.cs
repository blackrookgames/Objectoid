using System;

namespace Objectoid
{
    /// <summary>Thrown when an argument-related error occurs</summary>
    internal class ArgException : Exception
    {
        /// <summary>Creates an instance of <see cref="ArgException"/></summary>
        /// <param name="message">Message</param>
        /// <param name="paramName">Parameter name</param>
        public ArgException(string message, string paramName) :
            base($"{message} (Parameter '{paramName}')")
        {
            MainMessage = message;
            ParamName = paramName;
        }

        /// <summary>Message, excluding parameter information</summary>
        public string MainMessage { get; }

        /// <summary>Parameter name</summary>
        public string ParamName { get; }
    }

    /// <summary>Thrown when an argument is null</summary>
    internal class ArgNullException : ArgException
    {
        /// <summary>Creates an instance of <see cref="ArgNullException"/></summary>
        /// <param name="paramName">Parameter name</param>
        public ArgNullException(string paramName) : base("Value cannot be null.", paramName) { }
    }
}
