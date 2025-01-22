using System;
using System.Collections.Generic;
using System.Reflection;

namespace Objectoid.Source
{
    internal static class ObjSrcValidElements
    {
        static ObjSrcValidElements()
        {
            _ValidElements = new Dictionary<string, ObjSrcValidElement>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!ObjSrcValidElement.TryCreate(type, out var validElement))
                    continue;
                _ValidElements.TryAdd(validElement.Attribute.Keyword, validElement);
            }
        }

        private static readonly Dictionary<string, ObjSrcValidElement> _ValidElements;

        /// <summary>Attempts to get the valid element with the specified keyword</summary>
        /// <param name="keyword">Keyword of the element</param>
        /// <param name="validElement">Found element</param>
        /// <returns>Whether or not successful</returns>
        /// <exception cref="ArgumentNullException"><paramref name="keyword"/> is null</exception>
        public static bool TryGet(string keyword, out ObjSrcValidElement validElement)
        {
            try { return _ValidElements.TryGetValue(keyword, out validElement); }
            catch when (keyword is null) { throw new ArgumentNullException(nameof(keyword)); }
        }
    }
}
