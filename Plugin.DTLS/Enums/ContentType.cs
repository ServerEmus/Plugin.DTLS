namespace Plugin.DTLS.Enums;

public enum ContentType : byte
{
    ChangeCipherSpec = 20,
    Alert = 21,
    Handshake = 22,
    ApplicationData = 23,
    Hearthbeat = 24,
    Tls12Cid = 25,
    Ack = 26,
    ReturnRoutabilityCheck = 27,
}
