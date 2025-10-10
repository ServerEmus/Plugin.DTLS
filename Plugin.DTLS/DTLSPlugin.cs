using ModdableWebServer.Helper;
using Plugin.DTLS.Cert;
using Serilog;
using ServerShared.Controllers;
using ServerShared.Plugins;
using ServerShared.Server;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Plugin.DTLS;

public class DTLSPlugin : ServerPlugin
{
    public const int PORT = 5684;
    public override uint Priority => 0;

    public override string Name => "DTLS Plugin";

    public override void Start()
    {
        if (ServerController.IsPortUsed(PORT))
            return;
        /*
        string certPath = Path.Combine(Directory.GetCurrentDirectory(), "Cert");
        var ecdsa = ECDsa.Create();
        string dtls_pfx = Path.Combine(certPath, "dtls.pfx");
        X509Certificate2? cert = null;
        if (File.Exists(dtls_pfx))
            cert = X509CertificateLoader.LoadPkcs12FromFile(dtls_pfx, "test");
        else
            cert = CertGen.CreateCert("dtls", ecdsa, HashAlgorithmName.SHA256);
        */
        ServerController.Start(new ServerShared.CommonModels.ServerModel()
        {
            Name = "DTLS",
            Port = PORT,
            IsSecure = true,
            IsUdp = true,
        });
        CoreSslUdpSession.OnBytesReceived += DtlHandler.CoreSslUdpSession_OnBytesReceived;
    }    

    public override void Stop()
    {
        CoreSslUdpSession.OnBytesReceived -= DtlHandler.CoreSslUdpSession_OnBytesReceived;
    }
}
