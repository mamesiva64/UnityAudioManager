using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaTech
{
    /// <summary>
    /// コードサンプル
    /// </summary>
    public class AudioManagerSample : MonoBehaviour
    {

        public void Sample()
        {
            AlpacaTech.AudioManager.se.Play(0);

            AlpacaTech.AudioManager.bgm.PlayLoop(0);


        }

    }
}
