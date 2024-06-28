using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Objectoid
{
    /// <summary>Represents a writer for an Objectoid document</summary>
    internal class ObjWriter : ObjRW
    {
        /// <summary>Represents the entry of an addressable element registered with a writer</summary>
        public struct RegisteredElementEntry_
        {
            /// <summary>Creates an instance of <see cref="RegisteredElementEntry_"/>
            /// <br/>NOTE: It is assumed <paramref name="element"/> is not null
            /// <br/>NOTE: It is assumed <paramref name="address"/> is non-negative</summary>
            /// <param name="element">Element</param>
            /// <param name="address">Stream position</param>
            public RegisteredElementEntry_(ObjAddressable element, int address)
            {
                Element = element;
                Address = address;
            }

            /// <summary>Addressable element</summary>
            public ObjAddressable Element { get; }

            /// <summary>Stream position</summary>
            public int Address { get; }
        }

        /// <summary>Represents the entry of a property name registered with a writer</summary>
        public struct RegisteredPropertyNameEntry_
        {
            /// <summary>Creates an instance of <see cref="RegisteredPropertyNameEntry_"/>
            /// <br/>NOTE: It is assumed <paramref name="name"/> is not null
            /// <br/>NOTE: It is assumed <paramref name="address"/> is non-negative</summary>
            /// <param name="name">Property name</param>
            /// <param name="address">Stream position</param>
            public RegisteredPropertyNameEntry_(ObjNTString name, int address)
            {
                Name = name;
                Address = address;
            }

            /// <summary>Property name</summary>
            public ObjNTString Name { get; }

            /// <summary>Stream position</summary>
            public int Address { get; }
        }

        /// <summary>Constructor for <see cref="ObjWriter"/></summary>
        /// <param name="stream">Stream</param>
        /// <param name="intIsLittleEndian">Whether or not integers are stored as little-endian</param>
        /// <param name="floatIsLittleEndian">Whether or not floats are stored as little-endian</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking
        /// <br/>or
        /// <br/><paramref name="stream"/> does not support writing</exception>
        public ObjWriter(Stream stream, bool intIsLittleEndian, bool floatIsLittleEndian) :
            base(stream)
        {
            if (!stream.CanWrite)
                throw new ArgumentException("The stream does not support writing.", nameof(stream));
            SetIntIsLittleEndian_m(intIsLittleEndian);
            SetFloatIsLittleEndian_m(floatIsLittleEndian);
        }

        #region RegisteredAddressables

        private readonly Dictionary<ObjAddressable, RegisteredElementEntry_> _RegisteredAddressables =
            new Dictionary<ObjAddressable, RegisteredElementEntry_>(new ObjAddressable.EqualityComparer_());

        /// <summary>Addressable elements that are currently registered with the writer</summary>
        public IEnumerable<RegisteredElementEntry_> RegisteredAddressables => _RegisteredAddressables.Values;

        /// <summary>Attempts to register the specified addressable element, 
        /// assuming the current stream position is where the element's data starts
        /// <br/>CALLED BY: <see cref="ObjDocument"/></summary>
        /// <param name="element">Addressable element to register</param>
        /// <returns>True if the neither the addressable element 
        /// nor an addressable element of equal value was already registered with the writer
        /// <br/>False if either the addressable element 
        /// or an addressable element of equal value was already registered with the writer</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="OverflowException">It was attempted to reference a stream position greater than <see cref="int.MaxValue"/></exception>
        public bool RegisterAddressable(ObjAddressable element)
        {
            try
            {
                //Ensure element can be registered
                if (_RegisteredAddressables.ContainsKey(element)) return false;
                //Get address
                int address;
                try
                {
                    unchecked { address = (int)Stream.Position; }
                }
                catch (OverflowException)
                {
                    throw new OverflowException($"Cannot reference a stream position greater than {int.MaxValue}.");
                }
                //Add element
                _RegisteredAddressables.Add(element, new RegisteredElementEntry_(element, address));
                //Success
                return true;
            }
            catch when (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
        }

        /// <summary>Writes the address of the specified addressable element
        /// <br/>CALLED BY: <see cref="ObjDocument"/>, derivatives of <see cref="ObjCollection"/></summary>
        /// <param name="element">Addressable element</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="element"/> has not been registered with the writer</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WriteAddressable(ObjAddressable element)
        {
            try
            {
                WriteInt32(_RegisteredAddressables[element].Address);
            }
            catch when (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            catch when (!_RegisteredAddressables.ContainsKey(element))
            {
                throw new ArgumentException(
                    "The addressable element has not been registered with the writer.", nameof(element));
            }
        }

        #endregion

        #region RegisteredPropertyNames

        private readonly Dictionary<ObjNTString, RegisteredPropertyNameEntry_> _RegisteredPropertyNames =
            new Dictionary<ObjNTString, RegisteredPropertyNameEntry_>();

        /// <summary>Property names that are currently registered with the writer</summary>
        public IEnumerable<RegisteredPropertyNameEntry_> RegisteredPropertyNames => _RegisteredPropertyNames.Values;

        /// <summary>Attempts to register the specified property name, 
        /// assuming the current stream position is where the property name starts
        /// <br/>CALLED BY: <see cref="ObjDocument"/></summary>
        /// <param name="name">Property name to register</param>
        /// <returns>True if the property name was not already registered with the writer
        /// <br/>False if the property name was already registered with the writer</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        /// <exception cref="OverflowException">It was attempted to reference a stream position greater than <see cref="int.MaxValue"/></exception>
        public bool RegisterPropertyName(ObjNTString name)
        {
            try
            {
                //Ensure element can be registered
                if (_RegisteredPropertyNames.ContainsKey(name)) return false;
                //Get address
                int address;
                try
                {
                    unchecked { address = (int)Stream.Position; }
                }
                catch (OverflowException)
                {
                    throw new OverflowException($"Cannot reference a stream position greater than {int.MaxValue}.");
                }
                //Add element
                _RegisteredPropertyNames.Add(name, new RegisteredPropertyNameEntry_(name, address));
                //Success
                return true;
            }
            catch when (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>Writes the address of the specified property name
        /// <br/>CALLED BY: <see cref="ObjDocument"/>, derivatives of <see cref="ObjCollection"/></summary>
        /// <param name="name">Property name</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> has not been registered with the writer</exception>
        /// <exception cref="IOException">I/O error occured</exception>
        /// <exception cref="ObjectDisposedException">Stream was already disposed</exception>
        public void WritePropertyName(ObjNTString name)
        {
            try
            {
                WriteInt32(_RegisteredPropertyNames[name].Address);
            }
            catch when (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            catch when (!_RegisteredPropertyNames.ContainsKey(name))
            {
                throw new ArgumentException(
                    "The property name has not been registered with the writer.", nameof(name));
            }
        }

        #endregion
    }
}
