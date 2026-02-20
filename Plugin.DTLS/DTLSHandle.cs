using Plugin.DTLS.Records;
using Serilog;
using ServerShared.EventArguments;
using ServerShared.IO;
using ServerShared.Server;

namespace Plugin.DTLS;

internal class DTLSHandle
{
    public static Dictionary<Guid, DtlsSession> Sessions = [];

    public static void CoreSslUdpSession_OnBytesReceived(object? sender, SessionBytesReceivedEventArgs args)
    {
        CoreSslUdpSession Session = (CoreSslUdpSession)args.Session!;
        if (args.Data.Length < 1)
            return;

        DtlsSession? dtlsSession;
        if (!Sessions.TryGetValue(Session.Id, out var _dtls))
        {
            var config = DTLSPlugin.PluginConfig.Servers.FirstOrDefault(server => server.Port == Session.Server.Port);
            if (config == null)
                return;

            dtlsSession = new(Session, config);
        }
        else
        {
            dtlsSession = _dtls;
        }
            

        using BinaryReaderBig mainReader = new(new MemoryStream(args.Data.ToArray()));
        Common common = Common.Parse(mainReader);
        Log.Information("RecordType: {Type}, ContentType: {Content}", common.RecordType, common.ContentType);
        IRecord? record = MainStorage.GetRecord(common.RecordType);
        MainStorage.ProcessRecord(dtlsSession, ref record, mainReader);
    }
}
