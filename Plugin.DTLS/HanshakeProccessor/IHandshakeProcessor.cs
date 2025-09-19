using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.Records;

namespace Plugin.DTLS.HanshakeProccessor;

public interface IHandshakeProcessor
{
    public HandshakeType Type { get; }

    public void Process(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, in IHandshake handshake);
}
