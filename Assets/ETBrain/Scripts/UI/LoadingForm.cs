using UnityEngine;
using UnityEngine.UI;

namespace ETBrain
{
    public class LoadingForm: UGuiForm
    {
        public Slider slider;

        public Text progressTxt;
        public Sprite sp;
        public Image bgImg;

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

//        protected override void Start()
//        {
//            base.Start();
//#if UNITY_ANDROID
//            LoadBackgroundImgByIO();
//#endif
//            StartCoroutine(LoginGame());
//        }

//        void LoadBackgroundImgByIO()
//        {
//            bgImg.sprite = null;
//            bgImg.color = new Color(0, 0, 0);
//            string localPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "loadingBgImg.png";
//            if (File.Exists(localPath))
//            {
//                //Debug.LogError("存在本地背景:" + localPath);
//                if (bgImg != null) bgImg.sprite = LoadByIO(localPath);
//            }
//            else
//            {
//                // 本地不存在背景
//                //Debug.LogError("不存在本地背景:" + localPath);
//                // 显示默认背景
//                bgImg.sprite = sp;
//            }
//            bgImg.color = new Color(1, 1, 1);
//            // 更新背景图片
//            UpdateBgImg(localPath);
//        }

//        Sprite LoadByIO(string localPath)
//        {
//            // 创建文件读取流
//            FileStream fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
//            fileStream.Seek(0, SeekOrigin.Begin);
//            // 创建文件长度缓冲区
//            byte[] bytes = new byte[fileStream.Length];
//            // 读取文件
//            fileStream.Read(bytes, 0, (int)fileStream.Length);
//            // 释放文件读取流
//            fileStream.Close();
//            fileStream.Dispose();
//            fileStream = null;
//            // 创建texture
//            int width = 300;
//            int height = 372;
//            Texture2D texture = new Texture2D(width, height);
//            texture.LoadImage(bytes);
//            // 创建sprite
//            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//            return sprite;
//        }

//        private void UpdateBgImg(string saveTargetLocalPath)
//        {
//            Debug.Log("=============================更新启动页背景");
//            WebMgr.GetAppLuanch(new UF_GetAppLuanchRequest()
//            {
//                appCode = GlobalConfig.Instance.appCode
//            }, (response) =>
//            {
//                if (response.code == 0 && "success".Equals(response.text))
//                {
//                    //string startDate = response.data.imageStartTime;
//                    //string endDate = response.data.imageEndTime;
//                    //if (MyDateTool.BetweenDateTime(startDate, endDate))
//                    //{
//                    //}
//                    string imgUrl = PathUtility.unitImgPrefix + response.data.startupUrl;
//                    // 下载背景并保存
//                    DoCoroutine.Instance.StartCoroutine(ResMgr.Instance.SaveWWW(imgUrl, saveTargetLocalPath));
//                }
//            });
//        }

//        IEnumerator LoginGame()
//        {
//            yield return null;
//            LoginModule.Instance.Login();
//        }
        
//        public override void OnEnter(params object[] prams)
//        {
//            base.OnEnter(prams);
//            canvasGroup.alpha = 1;
//            DataMgr.Instance.GamePlayMode = GamePlayModeEnum.Null;
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//            //canvasGroup.alpha = 0;
//        }
//        float time = 0;
//        float value = 0;
//        float loginProgress = 0;
//        private void Update()
//        {
//            if (!canvasGroup.interactable)
//            {
//                return;
//            }

//            //InputMgr.Instance.KeyCodeEnterClick();

//            if (null != LoginModule.Instance)
//            {
//                if (LoginModule.Instance.loginProgress >= 1f)
//                {
//                    slider.value = LoginModule.Instance.loginProgress;

//                    progressTxt.text = "100%";
//                }
//                else
//                {
//                    if (loginProgress != LoginModule.Instance.loginProgress)
//                    {
//                        time = 0;
//                        loginProgress = LoginModule.Instance.loginProgress;
//                    }

//                    time += Time.deltaTime;
//                    value = Mathf.Lerp(slider.value, LoginModule.Instance.loginProgress, 6f * time);
//                    slider.value = value;

//                    progressTxt.text = (int)(value*100)+ "%";
//                }
//            }
//        }

//        void OnDestroy()
//        {
//            progressTxt.text = "100%";
//        }
    }
}
