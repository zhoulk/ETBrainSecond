/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：由Sdk 提供的工具集 接口,具体包括支付，二维码扫码等功能
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace LT.Sdk
{
    /// <summary>
    /// 由Sdk 提供的工具集接口,具体包括支付，二维码扫码等功能
    /// </summary>
    public interface ISdkTool
    {
        /// <summary>
        /// 异步获取手柄二维码(下载/扫描链接功能二合一)
        /// </summary>
        /// <param name="width">二维码宽</param>
        /// <param name="height">二维码高</param>
        /// <returns>返回二维码Sprite</returns>
        LTTask<Sprite> GetQRCode2In1Async(int width = 188, int height = 188);

        /// <summary>
        /// 异步获取提供下载功能的二维码图片
        /// </summary>
        /// <param name="width">二维码宽</param>
        /// <param name="height">二维码高</param>
        /// <returns>返回图片路径</returns>
        LTTask<Sprite> GetDownloadQRCodeAsync(int width = 188, int height = 188);

        /// <summary>
        /// 异步获取提供扫码链接功能的二维码图片
        /// </summary>
        /// <param name="width">二维码宽</param>
        /// <param name="height">二维码高</param>
        /// <returns>返回图片路径</returns>
        LTTask<Sprite> GetLinkQRCodeAsync(int width = 188, int height = 188);

        /// <summary>
        /// 异步支付接口
        /// </summary>
        /// <param name="productID">产品id </param>
        LTTask<PaymentResult> PaymentAsync(string productID);

        /// <summary>
        /// 添加产品计费表,先存了该表才能进行PaymentAsync支付
        /// </summary>
        /// <param name="productTable">产品计费表</param>
        void AddProductTable<T>(List<T> productTable);

        /// <summary>
        /// 获取SDK中的配置
        /// <para>内置配置类型为GameSdkConfig，支持自定义类型</para>
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>返回配置</returns>
        TConfig GetGameConfig<TConfig>() where TConfig : new();

        /// <summary>
        /// 调用Android的方法退出应用
        /// </summary>
        void ExitApp();

        /// <summary>
        /// 设置Sdk配置
        /// </summary>
        /// <param name="json">json对象</param>
        void SetSDKConfig(object json);

        /// <summary>
        /// 设置sdk配置
        /// </summary>
        /// <param name="json">json数据</param>
        void SetSDKConfig(string json);

        /// <summary>
        /// 打开梦想乐园测试数据
        /// </summary>
        void OpenMXLYTestData();

        /// <summary>
        /// 广播数据到手柄
        /// </summary>
        /// <param name="json">json数据</param>
        void BroadcastDataToGamePad(string json);

        /// <summary>
        /// 广播数据到手柄
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">对象实例</param>
        void BroadcastDataToGamePad<T>(T data);

        /// <summary>
        /// 保存游戏质量等级
        /// </summary>
        /// <param name="lv"></param>
        void SaveGameQuality(int lv);

        /// <summary>
        /// 增加金币
        /// </summary>
        /// <param name="value">增量</param>
        void AddGoldCoin(int value);

        /// <summary>
        /// 获取金币
        /// </summary>
        /// <returns>金币数</returns>
        int GetGoldCoin();
    }
}