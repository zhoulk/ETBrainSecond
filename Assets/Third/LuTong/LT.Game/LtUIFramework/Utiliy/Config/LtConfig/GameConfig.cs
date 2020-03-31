/*
*    描述:
*          1. 配置数据
*
*    开发人: 邓平
*/
using System;

namespace LtFramework.Util
{
    [Serializable]
    public class ConfigInfo
    {
        public bool IsSdk; //是否对接SDK
        public bool OpenShouBing; //打开手柄
        public bool OpenDouble; //是否打开双人模式
        public bool OpenErWeiMa; //是否显示二维码
        public bool IsInAppPurchase; //是否内购
        public bool IsLocalSave; //游戏存档是否存本地
        public bool OpenMonth; //是否打开包月
        public bool OpenStageFree; //是否打开阶段免费进入 false为免费模式
    }
}
