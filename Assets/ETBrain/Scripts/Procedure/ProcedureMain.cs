using GameFramework.Event;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETBrain
{
    public class ProcedureMain : ProcedureBase
    {
        bool m_changeToLevel;
        int m_currentLevel;

        public static ProcedureMain Instance;

        protected override internal void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            Instance = this;
        }

        protected override internal void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override internal void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_changeToLevel = false;

            if (procedureOwner.GetData<VarBool>(Constant.ProcedureData.Gaming) != null &&
                procedureOwner.GetData<VarBool>(Constant.ProcedureData.Gaming).Value)
            {
                GameEntry.UI.OpenUIForm(UIFormId.MapForm);
                return;
            }

            string deviceCode = GetDeviceCode();
            GameEntry.WebRequestCB.GetDeviceBindList(
                PlatformDeviceBindRequest.Create(
                    deviceCode, 
                    "nldmx"),
                (deviceBind) =>{
                    if (deviceBind.code == 0)
                    {
                        string userId = deviceBind.guest;
                        if (deviceBind.userMap != null && deviceBind.userMap.Count > 0)
                        {
                            foreach (var kv in deviceBind.userMap)
                            {
                                //Debug.LogError(kv.Key + "  " + kv.Value.uid);
                                if (kv.Value.thridIds != null && kv.Value.thridIds.Count > 0)
                                {
                                    foreach (var kkvv in kv.Value.thridIds)
                                    {
                                        if ("wx".Equals(kkvv.Key))
                                        {
                                            VarBool isBind = new VarBool(true);
                                            GameEntry.DataNode.SetData(Constant.DataNode.IsWXBind, isBind);
                                            userId = kv.Value.uid;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        VarString v_userId = new VarString(userId);
                        GameEntry.DataNode.SetData(Constant.DataNode.UserId, v_userId);

                        if ((bool)GameEntry.DataNode.GetData(Constant.DataNode.IsWXBind).GetValue())
                        {
                            GameEntry.UI.OpenUIForm(UIFormId.MapForm);
                        }
                        else
                        {
                            GameEntry.UI.OpenUIForm(UIFormId.StageForm);
                        }
                    }
                    else
                    {
                        Log.Error("获取绑定信息异常");
                    }
            });
        }

        protected override internal void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override internal void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_changeToLevel)
            {
                VarInt v_level = new VarInt(m_currentLevel);
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.GameLevel, v_level);
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, 1);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        public void JumpToLevel(int level)
        {
            m_currentLevel = level;
            m_changeToLevel = true;
        }

        string GetDeviceCode()
        {
            string strPwd = SystemInfo.deviceUniqueIdentifier;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(strPwd)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}