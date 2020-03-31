/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/18
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT;

/// <summary>
/// 游戏Sdk配置参数
/// </summary>
public partial class GameSdkConfig
{
    /// <summary>
    /// Pomelo 地址
    /// <para>由页面参数pomeloUrl获得，默认为https://api-youxi-mxly.vas.lutongnet.com</para>
    /// </summary>
    public string PomeloIP = "https://api-youxi-mxly.vas.lutongnet.com";

    /// <summary>
    /// Pomelo 端口
    /// <para>由页面参数pomeloUrl,默认端口为：2014</para>
    /// </summary>
    public string PomeloPort = "2014";

    /// <summary>
    /// 渠道
    /// <para>"common" 为通用版本</para>
    /// <para>"telecom_neimenggu"为内蒙古单机版</para>
    /// </summary>
    public string Channel = "common";

    /// <summary>
    /// 二维码页面类型
    /// <para>"common"下载二维码和IP二维码分开</para>
    /// <para>"common2in1"：通用二合一</para>
    /// <para>"private"，广东转网</para>
    /// </summary>
    public string QRCodePageType = "common2in1";

    /// <summary>
    /// "0" 普通，"1" 匹配
    /// </summary>
    public string IsMatch = "0";

    /// <summary>
    /// 用户id,由安卓lt_params 获取
    /// </summary>
    public string UserId = $"Test{ConvGenerator.Conv()}";

    /// <summary>
    /// 游戏入口,由安卓lt_params 获取
    /// <para>"game" 从游戏详情页进入</para>
    /// <para>"battle"表示从匹配页面进入(控制游戏的页面逻辑)</para>
    /// </summary>
    public string Entry = "game";

    /// <summary>
    /// 资源地址
    /// </summary>
    public string ResServerUrl;
}