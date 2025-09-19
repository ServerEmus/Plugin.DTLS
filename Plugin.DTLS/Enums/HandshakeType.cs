namespace Plugin.DTLS.Enums;

public enum HandshakeType : byte
{
    HelloRequest = 0,
	ClientHello = 1,
	ServerHello = 2,
	HelloVerifyRequest = 3,
    NewSessionTicket = 4,
	EndOfEarlyData = 5,
	HelloRetryRequest = 6,
	EncryptedExtensions = 8,
	Certificate = 11,
	ServerKeyExchange = 12,
	CertificateRequest = 13,
	ServerHelloDone = 14,
	CertificateVerify = 15,
	ClientKeyExchange = 16,
	Finished = 20,
	CertificateUrl = 21,
	CertificateStatus = 22,
	SupplimentalData = 23,
	KeyUpdate = 24,
	MessageHash = 254,
}
