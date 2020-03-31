/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LT;
using LT.Net;

namespace LT
{
    public static class MessagesFactory
    {
        //键盘消息
        public static IMessage Keyboard(byte hid, string keyName, KeyboardState state)
        {
            int keycode = KeyMap[hid][keyName];

            MessageKeyboard msg = new MessageKeyboard();
            msg.Clear();
            msg.TimeStamp = SystemTime.LowClientNow();
            msg.Hid = hid;
            msg.KeyCode = (KeyCode2)keycode;
            msg.State = state;
            return msg;
        }

        //摇杆消息
        public static IMessage Rocker(byte hid, byte id, float x, float y, string keyName, KeyboardState state)
        {
            int keycode = KeyMap[hid][keyName];
            MessageRocker msg = new MessageRocker();
            msg.Clear();
            msg.Rid = id;
            msg.TimeStamp = SystemTime.LowClientNow();
            msg.Hid = hid;
            msg.KeyCode = (KeyCode2)keycode;
            msg.State = state;
            msg.Rx = x;
            msg.Ry = y;

            return msg;
        }

        /// <summary>
        /// 陀螺仪消息
        /// </summary>
        public static IMessage Gyro(byte hid)
        {
            MessageGyro msg = new MessageGyro();
            msg.Clear();
            msg.TimeStamp = SystemTime.LowClientNow();
            msg.Hid = hid;
            msg.Gravity = Input.gyro.gravity;
            msg.RotationRate = Input.gyro.rotationRate;
            msg.UserAcceleration = Input.gyro.userAcceleration;
            msg.Attitude = Input.gyro.attitude;

            return msg;
        }

        /// <summary>
        /// 心跳消息
        /// </summary>
        /// <returns></returns>
        public static IMessage Heart(byte hid)
        {
            MessageHeart msg = new MessageHeart();
            msg.Clear();
            msg.Hid = hid;
            msg.TimeStamp = SystemTime.LowClientNow();
            return msg;
        }

        /// <summary>
        /// 链接
        /// </summary>
        public static IMessage Connect(byte hid)
        {
            MessageConnect msg = new MessageConnect();
            msg.TimeStamp = SystemTime.LowClientNow();
            msg.Hid = hid;
            msg.State = 1;

            return msg;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public static IMessage Disconnect(byte hid)
        {
            MessageConnect msg = new MessageConnect();
            msg.TimeStamp = SystemTime.LowClientNow();
            msg.Hid = hid;
            msg.State = 2;

            return msg;
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IMessage Msg(string data)
        {
            //该协议不传hid，主要用于平台传输消息给手柄
            MessageMsg msg = new MessageMsg();
            byte[] bytes = data.ToByteArray();
            msg.Data = bytes;
            return msg;
        }

        public static Dictionary<byte, Dictionary<string, int>> KeyMap = new Dictionary<byte, Dictionary<string, int>>(){

            { 1,new Dictionary<string,int>(){
            { "up", 19 } ,
            { "down", 20},
            { "left", 21},
            { "right", 22},
            { "a", 23},
            { "b", 97},
            { "x", 99},
            { "y", 100},
            { "l", 102},
            { "r", 103}
        } },

        { 2,new Dictionary<string, int>(){
            { "up", 51 } ,
            { "down", 47},
            { "left", 29},
            { "right", 32},
            { "a", 40},
            { "b", 39},
            { "x", 38},
            { "y", 37},
            { "l", 49},
            { "r", 43}
        } }
    };
    }
}