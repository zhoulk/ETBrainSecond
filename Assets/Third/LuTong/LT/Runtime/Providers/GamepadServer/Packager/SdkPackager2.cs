/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Gamepad Sdk 2.0 协议解包器
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT.Net
{
    public class SdkPackager2 : CShaperPackager
    {
        public override IMessage Decode(string data)
        {
            byte[] buf = Convert.FromBase64String(data);

            return base.Decode(buf);
        }
    }
}