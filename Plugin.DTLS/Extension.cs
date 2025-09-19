using Plugin.DTLS.Enums;

namespace Plugin.DTLS;

internal static class Extension
{
    public static DtlsRecordType GetRecordType(byte b) =>
        b switch
        {
            (byte)ContentType.Alert => DtlsRecordType.PlainText,
            (byte)ContentType.Handshake => DtlsRecordType.PlainText,
            (byte)ContentType.Ack => DtlsRecordType.PlainText,
            var hdr => (hdr & 0b_0010_0000) != 0 ? DtlsRecordType.CipherText : DtlsRecordType.Invalid
        };
}
