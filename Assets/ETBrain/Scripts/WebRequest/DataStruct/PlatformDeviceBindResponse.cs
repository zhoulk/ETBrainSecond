using System;
using System.Collections.Generic;

namespace ETBrain
{
    [Serializable]
    public class PlatformDeviceBindResponse
    {
        public int code;
        public string guest;
        public Dictionary<string, PlatformDeviceBindData> userMap;
    }

    [Serializable]
    public class PlatformDeviceBindData
    {
        public string uid;
        public string product;
        public string deviceCode;
        public Dictionary<string, string> thridIds;
    }
}
