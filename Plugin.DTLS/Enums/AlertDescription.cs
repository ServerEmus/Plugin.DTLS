namespace Plugin.DTLS.Enums;

public enum AlertDescription : byte
{
    CloseNotify = 0,
    Unexpected_message = 10,
    BadRecordMac = 20,
    DecryptionFailed = 21, //RESERVED
    RecordOverflow = 22,
    DecompressionFailure = 30,
    HandshakeFailure = 40,
    NoCertificate = 41, //RESERVED
    BadCertificate = 42,
    UnsupportedCertificate = 43,
    CertificateRevoked = 44,
    CertificateExpired = 45,
    CertificateUnknown = 46,
    IllegalParameter = 47,
    UnknownCA = 48,
    AccessDenied = 49,
    DecodeError = 50,
    DecryptError = 51,
    ExportRestriction = 60, //RESERVED
    ProtocolVersion = 70,
    InsufficientSecurity = 71,
    InternalError = 80,
    UserCanceled = 90,
    NoRenegotiation = 100,
    UnsupportedExtension = 110,
}
