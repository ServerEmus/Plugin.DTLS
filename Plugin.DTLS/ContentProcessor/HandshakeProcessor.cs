using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.Records;
using Serilog;
using ServerShared.IO;

namespace Plugin.DTLS.ContentProcessor;

internal class HandshakeProcessor : IContentProcessor
{
    public ContentType Type => ContentType.Handshake;

    public void Process(DtlsSession session, ref DTLSPlaintext record, in IContent content)
    {
        if (content is not HandshakeHeader handshake)
            return;
        IHandshake? ihandshake = MainStorage.GetHandshake(handshake.HandshakeType);
        if (ihandshake == null)
        {
            Log.Information("IHandshake type not found: {HandshakeType}", handshake.HandshakeType);
            return;
        }
        using BinaryReaderBig plainTextReader = new(new MemoryStream(handshake.Payload));
        ihandshake.Deserialize(plainTextReader);
        Log.Information("Processing IHandshake: {ihandshake}", ihandshake);
        MainStorage.ProcessHandshake(session, ref record, ref handshake, ihandshake);
    }
}
