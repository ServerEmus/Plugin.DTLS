using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct EllipticCurvesExtension() : IExtension
{
    public List<short> EllipticCurves = [];
    public readonly ExtensionType Type => ExtensionType.EllipticCurves;

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        EllipticCurves.Clear();
        long len = reader.ReadUInt16();
        long endLen = reader.BaseStream.Position + len;
        while (len != endLen)
        {
            EllipticCurves.Add(reader.ReadInt16());
            len = reader.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        using MemoryStream memoryStream = new();
        BinaryWriterBig writerBig = new(memoryStream);
        foreach (short curve in EllipticCurves)
        {
            writerBig.Write(curve);
        }

        writer.Write((uint)memoryStream.Length);
        writer.Write(memoryStream.ToArray());
    }

    public readonly override string ToString()
    {
        return $"\nEllipticCurves: {string.Join(", ", EllipticCurves)}";
    }
}
