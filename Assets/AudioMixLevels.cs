using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixLevels : MonoBehaviour
{

    public AudioMixer mixer;

  
    public void SetLvlMusic(float nMusic)
    {
        mixer.SetFloat("Music", nMusic);
    }
    public void SetLvlEffect(float nEffect)
    {
        mixer.SetFloat("SoundEffects", nEffect);
    }

}
