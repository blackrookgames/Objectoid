using System;
using System.IO;
using System.Text;

namespace Objectoid.Source
{
    internal enum ObjSrcReaderState
    {
        /// <summary>Reader has not started reading</summary>
        Start = -1,
        /// <summary>Reader is reading</summary>
        Reading,
        /// <summary>Reader has reached the end of the stream</summary>
        End,
    }
}
