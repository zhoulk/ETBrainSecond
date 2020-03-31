/***
 * 
 *    项目: UI框架
 *
 *    描述: 
 *           功能： 声音播放管理
 *                     通过枚举类 播放音效
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

using System.Collections;
using UnityEngine;


namespace LtFramework.UI
{

    public class AudioMgr : MonoBehaviour
    {
        #region  字段

        private static AudioMgr istance;

        public static AudioMgr Instance
        {
            get
            {
                if (istance == null)
                {
                    istance = new GameObject("AudioMgr").gameObject.AddComponent<AudioMgr>();
                }

                return istance;
            }
        }

        //背景音乐
        private AudioSource bgMusic;

        //音效  可以设置多个 默认3个
        private AudioSource effectMusic;

        private bool isBGPlay = true;
        private bool isEffectPlay = true;


        #endregion

        #region 私有方法

        void Awake()
        {
            if (istance == null)
            {
                istance = this;
            }

            bgMusic = gameObject.AddComponent<AudioSource>();
            effectMusic = gameObject.AddComponent<AudioSource>();
            bgMusic.loop = true;
            bgMusic.playOnAwake = false;

            DontDestroyOnLoad(this.gameObject);
        }

        public void PlayAudio(AudioSource audio, object audioClip,bool loop = false)
        {
            if (isEffectPlay)
            {
                AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(audioClip);
                audio.loop = loop;
                audio.clip = clip;
                audio.Play();
            }

        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="bgName"></param>
        /// <param name="restart">是否重新播放</param>
        private void PlayBgBase(object bgName, bool restart = false, float volume = 1f)
        {
            if (isBGPlay)
            {
                bgMusic.mute = false;
                bgMusic.volume = volume;
                string curBgName = string.Empty;

                if (bgMusic.clip != null)
                {
                    curBgName = bgMusic.clip.name;
                }

                AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(bgName);

                if (clip != null)
                {
                    if (clip.name == curBgName && !restart)
                    {
                        return;
                    }

                    bgMusic.clip = clip;
                    bgMusic.Play();
                }
                else
                {
                    UnityEngine.Debug.Log("没有找到音频片段");
                }
            }
            else
            {
                bgMusic.mute = true;
            }
        }

		public void StopBgMusic(object bgName)
		{
			if (isBGPlay) {
				bgMusic.mute = false;
				if (bgMusic.clip != null) {
					bgMusic.Stop ();
                }
			}
		}

        public void StopEffect(object effectName)
        {
            if (isEffectPlay)
            {
                effectMusic.mute = false;
                if (effectMusic.clip != null)
                {
                    effectMusic.Stop();
                }
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="effectName">音效名字</param>
        /// <param name="cover">是否覆盖上一个没有播完的音效</param>
        /// <param name="volume">音效大小</param>
        private void PlayEffectBase(object effectName, bool cover = false, float volume = 1f, bool loop = false)
        {
            if (IsEffectPlay)
            {
                 AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(effectName);

                if (clip == null)
                {
                    UnityEngine.Debug.Log("没有找到音效片段 :" + effectName);
                    return;
                }

                if (cover)
                {

                    effectMusic.mute = false;
                    effectMusic.clip = clip;
                    effectMusic.loop = loop;
                    effectMusic.Play();

                }
                else
                {
                    effectMusic.loop = loop;
                    effectMusic.PlayOneShot(clip, volume);
                }

            }
            else
            {
                effectMusic.mute = true;
            }
        }

        #endregion


        public bool IsBgPlay
        {
            get { return isBGPlay; }
            set
            {
                isBGPlay = value; 
                if (value)
                {
                    bgMusic.mute = false;
                }
                else
                {
                    bgMusic.mute = true;
                }
            }
        }

        public bool IsEffectPlay
        {
            get { return isEffectPlay; }
            set
            {
                isEffectPlay = value;
                if (value)
                {
                    effectMusic.mute = false;
                }
                else
                {
                    effectMusic.mute = true;
                }
            }
        }

        // 背景音大小
        public float BgVolume
        {
            get { return bgMusic.volume; }
            set { bgMusic.volume = value; }
        }

        //音效大小
        public float EffectVolmue
        {
            get { return effectMusic.volume; }
            set { effectMusic.volume = value; }
        }

        /// <summary>
        /// 播放背景音
        /// </summary>
        /// <param name="bgName">背景音乐名字</param>
        /// <param name="restart">是否重新播放</param>
        public void PlayBg(SoundType.UI bgName, bool restart = false, float volume = 1f)
        {
            Debug.Log("播放背景音效");
            PlayBgBase(bgName, restart, volume);
        }

        /// <summary>
        /// 播放背景音
        /// </summary>
        /// <param name="bgName">背景音乐名字</param>
        /// <param name="restart">是否重新播放</param>
        public void PlayBg(SoundType.Battle bgName, bool restart = false, float volume = 1f)
        {
            PlayBgBase(bgName, restart, volume);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="effectName">音效名字</param>
        /// <param name="cover">是否覆盖上一个没有播完的音效</param>
        /// <param name="volume">音效大小</param>
        public void PlayEffect(SoundType.UI effectName, bool cover = false, float volume = 1f)
        {
            PlayEffectBase(effectName, cover, volume);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="effectName">音效名字</param>
        /// <param name="cover">是否覆盖上一个没有播完的音效</param>
        /// <param name="volume">音效大小</param>
        public void PlayEffect(SoundType.Battle effectName, bool cover = false, float volume = 1f, bool loop = false)
        {
            //Debug.LogError("PlayEffect  " + effectName);
            PlayEffectBase(effectName, cover, volume, loop);
        }

        public void ClearEffectAudio()
        {
            if (effectMusic != null)
            {
                effectMusic.loop = false;
                effectMusic.Stop();
                effectMusic.clip = null;
            }
        }


        #region 打字音响

        
        private void PlayTypewriteBase(SoundType.UI typewrite, float interval, int times ,float volume)
        {
            AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(typewrite);

            if (interval == -1)
            {
                interval = clip.length;
            }

            if (isPlayTypewrite == false)
            {
                StartCoroutine(PlayOneShot(clip, interval, times, volume));
            }

        }
        
        public void PlayTypewrite(SoundType.UI typewrite, float interval, float volume)
        {
            PlayTypewriteBase(typewrite, interval, 200, volume);
        }
        public void PlayTypewrite(SoundType.UI typewrite, float interval)
        {
            PlayTypewriteBase(typewrite, interval, 200, 1);
        }


        public void PlayTypewrite(SoundType.UI typewrite, int times)
        {
            PlayTypewriteBase(typewrite, -1, times, 1);
        }
        public void PlayTypewrite(SoundType.UI typewrite, int times, float volume)
        {
            PlayTypewriteBase(typewrite, -1, times, volume);
        }

        public void PlayTypewrite(SoundType.UI typewrite,float interval , int times)
        {
            PlayTypewriteBase(typewrite, interval, times, 1);
        }

        /// <summary>
        /// 播放打字音效
        /// </summary>
        /// <param name="typewrite"></param>
        /// <param name="interval"></param>
        /// <param name="times"></param>
        /// <param name="volume"></param>
        public void PlayTypewrite(SoundType.UI typewrite, float interval = -1 , int times = 200,float volume = 1)
        {
            PlayTypewriteBase(typewrite, interval, times, volume);
        }


        private bool isBreakTypewrite = false;
        private bool isPlayTypewrite = false;
        IEnumerator PlayOneShot(AudioClip click, float interval, int times ,float volume)
        {
            isPlayTypewrite = true;
            isBreakTypewrite = false;
            int index = 0;
            while (true)
            {
                effectMusic.PlayOneShot(click, volume);
                yield return new WaitForSeconds(interval);
                index++;
                if (index >= times)
                {
                    isPlayTypewrite = false;
                    break;
                }

                if (isBreakTypewrite)
                {
                    isPlayTypewrite = false;
                    isBreakTypewrite = false;
                    break;
                }
            }
        }

        public void BreakTypewrite()
        {
            isBreakTypewrite = true;
        }
        

        #endregion


    }
}