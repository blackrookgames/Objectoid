using System;
using System.Linq;
using Executioner;
using Objectoid;

namespace test
{
    internal partial class Cmd_encode
    {
        private static readonly Dictionary<ObjType, Func<string, ObjElement>> _CreateValuableElementMethods =
            new Dictionary<ObjType, Func<string, ObjElement>>
            {
                { ObjType.NullTerminatedString, s => {
                    try { return new ObjNTStringElement(new ObjNTString(s)); }
                    catch { throw new CommandFailedException($"\"{s}\" is not a valid null-terminated string value."); }
                }},
                { ObjType.String, s => {
                    return new ObjStringElement(s);
                }},
                { ObjType.RawBytes, s => {
                    try { return new ObjRawBytesElement(from ss in s.Split(_WhitespaceChars, StringSplitOptions.RemoveEmptyEntries) select byte.Parse(ss)); }
                    catch { throw new CommandFailedException($"\"{s}\" is not valid raw byte data."); }
                }},
                { ObjType.UInt8, s => {
                    if (!byte.TryParse(s, out byte value))
                        throw new CommandFailedException($"\"{s}\" is not a valid unsigned 8-bit integer value.");
                    return new ObjUInt8Element(value);
                }},
                { ObjType.Int8, s => {
                    if (!sbyte.TryParse(s, out sbyte value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 8-bit integer value.");
                    return new ObjInt8Element(value);
                }},
                { ObjType.UInt16, s => {
                    if (!ushort.TryParse(s, out ushort value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 16-bit integer value.");
                    return new ObjUInt16Element(value);
                }},
                { ObjType.Int16, s => {
                    if (!short.TryParse(s, out short value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 16-bit integer value.");
                    return new ObjInt16Element(value);
                }},
                { ObjType.UInt32, s => {
                    if (!uint.TryParse(s, out uint value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 32-bit integer value.");
                    return new ObjUInt32Element(value);
                }},
                { ObjType.Int32, s => {
                    if (!int.TryParse(s, out int value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 32-bit integer value.");
                    return new ObjInt32Element(value);
                }},
                { ObjType.UInt64, s => {
                    if (!ulong.TryParse(s, out ulong value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 64-bit integer value.");
                    return new ObjUInt64Element(value);
                }},
                { ObjType.Int64, s => {
                    if (!long.TryParse(s, out long value))
                        throw new CommandFailedException($"\"{s}\" is not a valid signed 64-bit integer value.");
                    return new ObjInt64Element(value);
                }},
                { ObjType.Single, s => {
                    if (!float.TryParse(s, out float value))
                        throw new CommandFailedException($"\"{s}\" is not a valid single-precision floating-point value.");
                    return new ObjSingleElement(value);
                }},
                { ObjType.Double, s => {
                    if (!double.TryParse(s, out double value))
                        throw new CommandFailedException($"\"{s}\" is not a valid double-precision floating-point value.");
                    return new ObjDoubleElement(value);
                }},
                { ObjType.Bool, s => {
                    if (!bool.TryParse(s, out bool value))
                        throw new CommandFailedException($"\"{s}\" is not a valid boolean value.");
                    return new ObjBoolElement(value);
                }},
            };
    }
}

