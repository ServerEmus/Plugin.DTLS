using Plugin.DTLS.Records;
using Serilog;
using ServerShared.EventArguments;
using ServerShared.IO;
using ServerShared.Server;

namespace Plugin.DTLS;

internal class DtlHandler
{
    public static Dictionary<Guid, DtlsSession> Sessions = [];

    public static void CoreSslUdpSession_OnBytesReceived(object? sender, SessionBytesReceivedEventArgs args)
    {
        CoreSslUdpSession Session = (CoreSslUdpSession)args.Session!;
        if (Session.Server.Port != DTLSPlugin.PORT)
            return;
        if (args.Data.Length < 1)
            return;
        DtlsSession? dtlsSession;
        if (Sessions.TryGetValue(Session.Id, out var _dtls))
            dtlsSession = _dtls;
        else
            dtlsSession = new(Session);
        using BinaryReaderBig mainReader = new(new MemoryStream(args.Data.ToArray()));
        Common common = Common.Parse(mainReader);
        Log.Information("RecordType: {Type}, ContentType: {Content}", common.RecordType, common.ContentType);
        IRecord? record = MainStorage.GetRecord(common.RecordType);
        MainStorage.ProcessRecord(dtlsSession, ref record, mainReader);
        /*
        IRecord? processed = 
        if (processed == null)
            return;
        using MemoryStream ms = new();
        using BinaryWriterBig writer = new(ms);
        processed.Serialize(writer);
        Session.Send(ms.ToArray());
        /*
        common.ReadRecord(mainReader);
        Log.Information("Readed Record: {record}", common.GetRecord());

        if (common.GetRecord() is DTLSPlaintext plaintext)
        {
            using BinaryReaderBig plainTextReader = new(new MemoryStream(plaintext.Fragment));
            if (common.GetContent() is IBigSerializable serializable)
            {
                serializable.Deserialize(plainTextReader);
            }
            Log.Information("Readed Content: {Content}", common.GetContent());

            if (common.GetContent() is HandshakeHeader handshake)
            {
                handshake.GetHandshake()?.Deserialize(plainTextReader);
                Log.Information("GetHandshake Handshake: {Handshake}", handshake.GetHandshake());
            }
        }
        */
    }
}
