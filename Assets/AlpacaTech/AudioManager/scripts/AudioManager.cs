using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace AlpacaTech
{
    /// <summary>
    /// 
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private const float MAX_DECIBEL = 0f;
        private const float MIN_DECIBEL = -80f;

        static public AudioManager se;
        static public AudioManager bgm;
        static public AudioManager voice;

        public enum Mode
        {
            Se = 0,
            Bgm,
            Voice,
        }
        public Mode mode = Mode.Se;
        public AudioMixerGroup audioMixerGroup;

        public List<AudioClipInfo> clips;
        List<AudioSource> audioSources = new List<AudioSource>();
        Dictionary<string, AudioSource> audioMap = new Dictionary<string, AudioSource>();

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            switch (mode)
            {
                case Mode.Se:
                    se = this;
                    break;
                case Mode.Bgm:
                    bgm = this;
                    break;
            }

            //
            //  clipからソースを生成
            //  
            for (int i = 0; i < clips.Count; ++i)
            {
                var source = this.gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.clip = clips[i].clip;
                source.outputAudioMixerGroup = audioMixerGroup;
                audioSources.Add(source);

                //  Key
                if (clips[i].id.Length != 0)
                {
                    audioMap.Add(clips[i].id, source);
                }
            }
        }

        /// <summary>
        /// マスターボリューム変更
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(float volume)
        {
            if (audioMixerGroup)
            {
                var mixer = audioMixerGroup.audioMixer;
                if(mixer)
                {
                    var decivel = Mathf.Clamp(20f * Mathf.Log10(volume), MIN_DECIBEL, MAX_DECIBEL);
                    mixer.SetFloat("Master", decivel);
                }
            }
        }

        /// <summary>
        /// グループボリューム変更
        /// </summary>
        /// <param name="volume"></param>
        public void SetGroupVolume(float volume)
        {
            if (audioMixerGroup)
            {
                var mixer = audioMixerGroup.audioMixer;
                if (mixer)
                {
                    var decivel = Mathf.Clamp(20f * Mathf.Log10(volume), MIN_DECIBEL, MAX_DECIBEL);
                    mixer.SetFloat(audioMixerGroup.name, decivel);
                }
            }
        }


        /// <summary>
        /// ボリューム変更。単体
        /// </summary>
        /// <param name="index"></param>
        /// <param name="volume"></param>
        public void SetVolume(int index, float volume)
        {
            if (audioSources[index])
            {
                audioSources[index].volume = volume;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float GetVolume(int index)
        {
            if (audioSources[index])
            {
                return audioSources[index].volume;
            }
            return 0;
        }

        /// <summary>
        /// 再生。インデックス指定
        /// </summary>
        /// <param name="index"></param>
        /// <param name="loop"></param>
        public void Play(int index, bool loop = false)
        {
            audioSources[index].loop = loop;
            audioSources[index].Play();
        }

        /// <summary>
        /// 再生。キー指定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loop"></param>
        public void Play(string id, bool loop = false)
        {
            audioMap[id].loop = loop;
            audioMap[id].Play();
        }

        /// <summary>
        /// 音楽再生一度だけ（ボタンイベント用）
        /// </summary>
        /// <param name="index"></param>
        public void PlayOnce(int index)
        {
            Play(index, false);
        }

        /// <summary>
        /// 音楽再生ループ（ボタンイベント用）
        /// </summary>
        /// <param name="index"></param>
        public void PlayLoop(int index)
        {
            Play(index, true);
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        /// <param name="index"></param>
        public void Stop(int index = -1)
        {
            if (index != -1)
            {
                audioSources[index].Stop();
            }
            else
            {
                for (int i = 0; i < audioSources.Count; ++i)
                {
                    audioSources[i].Stop();
                }
            }
        }

        /// <summary>
        /// 全体停止
        /// </summary>
        public void StopAll()
        {
            Stop(-1);
        }

        /// <summary>
        /// フェードアウト 1->0
        /// </summary>
        /// <param name="index"></param>
        public void FadeOut(int index)
        {
            StopCoroutine("FadeCoroutine");
            StartCoroutine("FadeCoroutine",true);
        }

        /// <summary>
        /// フェードイン 0->1
        /// </summary>
        /// <param name="index"></param>
        public void FadeIn(int index)
        {
            StopCoroutine("FadeCoroutine");
            StartCoroutine("FadeCoroutine", false);
        }

        /// <summary>
        /// フェードコルーチン
        /// </summary>
        /// <param name="fadeTime"></param>
        /// <param name="fadeOut"></param>
        /// <returns></returns>
        IEnumerator FadeCoroutine(float fadeTime, bool fadeOut)
        {
            float time = 0;
            while (true)
            {

                yield return null;
                time += Time.fixedUnscaledDeltaTime;

                if (time <= 0)
                {
                    break;
                }
            }

            yield break;
        }

    }

}


















