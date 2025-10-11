using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct EllipticCurvesExtension() : IExtension, ISize
{
    public List<EllipticCurve> EllipticCurves = [];
    public readonly ExtensionType Type => ExtensionType.EllipticCurves;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)((EllipticCurves.Count * sizeof(EllipticCurve)) + sizeof(ushort) + sizeof(ushort));
    public readonly void Deserialize(BinaryReaderBig reader)
    {
        EllipticCurves.Clear();
        uint count = (uint)(reader.ReadUInt16() / sizeof(EllipticCurve));
        for (int i = 0; i < count; i++)
        {
            EllipticCurve curve = (EllipticCurve)reader.ReadUInt16();
            EllipticCurves.Add(curve);
        }
    }

    public void Serialize(BinaryWriterBig writer)
    {
        ushort size = (ushort)(EllipticCurves.Count * sizeof(EllipticCurve));
        ExtensionLength = (ushort)(size + sizeof(ushort));
        writer.Write(ExtensionLength);
        writer.Write(size);
        foreach (EllipticCurve curve in EllipticCurves)
        {
            writer.Write((ushort)curve);
        }
    }

    public readonly override string ToString()
    {
        return $"\nEllipticCurves: {string.Join(", ", EllipticCurves)}";
    }
}
