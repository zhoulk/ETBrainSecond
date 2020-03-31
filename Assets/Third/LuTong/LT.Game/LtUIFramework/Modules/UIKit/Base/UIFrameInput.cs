/*
 *    描述:
 *          1. UI操作外观类
 *
 *    开发人: 邓平
 */
using LT;
using UnityEngine;
//using LT;

#if UNITY_EDITOR_
namespace LtFramework.UI
{
    public static class UIFrameInput
    {
        public static bool GetAnyKey_Down => Input.anyKeyDown;

        public static bool GetKey_Up_Down => Input.GetKeyDown(KeyCode.UpArrow);

        public static bool GetKey_Down_Down => Input.GetKeyDown(KeyCode.DownArrow);

        public static bool GetKey_Left_Down => Input.GetKeyDown(KeyCode.LeftArrow);

        public static bool GetKey_Right_Down => Input.GetKeyDown(KeyCode.RightArrow);


        public static bool GetKey_A_Down
        {
            get
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
                    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown((KeyCode)10) ||
                    Input.GetKeyDown(KeyCode.Keypad1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool GetKey_B_Down => Input.GetKeyDown(KeyCode.Keypad2);


        public static bool GetKey_Up_Down_2P => Input.GetKeyDown(KeyCode.W);

        public static bool GetKey_Down_Down_2P => Input.GetKeyDown(KeyCode.S);

        public static bool GetKey_Left_Down_2P => Input.GetKeyDown(KeyCode.A);

        public static bool GetKey_Right_Down_2P => Input.GetKeyDown(KeyCode.D);


        public static bool GetKey_A_Down_2P => Input.GetKeyDown(KeyCode.J);

        public static bool GetKey_B_Down_2P => Input.GetKeyDown(KeyCode.K);

        public static bool GetAnyKey
        {
            get
            {
                return Input.anyKey;
            }
        }
    }
}
#else
public static class UIFrameInput
{
    public static bool GetKey_Up_Down
    {
        get
        {
            return LTInput.GetKeyDown(KeyCode2.Up, 1);
        }
    }

    public static bool GetKey_Down_Down
    {
        get { return LTInput.GetKeyDown(KeyCode2.Down, 1); }
    }

    public static bool GetKey_Left_Down
    {
        get { return LTInput.GetKeyDown(KeyCode2.Left, 1); }
    }

    public static bool GetKey_Right_Down
    {
        get { return LTInput.GetKeyDown(KeyCode2.Right, 1); }
    }


    public static bool GetKey_A_Down
    {
        get
        {
            return LTInput.GetKeyDown(KeyCode2.A, 1);
        }
    }

    public static bool GetKey_B_Down
    {
        get
        {
            return LTInput.GetKeyDown(KeyCode2.B, 1);
        }
    }


    public static bool GetKey_Up_Down_2P
    {
        get
        {
            return LTInput.GetKeyDown(KeyCode2.Up, 2);
        }
    }

    public static bool GetKey_Down_Down_2P
    {
        get { return LTInput.GetKeyDown(KeyCode2.Down, 2); }
    }

    public static bool GetKey_Left_Down_2P
    {
        get { return LTInput.GetKeyDown(KeyCode2.Left, 2); }
    }

    public static bool GetKey_Right_Down_2P
    {
        get { return LTInput.GetKeyDown(KeyCode2.Right, 2); }
    }


    public static bool GetKey_A_Down_2P
    {
        get { return LTInput.GetKeyDown(KeyCode2.A, 2); }
    }

    public static bool GetKey_B_Down_2P
    {
        get { return LTInput.GetKeyDown(KeyCode2.B, 2); }
    }

    public static bool GetAnyKey
    {
        get
        {
            return LTInput.AnyKey;
        }
    }
}

    #endif
