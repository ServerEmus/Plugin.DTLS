using Plugin.DTLS.Extensions;
using ServerShared.IO;

namespace DTLS.Test;

public static class TestHelpers
{
    public static (T ext, ushort extLen, ushort readLen) SerAndDeserExtension<T>(this T extension) where T : IExtension, new()
    {
        using MemoryStream ms = new();
        using BinaryWriterBig writerBig = new(ms);
        extension.Serialize(writerBig);
        ushort extensionLen = extension.ExtensionLength;
        using MemoryStream readms = new(ms.ToArray());
        using BinaryReaderBig readerBig = new(readms);
        ushort len = readerBig.ReadUInt16();

        T newExt = readerBig.ReadSerializable<T>();
        newExt.ExtensionLength = len;
        return (newExt, extensionLen, len);
    }

    public static (T ser, long writeLen, long readLen) SerAndDeser<T>(this T ser) where T : ICustomSerializable, new()
    {
        using MemoryStream ms = new();
        using BinaryWriterBig writerBig = new(ms);
        ser.Serialize(writerBig);
        using MemoryStream readms = new(ms.ToArray());
        using BinaryReaderBig readerBig = new(readms);
        T newExt = readerBig.ReadSerializable<T>();
        return (newExt, ms.Length, readms.Position);
    }
}
