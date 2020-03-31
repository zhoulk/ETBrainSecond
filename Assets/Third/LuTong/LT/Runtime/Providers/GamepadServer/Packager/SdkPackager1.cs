/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Gamepad Sdk 1.0 协议解包器
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Net
{
    public class SdkPackager1 : IPackager
    {
        MessageKeyboard keyboard;

        public SdkPackager1()
        {
            keyboard = new MessageKeyboard();
        }

        public IMessage Decode(string data)
        {
            keyboard.Clear();

            string[] keyData = data.Split('-');
            KeyCode2 kc = (KeyCode2)int.Parse(keyData[0]);
            string keyState = keyData[1];

            //TODO 1p,2p 赋值
            keyboard.KeyCode = kc;
            if (keyState.Equals("down")) keyboard.State = KeyboardState.KeyDown;
            else if (keyState.Equals("up")) keyboard.State = KeyboardState.KeyUp;

            return keyboard;
        }

        public IMessage Decode(byte[] bytes)
        {
            return null;
        }
    }
}

