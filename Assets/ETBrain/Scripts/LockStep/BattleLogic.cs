
using Msg;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class BattleLogic
    {

        //帧同步核心逻辑对象
        LockStepLogic m_lockStepLogic = null;

        //- 主循环
        // Some description, can be over several lines.
        // @return value description.
        // @author
        public void updateLogic()
        {
            ////如果战斗逻辑暂停则不再运行
            //if (m_bIsBattlePause)
            //{
            //    return;
            //}

            //调用帧同步逻辑
            m_lockStepLogic.updateLogic();
        }


        //- 战斗逻辑
        // 
        // @return none
        public void frameLockLogic()
        {
            //Log.Info("frameLockLogic  " + GameData.g_uGameLogicFrame);

            ProcedureGame.Instance.CurrentGame.UpdateLogic();

            recordLastPos();

            for (int i = 0; i < GameData.g_listAsteroid.Count; i++)
            {
                Asteroid asteroid = GameData.g_listAsteroid[i];
                asteroid.UpdateLogic();
            }

            for (int i = 0; i < GameData.g_listBullet.Count; i++)
            {
                Bullet bullet = GameData.g_listBullet[i];
                bullet.UpdateLogic();
            }

            CSData csData = CSData.Create();
            DataRequest dataRequest = (DataRequest)csData.GetExtensionObject();

            for (int i = 0; i < GameData.g_listAircreaft.Count; i++)
            {
                Aircraft aircraft = GameData.g_listAircreaft[i];
                aircraft.UpdateLogic();

                ObjData objData = new ObjData();
                objData.SerialId = aircraft.Id;
                objData.X = aircraft.m_fixv3LogicPosition.x.RawValue;
                objData.Y = aircraft.m_fixv3LogicPosition.y.RawValue;
                objData.Z = aircraft.m_fixv3LogicPosition.z.RawValue;
                objData.IsFire = aircraft.isFire;
                dataRequest.Objs.Add(objData);
            }

            //发送到服务器
            if (dataRequest.Objs.Count > 0 && GameEntry.Network.GetNetworkChannel("game") != null)
            {
                GameEntry.Network.GetNetworkChannel("game").Send(csData);

                //for (int i = 0; i < GameData.g_listAircreaft.Count; i++)
                //{
                //    Aircraft aircraft = GameData.g_listAircreaft[i];
                //    aircraft.m_fixv3RenderPosition = aircraft.m_fixv3LogicPosition;
                //}
            }
        }

        //- 更新各种对象绘制的位置
        // 包括怪,子弹等等,因为塔的位置是固定的,所以不需要实时刷新塔的位置,提升效率
        // @return none
        public void updateRenderPosition(float interpolation)
        {
            //士兵
            for (int i = 0; i < GameData.g_listAircreaft.Count; i++)
            {
                GameData.g_listAircreaft[i].UpdateRenderPosition(interpolation);
            }

            for (int i = 0; i < GameData.g_listAsteroid.Count; i++)
            {
                GameData.g_listAsteroid[i].UpdateRenderPosition(interpolation);
            }

            for (int i = 0; i < GameData.g_listBullet.Count; i++)
            {
                GameData.g_listBullet[i].UpdateRenderPosition(interpolation);
            }
        }

        //- 记录最后的位置
        // 
        // @return none.
        void recordLastPos()
        {
            //士兵
            for (int i = 0; i < GameData.g_listAircreaft.Count; i++)
            {
                GameData.g_listAircreaft[i].RecordLastPos();
            }

            for (int i = 0; i < GameData.g_listAsteroid.Count; i++)
            {
                GameData.g_listAsteroid[i].RecordLastPos();
            }

            for (int i = 0; i < GameData.g_listBullet.Count; i++)
            {
                GameData.g_listBullet[i].RecordLastPos();
            }
        }

        //- 初始化
        // 
        // @param mt 上下文句柄
        // @return 元表
        public void Init()
        {
            Log.Info("BattleLogic init!");
            //初始化帧同步逻辑对象
            m_lockStepLogic = new LockStepLogic();
            m_lockStepLogic.setCallUnit(this);

            ////游戏运行速度
            //UnityTools.setTimeScale(1);

            ////战斗不暂停
            //m_bIsBattlePause = true;
        }
    }

}

