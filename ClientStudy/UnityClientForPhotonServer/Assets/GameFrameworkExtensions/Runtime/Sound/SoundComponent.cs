/*
 * Author:     NOW
 * CreateTime:  20171127
 * Description:
 * 
*/
using UnityEngine;
using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 声音组件。
    /// </summary>
    public sealed partial class SoundComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="tran">音源挂载点</param>
        /// <param name="name">音源名</param>
        /// <param name="isBGM">是否为背景音乐</param>
        /// <param name="playOnAwake">是否激活时播放</param>
        /// <param name="isLoop">是否循环播放</param>
        public void PlaySound(Transform tran, string name, AudioClip audioClip, bool isPlayOnAwake = false, bool isLoop = false, float initVolume = 1.0f)
        {
            bool bPlay = false;
            AudioSource[] sources = tran.GetComponents<AudioSource>();
            if (sources != null)
            {
                for (int i = 0; i < sources.Length; ++i)
                {
                    if (!sources[i].isPlaying)
                        Destroy(sources[i]);
                    if (bPlay)
                        continue;
                    if (sources[i].isPlaying)
                    {
                        bPlay = true;
                        continue;
                    }
                    if (sources[i].isPlaying)
                    {
                        Destroy(sources[i]);
                    }
                }
            }
            if (bPlay)
                return;
            AudioSource direct = tran.gameObject.GetOrAddComponent<AudioSource>();
            AudioClip tryGetAudioClip = audioClip;
            tryGetAudioClip.name = name;
            if (tryGetAudioClip != null)
            {
                direct.clip = tryGetAudioClip;
                direct.volume = initVolume;
                direct.playOnAwake = isPlayOnAwake;
                direct.loop = isLoop;
                direct.Play();
            }
        }

        /// <summary>
        /// 关闭声音
        /// </summary>
        /// <param name="tran">音源挂载点</param>
        /// <param name="name">音源名</param>
        public void StopSound(Transform tran, string name = "")
        {
            AudioSource[] sources = tran.GetComponents<AudioSource>();
            if (sources != null)
            {
                if (name != "")
                {
                    for (int i = 0; i < sources.Length; ++i)
                    {
                        if (sources[i].clip.name.ToLower() == name)
                            Destroy(sources[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < sources.Length; ++i)
                    {
                        Destroy(sources[i]);
                    }
                }
            }
        }
    }
}