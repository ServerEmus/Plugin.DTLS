using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct EllipticCurvePointFormatsExtension() : IExtension
{
    public List<short> PointFormats = [];
    public readonly ExtensionType Type => ExtensionType.EllipticCurvePointFormats;

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        PointFormats.Clear();
        long len = reader.ReadUInt16();
        long endLen = reader.BaseStream.Position + len;
        while (len != endLen)
        {
            PointFormats.Add(reader.ReadInt16());
            len = reader.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        if (!PointFormats.Contains(0) || (PointFormats.Count > 1 && PointFormats[0] != 0) )
        {
            short temp = 0;
            if (PointFormats.Count > 1)
                temp = PointFormats[0];
            PointFormats.Insert(0, 0);
            if (temp != 0)
                PointFormats.Add(temp);
        }

        using MemoryStream memoryStream = new();
        BinaryWriterBig writerBig = new(memoryStream);

        foreach (short curve in PointFormats)
        {
            writerBig.Write(curve);
        }

        writer.Write((uint)memoryStream.Length);
        writer.Write(memoryStream.ToArray());
    }

    public readonly override string ToString()
    {
        return $"\nPointFormats: {string.Join(", ", PointFormats)}";
    }
}
