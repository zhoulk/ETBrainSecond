
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static BuiltinDataComponent Builtin
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static WebRequestCBComponent WebRequestCB
        {
            get;
            private set;
        }

        /// <summary>
        /// 锁定同步
        /// </summary>
        public static LockStepComponent LockStep
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Builtin = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            WebRequestCB = UnityGameFramework.Runtime.GameEntry.GetComponent<WebRequestCBComponent>();
            WebRequestCB.InitWebRequest();
            LockStep = UnityGameFramework.Runtime.GameEntry.GetComponent<LockStepComponent>();
            LockStep.Init();
        }

        private static void InitCustomDataNode()
        {
            VarBool isBind = new VarBool(false);
            DataNode.SetData(Constant.DataNode.IsWXBind, isBind);
            VarString v_userId = new VarString("");
            DataNode.SetData(Constant.DataNode.UserId, v_userId);
            VarUInt v_score = new VarUInt(0);
            DataNode.SetData(Constant.DataNode.UserScore, v_score);
        }
    }
}