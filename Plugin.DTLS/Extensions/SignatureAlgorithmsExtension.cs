using Plugin.DTLS.Enums;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct SignatureAlgorithmsExtension() : IExtension
{
    public List<short> SignatureAlgorithms = [];
    public readonly ExtensionType Type => ExtensionType.SignatureAlgorithms;

    public readonly void Deserialize(BinaryReaderBig reader)
    {
        SignatureAlgorithms.Clear();
        long len = reader.ReadUInt16();
        long endLen = reader.BaseStream.Position + len;
        while (len != endLen)
        {
            SignatureAlgorithms.Add(reader.ReadInt16());
            len = reader.BaseStream.Position;
        }
    }

    public readonly void Serialize(BinaryWriterBig writer)
    {
        using MemoryStream memoryStream = new();
        BinaryWriterBig writerBig = new(memoryStream);
        foreach (short curve in SignatureAlgorithms)
        {
            writerBig.Write(curve);
        }

        writer.Write((uint)memoryStream.Length);
        writer.Write(memoryStream.ToArray());
    }

    public readonly override string ToString()
    {
        return $"\nSignatureAlgorithms: {string.Join(", ", SignatureAlgorithms)}";
    }
}
