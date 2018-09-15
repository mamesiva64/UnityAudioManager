using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaTech
{
    public class AudioManagerPlay : MonoBehaviour
    {
        [SerializeField] int id;
        [SerializeField] bool loop = false;
        [SerializeField] bool playOnAwake = true;
        [SerializeField] AudioManager.Mode mode = AudioManager.Mode.Se;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            switch (mode)
            {
                case AudioManager.Mode.Se:
                    AudioManager.se.Play(id, loop);
                    break;
                case AudioManager.Mode.Bgm:
                    AudioManager.bgm.Play(id, loop);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {

        }
    }
}

