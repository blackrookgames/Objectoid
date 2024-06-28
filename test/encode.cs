using System;
using System.IO;
using System.Text.Json;
using Executioner;
using Executioner.Utility;
using Objectoid;

namespace test
{
    [Command(Path = "encode", Description = "Encodes an Objectoid document from the specified JSON document")]
    internal partial class Cmd_encode : Command
    {
        [OptionalParameter(Description = "Input *.json Document")]
        private string input;

        [RequiredParameter(Description = "Output *.objectoid Document")]
        private string output;

        protected override void Execute()
        {
            CompletePath outputPath = CompletePath.Validate(output);
            output = (string)outputPath;

            CompletePath inputPath = (input == null) ?
                new CompletePath(output + ".json") : CompletePath.Validate(input);
            input = (string)inputPath;

            if (!File.Exists(input))
                throw new CommandFailedException($"Could not find the file \"{inputPath}\".");

            string rawText;
            try { rawText = File.ReadAllText(input); }
            catch { throw new CommandFailedException($"Could not read the file \"{inputPath}\"."); }

            JsonDocumentOptions options = new JsonDocumentOptions();
            options.AllowTrailingCommas = true;

            JsonDocument jsonDocument;
            try { jsonDocument = JsonDocument.Parse(rawText, options: options); }
            catch (JsonException e) { throw new CommandFailedException(e.Message); }

            ObjDocument objDocument = new ObjDocument();
            void readObject(ObjDocObject docObject, JsonElement jsonElement, string path = null)
            {
                CommandFailedException invalid(string message) =>
                    new CommandFailedException($"Path: {path}\r\nmessage");
                foreach (var jsonProperty in jsonElement.EnumerateObject())
                {
                    //Name
                    ObjNTString name;
                    try { name = new ObjNTString(jsonProperty.Name); }
                    catch { throw invalid($"\"{jsonProperty.Name}\" is not a valid name."); }
                    //Value
                    ObjElement element = read(jsonProperty.Value, $"{path}/{jsonProperty.Name}");
                    //Add
                    docObject.Add(name, element);
                }
            }
            ObjElement read(JsonElement jsonElement, string path)
            {
                CommandFailedException invalid(string message) =>
                    new CommandFailedException($"Path: {path}\r\nmessage");
                switch (jsonElement.ValueKind)
                {
                    //Object
                    case JsonValueKind.Object:
                        {
                            ObjDocObject docObject = new ObjDocObject();
                            readObject(docObject, jsonElement, path);
                            return docObject;
                        }
                    //Array
                    case JsonValueKind.Array:
                        {
                            ObjList list = new ObjList();
                            int index = -1;
                            foreach (var jsonEntry in jsonElement.EnumerateArray())
                            {
                                //Value
                                ObjElement element = read(jsonEntry, $"{path}/{(++index)}");
                                //Add
                                list.Add(element);
                            }
                            return list;
                        }
                    //String
                    case JsonValueKind.String:
                        {
                            string s = jsonElement.GetString();
                            //Look for forward slash
                            if (s.Length == 0 || s[0] != '/') return new ObjStringElement(s);
                            //Should we parse the string to another type?
                            if (s.Length > 1 && s[1] == '/') return new ObjStringElement(s);
                            //Yes, check read type
                            int space;
                            for (space = 0; space < s.Length; space++) { if (s[space] == ' ') break; }
                            string typeStr = s[1..space];
                            ObjType type;
                            if (!Enum.TryParse(typeStr, out type)) throw invalid(
                                $"The type \"{typeStr}\" is unknown.");
                            //Create element
                            if (_CreateValuableElementMethods.TryGetValue(type, out var createElement))
                                return createElement(s[(space + 1)..]);
                            throw invalid(
                                $"Cannot create an element of the type {type} from a raw string.");
                        }
                    //Number
                    case JsonValueKind.Number:
                        {
                            if (jsonElement.TryGetInt32(out int intValue))
                                return new ObjInt32Element(intValue);
                            if (jsonElement.TryGetSingle(out float floatValue))
                                return new ObjSingleElement(floatValue);
                            if (jsonElement.TryGetInt64(out long longValue))
                                return new ObjInt64Element(longValue);
                            if (jsonElement.TryGetDouble(out double doubleValue))
                                return new ObjDoubleElement(doubleValue);
                            if (jsonElement.TryGetUInt64(out ulong ulongValue))
                                return new ObjUInt64Element(ulongValue);
                            throw invalid($"Could not determine the type of the number \"{jsonElement.GetRawText()}\".");
                        }
                    //True
                    case JsonValueKind.True:
                        {
                            return new ObjBoolElement(true);
                        }
                    //False
                    case JsonValueKind.False:
                        {
                            return new ObjBoolElement(false);
                        }
                    //Assume null
                    default:
                        {
                            return new ObjNullElement();
                        }
                }
            }
            readObject(objDocument.RootObject, jsonDocument.RootElement);

            try
            {
                using (Stream stream = File.OpenWrite(output))
                {
                    objDocument.Save(stream, false, false);
                }
            }
            catch (Exception e)
            {
                throw new CommandFailedException($"Could not save to \"{outputPath}\". {e.Message}");
            }
        }
    }
}
