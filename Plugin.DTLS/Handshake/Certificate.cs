using Plugin.DTLS.Enums;
using ServerShared.IO;
using ServerShared.Types;
using System.Security.Cryptography.X509Certificates;

namespace Plugin.DTLS.Handshake;

public struct Certificate() : IHandshake
{
    public readonly byte[]? Cert
    {
        get
        {
            if (CertChain.Count == 0)
                return null;
            return CertChain[0];
        }
    }

    public List<byte[]> CertChain = [];
    public CertificateType CertificateType;
    public readonly HandshakeType Type =>  HandshakeType.Certificate;

    public readonly void Deserialize(EndiannessReader reader)
    {
        CertChain.Clear();
        var certChainLen = reader.ReadInt24();
        switch (CertificateType)
        {
            case CertificateType.X509:
                {
                    while (certChainLen > 0)
                    {
                        var certificateLength = reader.ReadInt24();
                        byte[] certificate = reader.ReadBytes(certificateLength);
                        CertChain.Add(certificate);
                        certChainLen -= certificateLength - 3;
                    }
                }
                return;
            case CertificateType.OpenPGP:
                return;
            case CertificateType.RawPublicKey:
                return;
            default:
                return;
        }

    }

    public readonly void Serialize(EndiannessWriter writer)
    {
        Int24 len = 3;
        switch (CertificateType)
        {
            case CertificateType.X509:
                {
                    foreach (var chain in CertChain)
                        len += 3 + chain.Length;
                    writer.WriteInt24(len);
                    foreach (var chain in CertChain)
                    {
                        writer.WriteInt24(chain.Length);
                        writer.Write(chain);
                    }
                }
                return;
            case CertificateType.OpenPGP:
                return;
            case CertificateType.RawPublicKey:
                return;
            default:
                return;
        }
    }
}
