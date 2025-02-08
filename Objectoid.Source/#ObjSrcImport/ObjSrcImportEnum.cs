using System;
using System.Reflection;

namespace Objectoid.Source
{
    /// <summary>Represents a supported enumeration</summary>
    public readonly struct ObjSrcImportEnum : INamedCollectionItem<string>
    {
        #region helper

        /// <summary>Attempts to get an instance of <see cref="ObjSrcEnumCompatible"/> that supports an enumeration of the specified type</summary>
        /// <param name="enumType">Enumeration type</param>
        /// <param name="compatible">Retrieved instance of <see cref="ObjSrcEnumCompatible"/></param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> does not represent an enumeration</exception>
        private static bool TryGetEnumCompatible_m(Type enumType, out ObjSrcEnumCompatible compatible)
        {
            try
            {
                var underlyingType = Enum.GetUnderlyingType(enumType);
                return ObjSrcAttributeUtility.EnumCompatibles.TryGet(underlyingType, out compatible);
            }
            catch when (enumType is null) { throw new ArgumentNullException(nameof(enumType)); }
            catch when (!enumType.IsEnum) { throw new ArgumentException($"{nameof(enumType)} must represent an enumeration.", nameof(enumType)); }
        }

        #endregion

        /// <summary>Constructor for <see cref="ObjSrcImportEnum"/></summary>
        /// <param name="name">Name used to identify the supported enumeration</param>
        /// <param name="enumType">Data type of the supported enumeration</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null
        /// <br/>or<br/>
        /// <paramref name="enumType"/> is null
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="enumType"/> does not represent an enumeration
        /// <br/>or<br/>
        /// The underlying type of <paramref name="enumType"/> is not supported
        /// </exception>
        /// 
        public ObjSrcImportEnum(string name, Type enumType)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            _Name = name;

            if (!TryGetEnumCompatible_m(enumType, out var compatible))
                throw new ArgumentException($"The underlying type {Enum.GetUnderlyingType(enumType).Name} is not supported.", nameof(enumType));
            _EnumType = enumType;
            _Compatible = compatible;
        }

        #region fields

        private readonly string _Name;
        private readonly Type _EnumType;
        private readonly ObjSrcEnumCompatible _Compatible;

        #endregion

        #region properties

        /// <summary>Name used to identify the supported enumeration</summary>
        public string Name => _Name;

        /// <summary>Data type of the supported enumeration</summary>
        public Type EnumType => _EnumType;

        #endregion

        #region method

        /// <inheritdoc cref="ObjSrcEnumCompatible.Create"/>
        internal IObjSrcEnumCompatible Create_m() => _Compatible.Create();

        #endregion

        /// <summary>Checks whether or not an enumeration of the specified type is supported</summary>
        /// <param name="enumType">Enumeration type</param>
        /// <returns>Whether or not an enumeration of the specified type is supported</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> does not represent an enumeration</exception>
        public static bool IsSupported(Type enumType)
        {
            return TryGetEnumCompatible_m(enumType, out _);
        }
    }
}
