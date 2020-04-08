
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETBrain
{
    public class ProcedureChangeScene : ProcedureBase
    {
        bool m_changeToGame = false;

        private bool m_IsChangeSceneComplete = false;

        int nextSceneId = 0;

        protected override internal void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_IsChangeSceneComplete = false;
            m_changeToGame = false;

            if (procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId) != null)
            {
                nextSceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            }

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenFormSuccess);

            // 停止所有声音
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            if (GameEntry.UI.HasUIForm(UIFormId.LoadingForm))
            {
                ChangeScene();
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.LoadingForm, this);
            }
        }

        protected override internal void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenFormSuccess);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override internal void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsChangeSceneComplete)
            {
                return;
            }

            if (m_changeToGame)
            {
                ChangeState<ProcedureGame>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }

            if (GameEntry.UI.GetUIForm(UIFormId.LoadingForm) != null)
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.LoadingForm));
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            //if (m_BackgroundMusicId > 0)
            //{
            //    GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
            //}
            GameEntry.Sound.PlayMusic("music_background");

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }

        void OnOpenFormSuccess(object sender, GameEventArgs e)
        {
            ChangeScene();
        }

        void ChangeScene()
        {
            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            if (nextSceneId == SceneId.AircraftScene)
            {
                m_changeToGame = true;
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("game"), Constant.AssetPriority.SceneAsset, this);
            }
            else if (nextSceneId == SceneId.TilemapScene)
            {
                m_changeToGame = true;
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("tileMap"), Constant.AssetPriority.SceneAsset, this);
            }
            else if (nextSceneId == SceneId.CineMachine)
            {
                m_changeToGame = true;
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("cineMachine"), Constant.AssetPriority.SceneAsset, this);
            }
            else
            {
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("main"), Constant.AssetPriority.SceneAsset, this);
            }
        }
    }
}
