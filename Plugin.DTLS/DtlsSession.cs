using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.Records;
using Serilog;
using ServerShared.IO;
using ServerShared.Server;

namespace Plugin.DTLS;

public class DtlsSession(CoreSslUdpSession session)
{
    public ushort Epoch { get; set; }
    public ProtocolVersion ProtocolVersion { get; set; } = ProtocolVersion.DefaultVersion;
    public Guid SessionID { get; set; }
    public CoreSslUdpSession Session => session;
    public ushort ClientEpoch { get; set; }
    public long ClientSequenceNumber { get; set; }

    public void Send(ref DTLSPlaintext record, ref HandshakeHeader content, ref IHandshake handshake)
    {

        MemoryStream mem = new();
        BinaryWriterBig writerBig = new(mem);
        handshake.Serialize(writerBig);
        writerBig.Flush();
        Log.Information("Sending IHandshake: {IHandshake}", handshake);
        content.HandshakeType = handshake.Type;
        content.Reset();
        content.Length = (ServerShared.Types.UInt24)mem.Length;
        content.FragmentLength = (ServerShared.Types.UInt24)mem.Length;
        content.Payload = mem.ToArray();
        mem = new();
        writerBig = new(mem);
        content.Serialize(writerBig);
        writerBig.Flush();
        Log.Information("Sending HandshakeHeader: {content}", content);
        record.Fragment = mem.ToArray();
        mem = new();
        writerBig = new(mem);
        record.Serialize(writerBig);
        writerBig.Flush();
        Log.Information("Sending DTLSPlaintext: {record}", record);
        Session.Send(mem.ToArray());
    }

}
