using System;
using System.IO;
using Executioner;
using Executioner.Utility;
using Objectoid;

namespace test
{
    [Command(Path = "decode", Description = "Decodes the specified Objectoid document to an JSON document")]
    internal class Cmd_decode : Command
    {
        [RequiredParameter(Description = "Input *.objectoid Document")]
        private string input;

        [OptionalParameter(Description = "Output *.json Document")]
        private string output;

        protected override void Execute()
        {
            CompletePath inputPath = CompletePath.Validate(input);
            input = (string)inputPath;
            
            CompletePath outputPath = (output == null) ?
                new CompletePath(input + ".json") : CompletePath.Validate(output);
            output = (string)outputPath;

            if (!File.Exists(input))
                throw new CommandFailedException($"Could not find the file \"{inputPath}\".");

            ObjDocument objDocument = new ObjDocument();
            try
            {
                using (Stream stream = File.OpenRead(input))
                {
                    objDocument.Load(stream);
                }
            }
            catch (Exception e)
            {
                throw new CommandFailedException($"Could read the file \"{inputPath}\". {e.Message}");
            }

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(output))
                {
                    void writeChar(char ch)
                    {
                        switch (ch)
                        {
                            case '\b': streamWriter.Write("\\b"); break;
                            case '\f': streamWriter.Write("\\f"); break;
                            case '\n': streamWriter.Write("\\n"); break;
                            case '\r': streamWriter.Write("\\r"); break;
                            case '\t': streamWriter.Write("\\t"); break;
                            case '\"': streamWriter.Write("\\\""); break;
                            case '\\': streamWriter.Write("\\\\"); break;
                            default: streamWriter.Write(ch); break;
                        }
                    }
                    void writeNTString(ObjNTString s)
                    {
                        //Write characters
                        foreach (byte c in s)
                            writeChar((char)c);
                    }
                    void writeString(string s)
                    {
                        //Write characters
                        foreach (char c in s)
                            writeChar(c);
                    }

                    void printElement(ObjElement element, int indent = 0, bool addComma = true)
                    {
                        string indentStr = new string(' ', indent * 2);
                        //Document object
                        if (element is ObjDocObject)
                        {
                            streamWriter.WriteLine('{');
                            var _element = (ObjDocObject)element;
                            foreach (ObjDocObjectProperty property in _element)
                            {
                                streamWriter.Write($"{indentStr}  \"");
                                writeNTString(property.Name);
                                streamWriter.Write($"\": ");
                                printElement(property.Value, indent + 1);
                            }
                            streamWriter.Write($"{indentStr}}}");
                        }
                        //List
                        else if (element is ObjList)
                        {
                            streamWriter.WriteLine('[');
                            var _element = (ObjList)element;
                            for (int i = 0; i < _element.Count; i++)
                            {
                                streamWriter.Write($"{indentStr}  ");
                                printElement(_element[i], indent + 1);
                            }
                            streamWriter.Write($"{indentStr}]");
                        }
                        //If null-terminated string
                        else if (element is ObjNTStringElement)
                        {
                            var _element = (ObjNTStringElement)element;
                            //Get value
                            ObjNTString s = _element.Value;
                            streamWriter.Write($"\"/{element.Type} ");
                            writeNTString(s);
                            streamWriter.Write("\"");
                        }
                        //If string
                        else if (element is ObjStringElement)
                        {
                            var _element = (ObjStringElement)element;
                            string s = _element.Value;
                            streamWriter.Write("\"");
                            if (s.Length > 0 && s[0] == '/') streamWriter.Write('/');
                            writeString(s);
                            streamWriter.Write("\"");
                        }
                        //If boolean
                        else if (element is ObjBoolElement)
                        {
                            var _element = (ObjBoolElement)element;
                            streamWriter.Write($"{_element.Value.ToString().ToLower()}");
                        }
                        //If valuable
                        else if (element is IObjValuable)
                        {
                            var _element = (IObjValuable)element;
                            streamWriter.Write($"\"/{element.Type} {_element.Value}\"");
                        }
                        //If raw byte data
                        else if (element is ObjRawBytesElement)
                        {
                            var _element = (ObjRawBytesElement)element;
                            streamWriter.Write($"\"/{element.Type}");
                            foreach (byte b in _element)
                                streamWriter.Write($" {b}");
                            streamWriter.Write($"\"");
                        }
                        //Assume null
                        else
                        {
                            streamWriter.Write("null");
                        }
                        //Comma
                        if (addComma) streamWriter.WriteLine(',');
                    }
                    printElement(objDocument.RootObject, addComma: false);
                }
            }
            catch (Exception e)
            {
                throw new CommandFailedException($"Could not save to \"{outputPath}\". {e.Message}");
            }
        }
    }
}
