using System;

[Serializable]
public class PlatformAccessTokenRequest
{
    public string code;
    public string grantType;
    public string type;
}
