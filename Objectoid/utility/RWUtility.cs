using System;
using System.Collections.Generic;
using System.IO;

namespace Objectoid
{
    internal static class RWUtility
    {
        /// <summary>Reads a string using the specified reader</summary>
        /// <param name="reader">Objectoid reader</param>
        /// <returns>Value of the string</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public static string ReadString(ObjReader reader)
        {
            try
            {
                //Read length and character size
                int sizeLen = reader.ReadInt32();
                bool is8bit = (sizeLen & int.MinValue) != 0;
                int length = sizeLen & int.MaxValue;
                //Read chracters
                char[] chars = new char[length];
                if (is8bit)
                {
                    for (int i = 0; i < length; i++)
                        chars[i] = (char)reader.ReadUInt8();
                }
                else
                {
                    for (int i = 0; i < length; i++)
                        chars[i] = (char)reader.ReadUInt16();
                }
                return new string(chars);
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <summary>Writes a string using the specified writer</summary>
        /// <param name="writer">Objectoid writer</param>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public static void WriteString(ObjWriter writer, string value)
        {
            try
            {
                if (value is null)
                {
                    writer.WriteInt32(0);
                }
                else
                {
                    //Determine character size
                    bool is8bit = true;
                    foreach (char c in value)
                    {
                        if ((c & 0xFF00) == 0) continue;
                        is8bit = false;
                        break;
                    }
                    //Write length and character size
                    int sizelen = value.Length;
                    if (is8bit) sizelen |= int.MinValue;
                    writer.WriteInt32(sizelen);
                    //Write characters
                    if (is8bit)
                    {
                        foreach (char c in value)
                            writer.WriteUInt8((byte)c);
                    }
                    else
                    {
                        foreach (char c in value)
                            writer.WriteUInt16(c);
                    }
                }
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }

        /// <summary>Reads a null-terminated string using the specified reader</summary>
        /// <param name="reader">Objectoid reader</param>
        /// <returns>Value of the null-terminated string</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        /// <exception cref="EndOfStreamException">End of stream has already been reached</exception>
        public static ObjNTString ReadNTString(ObjReader reader)
        {
            try
            {
                List<byte> chars = new List<byte>();
                while (true)
                {
                    byte c = reader.ReadUInt8();
                    if (c == 0x00) break;
                    chars.Add(c);
                }
                return new ObjNTString(chars.ToArray());
            }
            catch when (reader is null) { throw new ArgumentNullException(nameof(reader)); }
        }

        /// <summary>Writes a null-terminated string using the specified writer</summary>
        /// <param name="writer">Objectoid writer</param>
        /// <param name="value">Value</param>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public static void WriteNTString(ObjWriter writer, ObjNTString value)
        {
            try
            {
                if (value is null)
                {
                    writer.WriteUInt8(0);
                }
                else
                {
                    foreach (byte c in value)
                        writer.WriteUInt8(c);
                    writer.WriteUInt8(0);
                }
            }
            catch when (writer is null) { throw new ArgumentNullException(nameof(writer)); }
        }
    }
}
