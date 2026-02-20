using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Extensions;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.Records;
using Serilog;
using ServerShared.IO;
using System.Net.Security;
using System.Security.Cryptography;

namespace Plugin.DTLS.HanshakeProccessor;

internal class ClientHelloProcessor : IHandshakeProcessor
{
    public static byte[] Key = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x9, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F];
    public static HMACSHA256 HMAC = new(Key);
    public HandshakeType Type => HandshakeType.ClientHello;

    public void Process(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, in IHandshake handshake)
    {
        if (handshake is not ClientHello clientHello)
            return;

        var cookie = CreateCookie(session, clientHello.Random);
        if (clientHello.Cookie.Length == 0)
        {
            CreateVerifyRequest(session, ref record, ref content, cookie);
            return;
        }

        if (!clientHello.Cookie.SequenceEqual(cookie))
        {
            Log.Warning("Client hello cookie is not the same as we made it.");
            return;
        }

        Log.Information("Client Hello! Client Cipher Suites: {suits}", clientHello.CipherSuites);
        // create new sessionId and send ServerHello

        CreateServerHello(session, ref record, ref content, ref clientHello);
    }

    private static byte[] CreateCookie(DtlsSession session, byte[] random)
    {
        using MemoryStream stream = new();
        stream.Write(session.Session.EndPoint.Serialize().Buffer.Span);
        stream.Write(random);
        return [.. HMAC.ComputeHash(stream.ToArray()).Take(32)];
    }

    private static void CreateVerifyRequest(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, byte[] cookie)
    {
        HelloVerifyRequest helloVerifyRequest = new()
        {
            Cookie = cookie,
            ServerVersion = session.ProtocolVersion,
        };
        session.Send(ref record, ref content, ref helloVerifyRequest);
    }

    private static void CreateServerHello(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, ref ClientHello clientHello)
    {
        record.SequenceNumber++;
        Enums.HashAlgorithm hash = Enums.HashAlgorithm.SHA256;
        ECCurve ecCurve = default;

        // Get hash and curve from extension.
        foreach (var extension in clientHello.Extensions)
        {
            if (extension is EllipticCurvesExtension curvesExtension)
            {
                foreach (var item in curvesExtension.EllipticCurves)
                {
                    try
                    {
                        ecCurve = ECCurve.CreateFromFriendlyName(item.ToString());
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("(CreateServerHello) EllipticCurvesExtension {ex}", ex);
                    }
                }
            }

            if (extension is SignatureAlgorithmsExtension algorithmsExtension)
            {
                foreach (var item in algorithmsExtension.SignatureAlgorithms)
                {
                    if (item.Signature == SignatureAlgorithm.ECDSA)
                    {
                        hash = item.Hash;
                        break;
                    }
                }
            }
        }

        TlsCipherSuite selectedSuite = TlsCipherSuite.TLS_NULL_WITH_NULL_NULL;
        foreach (var cipher in clientHello.CipherSuites)
        {
            if (!session.Config.SupporterCipherSuites.Contains(cipher))
                continue;

            // More check if PKS / cert loaded.

            //if (cipher.)

            selectedSuite = cipher;
            break;
        }

        ServerHello serverHello = new()
        { 
            CipherSuite = selectedSuite,
            SessionID = session.Session.Id.ToByteArray()[0..32],
            CompressionMethod = 0,
            Version = clientHello.Version,
            Random = RandomNumberGenerator.GetBytes(32),
            Extensions =
            {

            }
        };

        session.Send(ref record, ref content, ref serverHello);
    }
}
