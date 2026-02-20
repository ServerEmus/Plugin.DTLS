using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Plugin.DTLS; 

public class DTLHandshakeInfo
{
    private SHA1 sha1 => SHA1.Create();
    private MD5 md5 => MD5.Create();
    private SHA256 sha256 => SHA256.Create();

    public void UpdateHandshakeHash()
    {
        
    }
}
