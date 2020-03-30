
using GameFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public partial class MapForm : UGuiForm
    {
        //按钮
        private Button mBtnSet;
        private Button mBtnOrder;
        private Button mBtnStage;
        private Button mBtnTask;
        private Button mBtnLotto;
        private Button mBtnHome;
        private Button mBtnAchievement;
        private Button mBtnPho;
        private Button mBtnPK;
        private Button mBtnShop;
        private Button mBtnRank;
        private Button mBtnDayTask;

        private Button[] mLevelBtns;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            mBtnSet = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_set").GetComponent<Button>();
            mBtnOrder = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_order").GetComponent<Button>();
            mBtnStage = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_class").GetComponent<Button>();
            mBtnTask = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_task").GetComponent<Button>();
            mBtnLotto = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_lotto").GetComponent<Button>();
            mBtnHome = CachedTransform.Find("root/PanelCtrl/btns/Btn_icon_home").GetComponent<Button>();

            mBtnAchievement = CachedTransform.Find("root/PanelCtrl/Btn_user_center").GetComponent<Button>();
            mBtnPho = CachedTransform.Find("root/PanelCtrl/Btn_pho_user").GetComponent<Button>();
            mBtnPK = CachedTransform.Find("root/PanelCtrl/pkBtn").GetComponent<Button>();
            mBtnShop = CachedTransform.Find("root/PanelCtrl/shopBtn").GetComponent<Button>();
            mBtnRank = CachedTransform.Find("root/PanelCtrl/rankBtn").GetComponent<Button>();
            mBtnDayTask = CachedTransform.Find("root/PanelCtrl/Btn_icon_dayTask").GetComponent<Button>();

            mLevelBtns = new Button[36];
            for(int i=0; i<36; i++)
            {
                mLevelBtns[i] = CachedTransform.Find(Utility.Text.Format("{0}{1}", "root/Scroll View/Viewport/Content/MapBg/BtnLevel", i+1)).GetComponent<Button>();
            }

            mBtnSet.onClick.AddListener(OnBtnSetClick);
            mBtnRank.onClick.AddListener(OnBtnRankClick);
            mBtnPK.onClick.AddListener(OnBtnPKClick);
            mBtnShop.onClick.AddListener(OnBtnStoreClick);
            mBtnAchievement.onClick.AddListener(OnBtnAchievementClick);
            mBtnPho.onClick.AddListener(OnBtnUserPhoClick);
            mBtnLotto.onClick.AddListener(OnBtnLottoClick);
            mBtnHome.onClick.AddListener(OnBtnHomeClick);
            mBtnOrder.onClick.AddListener(OnBtnOrderClick);
            mBtnTask.onClick.AddListener(OnTaskBtnClick);
            mBtnStage.onClick.AddListener(OnBtnStageClick);
            mBtnDayTask.onClick.AddListener(OnBtnDayTaskClick);
            for (int i = 0; i < 36; i++)
            {
                int temp = i+1;
                mLevelBtns[i].onClick.AddListener(()=>
                {
                    OnBtnLevelClick(temp);
                });
            }
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            MapFormParams param = (MapFormParams)userData;
            if (param != null)
            {
                Log.Info(param.Stage);
                ReferencePool.Release(param);
            }
        }

        protected internal override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }


        /// <summary>
        /// 设置
        /// </summary>
        public void OnBtnSetClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SetForm);
        }

        /// <summary>
        /// 排行
        /// </summary>
        public void OnBtnRankClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.RankForm);
        }

        /// <summary>
        /// pk按钮
        /// </summary>
        public void OnBtnPKClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PKForm);
        }

        public void OnBtnOrderClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.OrderForm);
        }

        /// <summary>
        /// 商店
        /// </summary>
        public void OnBtnStoreClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ShopForm);
        }

        /// <summary>
        /// 成就中心
        /// </summary>
        public void OnBtnAchievementClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.AchievementForm);
        }

        /// <summary>
        /// 头像
        /// </summary>
        public void OnBtnUserPhoClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PhoForm);
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        public void OnBtnLottoClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.LotteryForm);
        }

        /// <summary>
        /// 首页
        /// </summary>
        public void OnBtnHomeClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HomeForm);
        }

        private void OnTaskBtnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.TaskForm);
        }

        public void OnBtnStageClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.StageForm);
        }

        /// <summary>
        /// 每日任务
        /// </summary>
        public void OnBtnDayTaskClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.DayTaskPopForm);
        }

        void OnBtnLevelClick(int level)
        {
            Log.Info(level);
            ProcedureMain.Instance.JumpToLevel(level);
            GameEntry.UI.CloseUIForm(this);
        }
    }
}
