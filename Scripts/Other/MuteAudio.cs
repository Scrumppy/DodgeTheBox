using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioToMute;
    [SerializeField]
    private AudioMixer masterMixer; 
    [SerializeField]
    private GameObject imageHolderObject;

    public void MuteMusicToggle(bool muted)
    {
        if (muted)
        {
            imageHolderObject.GetComponent<Image>().enabled = false;
            SetMusicVolume(-80f);
            //audioToMute.Pause();
        }
        else
        {
            imageHolderObject.GetComponent<Image>().enabled = true;
            SetMusicVolume(0f);
            //audioToMute.Play();
        }
    }

    public void MuteSoundToggle(bool muted)
    {
        if (muted)
        {
            imageHolderObject.GetComponent<Image>().enabled = false;
            SetSoundVolume(-80f);
        }
        else
        {
            imageHolderObject.GetComponent<Image>().enabled = true;
            SetSoundVolume(0f);
        }
    }

    public void SetMusicVolume(float musicVolume)
    {
        masterMixer.SetFloat("musicVol", musicVolume);
    }

    public void SetSoundVolume(float soundVolume)
    {
        masterMixer.SetFloat("sfxVol", soundVolume);
    }
}
