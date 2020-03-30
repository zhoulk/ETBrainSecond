using GameFramework;
using System;

[Serializable]
public class PlatformDeviceBindRequest : BaseUrlParam
{
    public string deviceCode;
    public string product;

    public static PlatformDeviceBindRequest Create(string deviceCode, string product)
    {
        PlatformDeviceBindRequest param = ReferencePool.Acquire<PlatformDeviceBindRequest>();
        param.deviceCode = deviceCode;
        param.product = product;
        return param;
    }

    public override void Clear()
    {
        base.Clear();

        deviceCode = string.Empty;
        product = string.Empty;
    }
}
