using Plugin.DTLS.Enums;
using System.Net.Security;

namespace Plugin.DTLS;

internal class CipherSuitesHelper
{
    public TlsCipherSuite Suite;
    public ProtocolVersion MinProtocolVersion;
    public HashAlgorithm Algorithm;
    public SignatureAlgorithm SignatureAlgorithm;
}
