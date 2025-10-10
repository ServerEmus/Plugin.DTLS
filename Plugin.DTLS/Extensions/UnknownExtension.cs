using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct UnknownExtension() : IExtension
{
    public byte[] Data = [];
    public ExtensionType Type { get; set; } = ExtensionType.Unknown;

    public void Deserialize(BinaryReaderBig reader)
    {
        long len = reader.ReadUInt16();
        Data = reader.ReadBytes((int)len);
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        writer.Write((uint)Data.Length);
        writer.Write(Data);
    }

    public readonly override string ToString()
    {
        return $"{Type}";
    }
}
