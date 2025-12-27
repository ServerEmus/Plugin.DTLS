using Plugin.DTLS.Enums;
using Plugin.DTLS.Interfaces;
using ServerShared.IO;

namespace Plugin.DTLS.Extensions;

public struct ServerCertificateTypeExtension() : IExtension, ISize
{
    public CertificateType Certificate = CertificateType.X509;
    public readonly ExtensionType Type => ExtensionType.ServerCertificateType;
    public ushort ExtensionLength { get; set; }
    public readonly ushort Size => sizeof(CertificateType) + sizeof(ushort);

    public void Deserialize(EndiannessReader reader)
    {
        Certificate = (CertificateType)reader.ReadByte();
    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        writer.Write((byte)Certificate);
    }

    public readonly override string ToString()
    {
        return $"CertificateType: {Certificate}";
    }
}
