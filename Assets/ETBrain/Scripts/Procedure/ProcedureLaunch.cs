﻿
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETBrain
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override internal void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //// 构建信息：发布版本时，把一些数据以 Json 的格式写入 Assets/ETBrain/Configs/BuildInfo.txt，供游戏逻辑读取。
            //GameEntry.BuiltinData.InitBuildInfo();

            //// 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言。
            //InitLanguageSettings();

            //// 变体配置：根据使用的语言，通知底层加载对应的资源变体。
            //InitCurrentVariant();

            //// 画质配置：根据检测到的硬件信息 Assets/Main/Configs/DeviceModelConfig 和用户配置数据，设置即将使用的画质选项。
            //InitQualitySettings();

            //// 声音配置：根据用户配置数据，设置即将使用的声音选项。
            //InitSoundSettings();

            //// 默认字典：加载默认字典文件 Assets/ETBrain/Configs/DefaultDictionary.xml。
            //// 此字典文件记录了资源更新前使用的各种语言的字符串，会随 App 一起发布，故不可更新。
            //GameEntry.BuiltinData.InitDefaultDictionary();
        }

        protected override internal void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            ChangeState<ProcedureSplash>(procedureOwner);
        }

    }
}