/***
 * 
 *    项目: UI框架
 *
 *    描述: 
 *           功能： 音效资源 加载类 根据枚举类找到相应的资源
 *                  
 *    时间: 2017.7
 *
 *    版本: 0.1版本
 *
 *    修改记录: 无
 *
 *    开发人: 邓平
 *     
 */

using UnityEngine;
using System.Collections;
using LtFramework.ResKit;

namespace LtFramework.UI
{

    public class ResourcesMgr
    {
        private ResourcesMgr()
        {
        }

        private static ResourcesMgr instance;

        public static ResourcesMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourcesMgr();
                }

                return instance;
            }
        }


        public T Load<T>(object enumName) where T : Object
        {
            string enumType = enumName.GetType().Name;
            string filePath = string.Empty;

            switch (enumType)
            {
                case "UI":
                    {
                        filePath = "sound/UI/" + enumName;
                        break;
                    }
                case "Battle":
                    {
                        filePath = "sound/Battle/" + enumName;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            //return ResMgr.LoadRes<T>(filePath);
            return Resources.Load<T>(filePath);
        }
    }
}