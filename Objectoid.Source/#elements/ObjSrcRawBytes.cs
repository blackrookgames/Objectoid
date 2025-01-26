using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rookie;

namespace Objectoid.Source
{
    /// <summary>Represents an objectoid raw bytes source</summary>
    [ObjSrcReadable(ObjSrcKeyword._RawBytes)]
    [ObjSrcDecodable(typeof(ObjRawBytesElement))]
    public class ObjSrcRawBytes : ObjSrcElement, IObjSrcDecodable<ObjRawBytesElement>, IEnumerable<byte>
    {
        #region helper

        private static void H_ThrowIfNegative_m(int arg, string paramName)
        {
            if (arg < 0) throw new ArgumentOutOfRangeException(paramName, $"Value must be non-negative.");
        }

        #endregion

        #region IObjSrcDecodable

        void IObjSrcDecodable<ObjRawBytesElement>.Decode(ObjRawBytesElement element)
        {
            try
            {
                __Bytes = new byte[element.Length];
                for (int i = 0; i < element.Length; i++)
                    __Bytes[i] = element[i];
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator thru the byte values</summary>
        /// <returns>An enumerator thru the byte values</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < __Bytes.Length; i++)
                yield return __Bytes[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcRawBytes"/></summary>
        public ObjSrcRawBytes() : this(0) { }

        /// <summary>Creates an instance of <see cref="ObjSrcRawBytes"/> with the specified initial length</summary>
        /// <param name="initialLength">Initial length</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="initialLength"/> is negative</exception>
        public ObjSrcRawBytes(int initialLength)
        {
            H_ThrowIfNegative_m(initialLength, nameof(initialLength));
            __Bytes = new byte[initialLength];
        }

        /// <summary>Creates an instance of <see cref="ObjSrcRawBytes"/> with the specified initial data</summary>
        /// <param name="initialData">Initial data</param>
        /// <exception cref="ArgumentNullException"><paramref name="initialData"/> is null</exception>
        public ObjSrcRawBytes(IEnumerable<byte> initialData)
        {
            try { __Bytes = initialData.ToArray(); }
            catch when (initialData is null) { throw new ArgumentNullException(nameof(initialData)); }
        }

        /// <inheritdoc/>
        internal override void Load_m(ObjSrcReader reader)
        {
            try
            {
                var bytes = new List<byte>();

                while (true)
                {
                    reader.Read();
                    if (reader.Token.Type == ObjSrcReaderTokenType.None) break;
                    if (reader.Token.Type == ObjSrcReaderTokenType.Numeric)
                    {
                        if (!Parser.TryToUInt8(reader.Token.Text, out var @byte))
                            new ObjSrcReaderException($"\"{reader.Token.Text}\" is not a valid byte value.", reader.Token);
                        bytes.Add(@byte);
                        continue;
                    }
                    ObjSrcReaderException.ThrowUnexpectedToken(reader.Token);
                }

                __Bytes = bytes.ToArray();
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <inheritdoc/>
        internal override void Save_m(ObjSrcWriter writer)
        {
            try
            {
                writer.Write($"{ObjSrcKeyword._RawBytes}");
                if (__Bytes.Length == 0) writer.WriteLine();
                else
                {
                    writer.WriteLine($" {ObjSrcSymbol._Stretch}");
                    writer.IncIndent();

                    var i = 0;
                    while (true)
                    {
                        writer.Write($"0x{__Bytes[i++]:X2} ");
                        if (i == __Bytes.Length) break;
                        if ((i % 16) == 0) writer.WriteLine($"{ObjSrcSymbol._Stretch}");
                    }

                    writer.WriteLine();
                    writer.DecIndent();
                }
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <inheritdoc/>
        internal override ObjElement CreateElement_m(IObjSrcImportOptions options)
        {
            return new ObjRawBytesElement(__Bytes);
        }

        private byte[] __Bytes;

        /// <summary>Length</summary>
        public int Length => __Bytes.Length;

        /// <summary>Gets or sets the byte value at the specified index</summary>
        /// <param name="index">Index of the byte</param>
        /// <returns>The byte value at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public byte this[int index]
        {
            get
            {
                try { return __Bytes[index]; }
                catch when (index < 0 || index >= __Bytes.Length) { throw new ArgumentOutOfRangeException(nameof(index)); }
            }
            set
            {
                try { __Bytes[index] = value; }
                catch when (index < 0 || index >= __Bytes.Length) { throw new ArgumentOutOfRangeException(nameof(index)); }
            }
        }

        /// <summary>Sets the length</summary>
        /// <param name="length">Length</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative</exception>
        public void SetLength(int length)
        {
            H_ThrowIfNegative_m(length, nameof(length));
            byte[] oldBytes = __Bytes;
            __Bytes = new byte[length];
            int minLength = (oldBytes.Length < length) ? oldBytes.Length : length;
            for (int i = 0; i < minLength; i++)
                __Bytes[i] = oldBytes[i];
        }
    }
}
