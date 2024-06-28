using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents a reader for an Objectoid document</summary>
    internal class ObjReader : ObjRW
    {
        /// <summary>Constructor for <see cref="ObjReader"/></summary>
        /// <param name="stream">Stream</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking
        /// <br/>or
        /// <br/><paramref name="stream"/> does not support reading</exception>
        public ObjReader(Stream stream) :
            base(stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("The stream does not support reading.", nameof(stream));
        }

        /// <inheritdoc cref="ObjRW.SetIntIsLittleEndian_m(bool)"/>
        public void SetIntIsLittleEndian(bool value) => SetIntIsLittleEndian_m(value);

        /// <inheritdoc cref="ObjRW.SetFloatIsLittleEndian_m(bool)"/>
        public void SetFloatIsLittleEndian(bool value) => SetFloatIsLittleEndian_m(value);

        /// <summary>Reads an address</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        /// <exception cref="InvalidDataException">Address is out of range</exception>
        public int ReadAddress()
        {
            int address = ReadInt32();
            if (address < 0 || address >= Stream.Length)
                throw InvalidData(Stream.Position - 4, "Address is out of range.");
            return address;
        }

        /// <summary>Reads a property name</summary>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public ObjNTString ReadPropertyName()
        {
            List<byte> chars = new List<byte>();
            while (true)
            {
                byte c = ReadUInt8();
                if (c == 0x00) break;
                chars.Add(c);
            }
            return new ObjNTString(chars.ToArray());
        }

        /// <summary>Creates an instance of <see cref="InvalidDataException"/> using the specified stream position and message</summary>
        /// <param name="position">Stream position</param>
        /// <param name="message">Message</param>
        /// <returns>An instance of <see cref="InvalidDataException"/></returns>
        public InvalidDataException InvalidData(long position, string message) =>
            new InvalidDataException($"{message} Stream position 0x{position:X8}.");
    }
}
