using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSwitchButton : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image buttonImage;

    private void Start()
    {
        UpdateButtonImage();
    }

    public void OnToggleSoundButtonClicked()
    {
        SoundManager.instance.OnAndOffBGM();
        UpdateButtonImage();
    }

    public void UpdateButtonImage()
    {
        if (SoundManager.instance == null || buttonImage == null) return;

        bool isMuted = SoundManager.instance.GetBgmMuteStatus();
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
