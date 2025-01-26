using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal class ObjSrcSrcElementException : ObjSrcException
    {
        #region helper

        /// <summary>Determines the path to the specified source element</summary>
        /// <param name="srcElement">Source element</param>
        /// <returns>Path to the specified source element</returns>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        private static string DeterminePath_m(ObjSrcElement srcElement)
        {
            if (srcElement is null)
                throw new ArgumentNullException(nameof(srcElement));

            var path = new StringBuilder();
            while (true)
            {
                //Object/Import
                if (srcElement.Collection is ObjSrcObject)
                {
                    var collection = (ObjSrcObject)srcElement.Collection;
                    foreach (var property in collection)
                    {
                        if (srcElement != property.Value) continue;
                        path.Insert(0, property.Name);
                        break;
                    }
                    goto next;
                }
                //List
                if (srcElement.Collection is ObjSrcList)
                {
                    var collection = (ObjSrcList)srcElement.Collection;
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (srcElement == collection[i]) continue;
                        path.Insert(0, i);
                        break;
                    }
                    goto next;
                }
            //Next
            next:
                srcElement = srcElement.Collection;
                if (srcElement is null) break;
                if (srcElement is ObjSrcRoot) break;
                path.Insert(0, '/');
            }
            return path.ToString();
        }

        #endregion

        /// <summary>Creates an instance of <see cref="ObjSrcSrcElementException"/></summary>
        /// <param name="srcElement">Source element related to the error</param>
        /// <param name="message">Message explaining the error</param>
        /// <exception cref="ArgumentNullException"><paramref name="srcElement"/> is null</exception>
        public ObjSrcSrcElementException(ObjSrcElement srcElement, string message) : base()
        {
            try
            {
                Message = $"{message}{((srcElement.Collection is null) ? "" : $"  {DeterminePath_m(srcElement)}.")}.";
                BaseMessage_p = message;
            }
            catch when (srcElement is null) { throw new ArgumentNullException(nameof(srcElement)); }
        }

        /// <inheritdoc/>
        public override string Message { get; }

        /// <inheritdoc/>
        internal override string BaseMessage_p { get; }

        /// <summary>Source element that caused the error</summary>
        public ObjSrcElement SrcElement { get; }
    }
}
