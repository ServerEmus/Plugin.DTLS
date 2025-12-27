using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct ClientCertificateTypeExtension() : IExtension, ISize
{
    public List<CertificateType> CertificateTypes = [];
    public readonly ExtensionType Type => ExtensionType.ClientCertificateType;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => (ushort)(CertificateTypes.Count + sizeof(ushort) + sizeof(ushort));

    public readonly void Deserialize(EndiannessReader reader)
    {
        CertificateTypes.Clear();
        uint count = reader.ReadUInt16();
        for (int i = 0; i < count; i++)
        {
            CertificateType certificateType = (CertificateType)reader.ReadByte();
            CertificateTypes.Add(certificateType);
        }
    }

    public void Serialize(EndiannessWriter writer)
    {
        ushort size = (ushort)CertificateTypes.Count;
        ExtensionLength = (ushort)(size + sizeof(ushort));
        writer.Write(ExtensionLength);
        writer.Write((byte)CertificateTypes.Count);
        foreach (CertificateType curve in CertificateTypes)
        {
            writer.Write((byte)curve);
        }
    }

    public readonly override string ToString()
    {
        return $"CertificateTypes: {string.Join(", ", CertificateTypes)}";
    }
}
