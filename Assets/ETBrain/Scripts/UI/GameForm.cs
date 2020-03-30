
using GameFramework;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class GameForm: UGuiForm
    {
        private Button mCloseBtn;
        private Text mScroreLabel;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCloseBtn = CachedTransform.Find("root/closeBtn").GetComponent<Button>();
            mScroreLabel = CachedTransform.Find("root/scoreLabel").GetComponent<Text>();

            mCloseBtn.onClick.AddListener(OnCloseClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            uint score = GameEntry.DataNode.GetData<VarUInt>(Constant.DataNode.UserScore).Value;
            mScroreLabel.text = Utility.Text.Format("分数: {0}", score);
        }

        void OnCloseClick()
        {
            ProcedureGame.Instance.JumpToMain();
            GameEntry.UI.CloseUIForm(this);
        }
    }
}
