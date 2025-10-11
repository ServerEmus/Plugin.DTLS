namespace Plugin.DTLS.Enums;

public enum CertificateType : byte
{
    X509,
    OpenPGP,
    RawPublicKey,
    MAX = byte.MaxValue
}
