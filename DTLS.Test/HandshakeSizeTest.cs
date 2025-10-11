using Plugin.DTLS.Extensions;
using Plugin.DTLS.Handshake;
using System.Net.Security;
using System.Security.Cryptography;

namespace DTLS.Test;

public class HandshakeSizeTest
{
    [Fact]
    public void ClientHello_Test()
    {
        ClientHello clientHello = new()
        {
            Version = Plugin.DTLS.ProtocolVersion.DefaultVersion,
            Random = RandomNumberGenerator.GetBytes(32),
            SessionID = [],
            CipherSuites = [TlsCipherSuite.TLS_NULL_WITH_NULL_NULL],
            CompressionMethods = [0],
            Cookie = RandomNumberGenerator.GetBytes(32),
            Extensions =
            {
                new UnknownExtension()
                {
                    Type = Plugin.DTLS.Enums.ExtensionType.SessionTicketTLS,
                    Data = []
                },
                new UnknownExtension()
                {
                    Type = Plugin.DTLS.Enums.ExtensionType.ExtendedMasterSecret,
                    Data = []
                },
            }
        };

        var (hello, writeLen, readLen) = TestHelpers.SerAndDeser(clientHello);
        Assert.Equal(writeLen, readLen);
        Assert.Equal(clientHello.Version, hello.Version);
        Assert.Equal(clientHello.Random, hello.Random);
        Assert.Equal(clientHello.SessionID, hello.SessionID);
        Assert.Equal(clientHello.CipherSuites, hello.CipherSuites);
        Assert.Equal(clientHello.CompressionMethods, hello.CompressionMethods);
        Assert.Equal(clientHello.Cookie, hello.Cookie);
        Assert.Equal(clientHello.Extensions, hello.Extensions);
    }

    [Fact]
    public void HelloVerifyRequestTest()
    {
        HelloVerifyRequest helloVerifyRequest = new()
        {
            ServerVersion = Plugin.DTLS.ProtocolVersion.DefaultVersion,
            Cookie = RandomNumberGenerator.GetBytes(32),
        };

        var (hello, writeLen, readLen) = TestHelpers.SerAndDeser(helloVerifyRequest);
        Assert.Equal(writeLen, readLen);
        Assert.Equal(helloVerifyRequest.ServerVersion, hello.ServerVersion);
        Assert.Equal(helloVerifyRequest.Cookie, hello.Cookie);
    }
}
