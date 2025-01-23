using System;
using System.Reflection;

namespace Objectoid.Source
{
    internal static partial class ObjSrcAttributeUtility
    {
        static ObjSrcAttributeUtility()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                ((ICollection_<string, ObjSrcReadableAttribute, ObjSrcReadable>)Readables).TryAdd(
                    type.GetCustomAttribute<ObjSrcReadableAttribute>(false), type);
                ((ICollection_<Type, ObjSrcDecodableAttribute, ObjSrcDecodable>)Decodables).TryAdd(
                    type.GetCustomAttribute<ObjSrcDecodableAttribute>(false), type);
            }
        }

        public static ObjSrcReadableCollection Readables { get; } = new ObjSrcReadableCollection();
        public static ObjSrcDecodableCollection Decodables { get; } = new ObjSrcDecodableCollection();
    }
}
