namespace Plugin.DTLS.Enums;

public enum SignatureAlgorithm : byte
{
    Anonymous,
    RSA,
    DSA,
    ECDSA,
    MAX = 255
}
