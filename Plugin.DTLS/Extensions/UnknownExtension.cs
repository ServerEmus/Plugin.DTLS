using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct UnknownExtension() : IExtension, ISize
{
    public byte[] Data = [];
    public ExtensionType Type { get; set; } = ExtensionType.Unknown;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)(Data.Length + sizeof(ushort));

    public void Deserialize(EndiannessReader reader)
    {
        Data = reader.ReadBytes(ExtensionLength);
    }

    public void Serialize(EndiannessWriter writer)
    {
        ExtensionLength = (ushort)Data.Length;
        writer.Write(ExtensionLength);
        writer.Write(Data);
    }

    public readonly override string ToString()
    {
        return $"{Type} {ExtensionLength}";
    }
}
