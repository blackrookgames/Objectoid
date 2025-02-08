using System.Text;
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
            ObjSrcDocument src = new ObjSrcDocument();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("@Object : \"test\" @List : @EndList : @EndObject")))
                src.Load(stream);
        }
    }
}