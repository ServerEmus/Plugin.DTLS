using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct EllipticCurvePointFormatsExtension() : IExtension, ISize
{
    public List<byte> PointFormats = [];
    public readonly ExtensionType Type => ExtensionType.EllipticCurvePointFormats;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)(PointFormats.Count + sizeof(ushort) + sizeof(ushort));
    public readonly void Deserialize(BinaryReaderBig reader)
    {
        PointFormats.Clear();
        int formatsLen = reader.ReadByte();
        for (int i = 0; i < formatsLen; i++)
        {
            PointFormats.Add(reader.ReadByte());
        }
    }

    public void Serialize(BinaryWriterBig writer)
    {
        ushort size = (ushort)PointFormats.Count;
        ExtensionLength = (ushort)(size + sizeof(ushort));
        writer.Write(ExtensionLength);
        writer.Write((byte)PointFormats.Count);
        foreach (byte curve in PointFormats)
        {
            writer.Write(curve);
        }
    }

    public readonly override string ToString()
    {
        return $"\nPointFormats: {string.Join(", ", PointFormats)}";
    }
}
