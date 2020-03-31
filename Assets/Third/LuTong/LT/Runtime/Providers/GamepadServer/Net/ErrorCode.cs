/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/30
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    public enum ErrorCode : int
    {
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000 以上，避免跟SocketError冲突
        ERR_MyErrorCode = 100000,

        KcpConnectSuccess = 101001,

        ERR_KcpCantConnect = 102005,
        ERR_KcpChannelTimeout = 102006,
        ERR_KcpRemoteDisconnect = 102007,
        ERR_PeerDisconnect = 102008,
        ERR_SocketCantSend = 102009,
        ERR_SocketError = 102010,
        ERR_KcpWaitSendSizeTooLarge = 102011,
    }
    //public static class ErrorCode
    //{
    //    // 1-11004 是SocketError请看SocketError定义
    //    //-----------------------------------
    //    // 100000 以上，避免跟SocketError冲突
    //    public const int ERR_MyErrorCode = 100000;

    //    public const int KcpConnectSuccess = 101001;
    //    public const int ERR_KcpCantConnect = 102005;
    //    public const int ERR_KcpChannelTimeout = 102006;
    //    public const int ERR_KcpRemoteDisconnect = 102007;
    //    public const int ERR_PeerDisconnect = 102008;
    //    public const int ERR_SocketCantSend = 102009;
    //    public const int ERR_SocketError = 102010;
    //    public const int ERR_KcpWaitSendSizeTooLarge = 102011;
    //}
}