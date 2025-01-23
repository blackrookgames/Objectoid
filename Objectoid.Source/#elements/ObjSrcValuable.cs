using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid value source</summary>
    public abstract class ObjSrcValuable<T> : ObjSrcElement
    {
        /// <inheritdoc/>
        internal sealed override void Load_m(ObjSrcReader reader)
        {
            try
            {
                reader.Read();
                if (reader.Token.Type != TokenType_p)
                    ObjSrcException.ThrowUnexpectedToken_m(reader.Token);
                if (!TryParse_m(reader.Token.Text, out T value))
                    ObjSrcException.ThrowSyntaxError_m($"\"{reader.Token.Text}\" is not a valid {Desc_p} value.", reader.Token);
                Value = value;
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal sealed override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{Keyword_p} ");
                writer.WriteLine(ToString_m(Value));
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <summary>Value</summary>
        public T Value { get; set; }

        /// <summary>Token type for a reader to look for</summary>
        private protected virtual ObjSrcReaderTokenType TokenType_p => ObjSrcReaderTokenType.Numeric;

        /// <summary>Keyword</summary>
        private protected abstract string Keyword_p { get; }

        /// <summary>Creates a string representation of the specified value</summary>
        /// <param name="value">Value</param>
        /// <returns>A string representation of the specified value</returns>
        private protected virtual string ToString_m(T value) => value?.ToString();

        /// <summary>Attempts to parse the specified string to a value of <typeparamref name="T"/></summary>
        /// <param name="s">String to parse</param>
        /// <param name="result">Result</param>
        /// <returns>Whether or not successful</returns>
        private protected abstract bool TryParse_m(string s, out T result);

        /// <summary>Brief description of the value type</summary>
        private protected abstract string Desc_p { get; }
    }
}
