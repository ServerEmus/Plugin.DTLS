using Plugin.DTLS.ContentTypes;
using Plugin.DTLS.Enums;
using Plugin.DTLS.Handshake;
using Plugin.DTLS.Records;
using Serilog;
using ServerShared.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.DTLS.HanshakeProccessor;

internal class ClientHelloProcessor : IHandshakeProcessor
{
    public static byte[] Key = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x9, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F];
    public HandshakeType Type => HandshakeType.ClientHello;

    public void Process(DtlsSession session, ref DTLSPlaintext record, ref HandshakeHeader content, in IHandshake handshake)
    {
        if (handshake is not ClientHello clientHello)
            return;
        var cookie = CreateCoockie(session, clientHello.Random);
        if (clientHello.Cookie.Length == 0)
        {
            HelloVerifyRequest helloVerifyRequest = new()
            { 
                Cookie = cookie,
                ServerVersion = session.ProtocolVersion,
            };
            // TODOD: make a better thank here.
            Log.Information("Sending: {HVR}", helloVerifyRequest);
            MemoryStream mem = new();
            BinaryWriterBig writerBig = new(mem);
            helloVerifyRequest.Serialize(writerBig);
            writerBig.Flush();
            Log.Information("Sending: {HVR} (Size: {Size})", helloVerifyRequest, mem.Length);
            content.HandshakeType = helloVerifyRequest.Type;
            content.Reset();
            content.Length = (ServerShared.Types.UInt24)mem.Length;
            content.FragmentLength = (ServerShared.Types.UInt24)mem.Length;
            content.Payload = mem.ToArray();
            Log.Information("Sending: {content}", content);
            mem = new();
            writerBig = new(mem);
            content.Serialize(writerBig);
            writerBig.Flush();
            Log.Information("Sending: {content} (Size: {Size})", content, mem.Length);
            record.Fragment = mem.ToArray();
            Log.Information("Sending: {record}", record);
            mem = new();
            writerBig = new(mem);
            record.Serialize(writerBig);
            writerBig.Flush();
            Log.Information("Sending: {record} (Size: {Size})", record, mem.Length);
            session.Session.Send(mem.ToArray());
            return;
        }
    }

    private static byte[] CreateCoockie(DtlsSession session, byte[] random)
    {
        using MemoryStream stream = new();
        stream.Write(session.Session.EndPoint.Serialize().Buffer.Span);
        stream.Write(random);
        return [.. new HMACSHA256(Key).ComputeHash(stream.ToArray()).Take(32)];
    }
}
