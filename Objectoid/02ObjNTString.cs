using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents a null-terminated string</summary>
    public class ObjNTString : IEquatable<ObjNTString>, IComparable<ObjNTString>, IEnumerable<byte>
    {
        #region equality

        private bool Equals_m(ObjNTString other)
        {
            if (other is null) return false;
            //Check length
            if (other._Chars.Length != _Chars.Length) return false;
            //Check each character
            for (int i = 0; i < _Chars.Length; i++)
            {
                if (other._Chars[i] != _Chars[i]) return false;
            }
            //Success
            return true;
        }

        private bool Equals_m(string s)
        {
            if (s is null) return false;
            //Check length
            if (s.Length != _Chars.Length) return false;
            //Check each character
            for (int i = 0; i < _Chars.Length; i++)
            {
                if (s[i] != _Chars[i]) return false;
            }
            //Success
            return true;
        }

        private static bool Equals_m(ObjNTString a, ObjNTString b)
        {
            if (a is null) return b is null;
            return a.Equals(b);
        }

        /// <summary>Checks if the two null-terminated strings are equal in value</summary>
        /// <param name="a">Null-terminated string</param>
        /// <param name="b">Null-terminated string</param>
        /// <returns>Whether or not the two null-terminated strings are equal in value</returns>
        public static bool operator ==(ObjNTString a, ObjNTString b) => Equals_m(a, b);

        /// <summary>Checks if the two null-terminated strings are not equal in value</summary>
        /// <param name="a">Null-terminated string</param>
        /// <param name="b">Null-terminated string</param>
        /// <returns>Whether or not the two null-terminated strings are not equal in value</returns>
        public static bool operator !=(ObjNTString a, ObjNTString b) => !Equals_m(a, b);

        #endregion

        #region object

        /// <summary>Determines whether or not the current instance is equal in value to the specified other instance</summary>
        /// <param name="obj">Other instance</param>
        /// <returns>Whether or not the current instance is equal in value to the specified other instance</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is ObjNTString) return Equals_m((ObjNTString)obj);
            if (obj is string) return Equals_m((string)obj);
            return false;
        }

        /// <summary>Gets a hash code for the instance</summary>
        /// <returns>A hash code for the instance</returns>
        public override int GetHashCode() => _Chars.Length;

        /// <summary>Creates a true string representation of the null-terminated string</summary>
        /// <returns>A true string representation of the null-terminated string</returns>
        public override string ToString()
        {
            char[] chars = new char[_Chars.Length];
            for (int i = 0; i < _Chars.Length; i++)
                chars[i] = (char)_Chars[i];
            return new string(chars);
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether or not the current null-terminated string is equal in value 
        /// to the specified other null-terminated string</summary>
        /// <param name="other">Other null-terminated string</param>
        /// <returns>Whether or not the current null-terminated string is equal in value 
        /// to the specified other null-terminated string</returns>
        public bool Equals(ObjNTString other) => Equals_m(other);

        #endregion

        #region IComparable

        /// <summary>Compares the current null-terminated string with a specified null-terminated string 
        /// and determines whether or not the current null-terminated string precedes, follows, 
        /// or occurs in the same sorting position as the other element</summary>
        /// <param name="other">Other null-terminated string</param>
        /// <returns>Less than zero: Current null-terminated string precedes the other null-terminated string
        /// <br/>Equal to zero: Current null-terminated string occurs in the same position as the other null-terminated string
        /// <br/>Greater than zero: Current null-terminated string follows the other null-terminated string</returns>
        public int CompareTo(ObjNTString other)
        {
            if (other == null) return 1;
            //Compare lengths
            int minLength;
            int lenCompare;
            if (other._Chars.Length < _Chars.Length)
            {
                minLength = other._Chars.Length;
                lenCompare = 1;
            }
            else
            {
                minLength = _Chars.Length;
                lenCompare = (other._Chars.Length > _Chars.Length) ? -1 : 0;
            }
            //Compare characters
            for (int i = 0; i < minLength; i++)
            {
                byte thisChar = _Chars[i];
                byte otherChar = other._Chars[i];
                if (otherChar == thisChar) continue;
                return (otherChar < thisChar) ? 1 : -1;
            }
            //Return length comparision
            return lenCompare;
        }

        #endregion

        #region IEnumerable

        /// <summary>Gets an enumerator for the null-terminated string</summary>
        /// <returns>An enumerator for the null-terminated string</returns>
        public IEnumerator<byte> GetEnumerator() => new Enumerator_(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator_(this);

        private class Enumerator_ : IEnumerator<byte>
        {
            public Enumerator_(ObjNTString instance)
            {
                _Instance = instance;
            }

            ObjNTString _Instance;
            int __Position = -1;

            private byte Current_p
            {
                get
                {
                    try { return _Instance._Chars[__Position]; }
                    catch { throw new InvalidOperationException(); }
                }
            }
            byte IEnumerator<byte>.Current => Current_p;
            object IEnumerator.Current => Current_p;

            void IDisposable.Dispose() { }

            bool IEnumerator.MoveNext() => (++__Position) < _Instance._Chars.Length;

            void IEnumerator.Reset() => __Position = -1;
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjNTString"/> using the specified true string</summary>
        /// <param name="s">True string</param>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="s"/> contains one or more null characters
        /// <br/>or
        /// <br/><paramref name="s"/> contains one or more non-8-bit characters</exception>
        public ObjNTString(string s)
        {
            try
            {
                _Chars = new byte[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    //Ensure character is not null
                    if (c == 0x00) throw new ArgumentException(
                        "String contains one or more null characters.", nameof(s));
                    //Ensure character is 8-bit
                    if ((c & 0xFF00) != 0) throw new ArgumentException(
                        "String contains one or more non-8-bit characters.", nameof(s));
                    //Add character
                    _Chars[i] = (byte)c;
                }
            }
            catch when (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
        }

        /// <summary>Constructor for <see cref="ObjNTString"/>
        /// <br/>NOTE: It is assumed <paramref name="chars"/> is not null, 
        /// contains no null characters, 
        /// contains only 8-bit characters, 
        /// and does not exist outside this scope</summary>
        /// <param name="chars">Characters</param>
        internal ObjNTString(byte[] chars)
        {
            _Chars = chars;
        }

        private readonly byte[] _Chars;

        /// <summary>Length of the null-terminated string</summary>
        public int Length => _Chars.Length;

        /// <summary>Gets the character at the specified index</summary>
        /// <param name="index">Index of the character</param>
        /// <returns>The character at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range</exception>
        public byte this[int index]
        {
            get
            {
                try
                {
                    return _Chars[index];
                }
                catch when (index < 0 || index >= _Chars.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }
}
