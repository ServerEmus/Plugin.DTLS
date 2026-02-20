using Serilog;
using ServerShared.Controllers;
using ServerShared.Plugins;
using ServerShared.Server;
using System.Net.Security;

namespace Plugin.DTLS;

public class DTLSPlugin : ServerPlugin<DTLSPluginConfig>
{
    public static DTLSPluginConfig PluginConfig { get; set; } = default!;
    public override uint Priority => 0;

    public override string Name => "DTLS Plugin";

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        for (int i = 0; i < Config.Servers.Count; i++)
        {
            var serverConfig = Config.Servers[i];
            if (serverConfig.SupporterCipherSuites.Count == 0)
            {
                Log.Warning("You did not declare any Cipher suites to use! Falling back to TLS_NULL_WITH_NULL_NULL (Index: {i})", i);
                serverConfig.SupporterCipherSuites.Add(TlsCipherSuite.TLS_NULL_WITH_NULL_NULL);
            }

            if (string.IsNullOrEmpty(serverConfig.PemFilePath))
            {
                Log.Warning("Pem File path is empty! IF you declared anything other than using PSK you did something wrong! (Index: {i})", i);
            }

            if (serverConfig.PSKIdentities.Count == 0)
            {
                Log.Warning("PSKIdentities is empty! IF you use any PSK with Cipher Suites you did something wrong! (Index: {i})", i);
            }
        }
    }

    public override void Start()
    {
        PluginConfig = Config;
        foreach (var item in Config.Servers)
        {
            if (ServerController.IsPortUsed(item.Port))
            {
                Log.Information($"DTLS Port already used! Port: {item.Port}");
                continue;
            }

            ServerController.Start(new ServerShared.CommonModels.ServerModel()
            {
                Name = $"DTLS {item.Port}",
                Port = item.Port,
                IsSecure = true,
                IsUdp = true,
            });
        }

        CoreSslUdpSession.OnBytesReceived += DTLSHandle.CoreSslUdpSession_OnBytesReceived;
    }    

    public override void Stop()
    {
        CoreSslUdpSession.OnBytesReceived -= DTLSHandle.CoreSslUdpSession_OnBytesReceived;
    }
}
