using System.Net.Security;

namespace Plugin.DTLS;

public class DTLSPluginConfig
{
    public List<ServerConfig> Servers  { get; set; } = [];
}

public class ServerConfig
{
    public int Port { get; set; }
    public Dictionary<string, string> PSKIdentities { get; set; } = [];
    public List<TlsCipherSuite> SupporterCipherSuites { get; set; } = [];
    public bool RequireClientCertificate { get; set; }
    public string PemFilePath { get; set; } = string.Empty;
}