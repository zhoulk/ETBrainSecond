
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        public static int? PlaySound(this SoundComponent soundComponent, string assetName, Entity bindingEntity = null, object userData = null)
        {
            //IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            //DRSound drSound = dtSound.GetDataRow(soundId);
            //if (drSound == null)
            //{
            //    Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
            //    return null;
            //}

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            //playSoundParams.Priority = drSound.Priority;
            //playSoundParams.Loop = drSound.Loop;
            //playSoundParams.VolumeInSoundGroup = drSound.Volume;
            //playSoundParams.SpatialBlend = drSound.SpatialBlend;
            return soundComponent.PlaySound(AssetUtility.GetSoundAsset(assetName), "Sound", Constant.AssetPriority.SoundAsset, playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }

        public static int? PlayMusic(this SoundComponent soundComponent, string assetName, object userData = null)
        {
            soundComponent.StopMusic();

            //IDataTable<DRMusic> dtMusic = GameEntry.DataTable.GetDataTable<DRMusic>();
            //DRMusic drMusic = dtMusic.GetDataRow(musicId);
            //if (drMusic == null)
            //{
            //    Log.Warning("Can not load music '{0}' from data table.", musicId.ToString());
            //    return null;
            //}

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.FadeInSeconds = FadeVolumeDuration;
            playSoundParams.SpatialBlend = 0f;
            s_MusicSerialId = soundComponent.PlaySound(AssetUtility.GetMusicAsset(assetName), "Music", Constant.AssetPriority.MusicAsset, playSoundParams, null, userData);
            return s_MusicSerialId;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }
    }
}

