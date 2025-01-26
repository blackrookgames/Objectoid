using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcElementException : ObjSrcException
    {
        #region helper

        /// <summary>Determines the path to the specified element</summary>
        /// <param name="element">Element</param>
        /// <returns>Path to the specified element</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        private static string DeterminePath_m(ObjElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            var path = new StringBuilder();
            while (true)
            {
                //Object
                if (element.Collection is ObjDocObject)
                {
                    var collection = (ObjDocObject)element.Collection;
                    foreach (var property in collection)
                    {
                        if (element != property.Value) continue;
                        path.Insert(0, property.Name);
                        break;
                    }
                    goto next;
                }
                //List
                if (element.Collection is ObjList)
                {
                    var collection = (ObjList)element.Collection;
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (element == collection[i]) continue;
                        path.Insert(0, i);
                        break;
                    }
                    goto next;
                }
            //Next
            next:
                element = element.Collection;
                if (element is null) break;
                if (element is ObjRootObject) break;
                path.Insert(0, '/');
            }
            return path.ToString();
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcElementException"/></summary>
        /// <param name="element">Element related to the error</param>
        /// <param name="message">Message explaining the error</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
        public ObjSrcElementException(ObjElement element, string message) : base()
        {
            try
            {
                Message = $"{message}{((element.Collection is null) ? "" : $"  {DeterminePath_m(element)}.")}";
                BaseMessage_p = message;
            }
            catch when (element is null) { throw new ArgumentNullException(nameof(element)); }
        }

        /// <inheritdoc/>
        public override string Message { get; }

        /// <inheritdoc/>
        internal override string BaseMessage_p { get; }

        /// <summary>Element that caused the error</summary>
        public ObjElement Element { get; }
    }
}
