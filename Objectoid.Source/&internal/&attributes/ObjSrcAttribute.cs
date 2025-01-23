using System;
using System.Collections.Generic;
using System.Reflection;

namespace Objectoid.Source
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal abstract class ObjSrcAttribute : Attribute { }
}
