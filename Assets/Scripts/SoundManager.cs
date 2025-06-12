using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource bgm_player;
    AudioSource sfx_player;

    public Slider bgm_slider;
    public Slider sfx_slider;

    public AudioClip[] audio_clips;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        bgm_slider = bgm_slider.GetComponent<Slider>();
        sfx_slider = sfx_slider.GetComponent<Slider>();

        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);


        bgm_player = GameObject.Find("BGM_Player").GetComponent <AudioSource>();
        sfx_player = GameObject.Find("SFX_Player").GetComponent <AudioSource>();
    }

    public void OnAndOffBGM()
    {
        bgm_player.mute = !bgm_player.mute;

        SoundSwitchButton toggleButton = FindObjectOfType<SoundSwitchButton>();
        if (toggleButton != null)
        {
            toggleButton.UpdateButtonImage();
        }
    }
    public bool GetBgmMuteStatus()
    {
        return bgm_player != null && bgm_player.mute;
    }
    public void SetBgmMuteStatus(bool isMuted)
    {
        if (bgm_player != null)
        {
            bgm_player.mute = isMuted;

            SoundSwitchButton toggleButton = FindObjectOfType<SoundSwitchButton>();
            if (toggleButton != null)
                toggleButton.UpdateButtonImage();
        }
    }


    void ChangeBgmSound(float value)
    {
        GameManager_ScrapLand.instance?.SetBgmVolume(value);
    }

    void ChangeSfxSound(float value)
    {
        GameManager_ScrapLand.instance?.SetSfxVolume(value);
    }


    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Click": index = 0; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.Play();
    }

    public void SetBgmVolume(float value)
    {
        if (bgm_player != null)
            bgm_player.volume = value;
        if (bgm_slider != null)
            bgm_slider.value = value;
    }

    public void SetSfxVolume(float value)
    {
        if (sfx_player != null)
            sfx_player.volume = value;
        if (sfx_slider != null)
            sfx_slider.value = value;
    }

}
