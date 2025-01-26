using System.Reflection;
using Objectoid;
using Objectoid.Source;
using Objectoid.Source.ElementUtility;
using Rookie;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var document = new ObjDocument();
            document.RootObject.Add(new ObjNTString("A"), new ObjNullElement());
            Console.WriteLine(document.RootObject.GetElement<ObjNullElement>(new ObjNTString("A")));
        }
    }
}