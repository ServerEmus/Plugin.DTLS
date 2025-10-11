using Plugin.DTLS.Extensions;
using System.Text;

namespace DTLS.Test;

public class ExtensionSizeTest
{
    [Fact]
    public void EllipticCurvePointFormatsExtension_Test()
    {
        EllipticCurvePointFormatsExtension extension = new()
        { 
            PointFormats = { 0 },
        };
        var (ext, extLen, readLen) = TestHelpers.SerAndDeserExtension(extension);
        Assert.Equal(extLen, readLen);
        Assert.Equal(extension.PointFormats, ext.PointFormats);
    }

    [Fact]
    public void EllipticCurvesExtension_Test()
    {
        EllipticCurvesExtension extension = new()
        {
            EllipticCurves = 
            { 
                 Plugin.DTLS.Enums.EllipticCurve.secp160r1,
                 Plugin.DTLS.Enums.EllipticCurve.secp160k1
            },
        };

        var (ext, extLen, readLen) = TestHelpers.SerAndDeserExtension(extension);
        Assert.Equal(extLen, readLen);
        Assert.Equal(extension.EllipticCurves, ext.EllipticCurves);
    }

    [Fact]
    public void SignatureAlgorithmsExtension_Test()
    {
        SignatureAlgorithmsExtension extension = new()
        {
            SignatureAlgorithms = 
            {
                new()
                {
                    Hash = Plugin.DTLS.Enums.HashAlgorithm.MD5,
                    Signature = Plugin.DTLS.Enums.SignatureAlgorithm.RSA,
                },
                new()
                {
                    Hash = Plugin.DTLS.Enums.HashAlgorithm.SHA256,
                    Signature = Plugin.DTLS.Enums.SignatureAlgorithm.ECDSA,
                }
            }
        };

        var (ext, extLen, readLen) = TestHelpers.SerAndDeserExtension(extension);
        Assert.Equal(extLen, readLen);
        Assert.Equal(extension.SignatureAlgorithms, ext.SignatureAlgorithms);
    }

    [Fact]
    public void ServerNameExtension_Test()
    {
        ServerNameExtension extension = new()
        {
            Names =
            {
                new()
                {
                    NameType = 0,
                    NameData = Encoding.ASCII.GetBytes("test.myserver.hu")
                },
                new()
                {
                    NameType = 1,
                    NameData = Encoding.ASCII.GetBytes("test.myserver23.hu")
                }
            }
        };
        var (ext, extLen, readLen) = TestHelpers.SerAndDeserExtension(extension);
        Assert.Equal(extLen, readLen);
        Assert.Equal(extension.Names, ext.Names, ServerName.Empty);
    }
}
