using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETBrain
{
    public class ProcedureGame : ProcedureBase
    {

        public static ProcedureGame Instance;

        bool m_changeToMain = false;

        private GameBase m_CurrentGame = null;

        public GameBase CurrentGame
        {
            get
            {
                return m_CurrentGame;
            }
        }

        protected override internal void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            Instance = this;

            m_CurrentGame = new FirstGame();
        }

        protected override internal void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override internal void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_changeToMain = false;

            int levelId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.GameLevel).Value;
            GameEntry.UI.OpenUIForm(UIFormId.GameForm);

            m_CurrentGame.Initialize();
        }

        protected override internal void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            m_CurrentGame.Shutdown();
        }

        protected override internal void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            m_CurrentGame.Update(elapseSeconds, realElapseSeconds);

            if (m_changeToMain)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, 2);
                procedureOwner.SetData<VarBool>(Constant.ProcedureData.Gaming, true);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        public void JumpToMain()
        {
            m_changeToMain = true;
        }
    }
}