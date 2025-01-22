using System.Reflection;
using Objectoid.Source;
using Rookie;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var srcDocument = new ObjSrcDocument();
            using (var stream = File.OpenRead("test.objsrc"))
                srcDocument.Load(stream);
            using (var stream = File.Open("test2.objsrc", FileMode.Truncate, FileAccess.Write))
                srcDocument.Save(stream);
        }
    }
}