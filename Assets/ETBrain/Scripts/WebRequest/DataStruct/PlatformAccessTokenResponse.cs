using System;

[Serializable]
public class PlatformAccessTokenResponse
{

    public int code;
    public string text;
    public PlatformAccessTokenResponseData data;
}

[Serializable]
public class PlatformAccessTokenResponseData
{
    public string accessToken;
    public int code;
    public int errCode;
    public string errMsg;
    public int expiresIn;
    public string openId;
    public string refreshToken;
    public string scope;
    public string text;
}

