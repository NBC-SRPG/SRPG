using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTab : UIBase
{
    private enum Sliders
    {
        MasterSlider,
        BgmSlider,
        EffectSlider
    }

    private enum Toggles
    {
        MasterToggle,
        BgmToggle,
        EffectToggle
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Bind<Slider>(typeof(Sliders));
        Bind<Toggle>(typeof(Toggles));

        // 슬라이더에는 볼륨 값 세팅 함수 연결
        Get<Slider>((int)Sliders.MasterSlider).onValueChanged.AddListener(SetMasterVolume);
        Get<Slider>((int)Sliders.BgmSlider).onValueChanged.AddListener(SetBgmVolume);
        Get<Slider>((int)Sliders.EffectSlider).onValueChanged.AddListener(SetEffectVolume);
        
        // 토글에는 볼륨 뮤트 함수 연결
        Get<Toggle>((int)Toggles.MasterToggle).onValueChanged.AddListener(MuteMasterVolume);
        Get<Toggle>((int)Toggles.BgmToggle).onValueChanged.AddListener(MuteBgmVolume);
        Get<Toggle>((int)Toggles.EffectToggle).onValueChanged.AddListener(MuteEffectVolume);
    }

    private void SetMasterVolume(float volume)
    {
        Debug.Log("SetMasterVolume");

        Managers.Sound.SetMasterVolume(volume);
    }

    private void SetBgmVolume(float volume)
    {
        Debug.Log("SetBgmVolume");

        Managers.Sound.SetSoundVolume(Constants.Sound.Bgm, volume);
    }

    private void SetEffectVolume(float volume)
    {
        Debug.Log("SetEffectVolume");

        Managers.Sound.SetSoundVolume(Constants.Sound.Effect, volume);
    }
    private void MuteMasterVolume(bool mute)
    {
        Debug.Log("MuteMasterVolume");

        Managers.Sound.MuteMasterVolume(mute);
    }

    private void MuteBgmVolume(bool mute)
    {
        Debug.Log("MuteBgmVolume");

        Managers.Sound.MuteSoundVolume(Constants.Sound.Bgm, mute);
    }

    private void MuteEffectVolume(bool mute)
    {
        Debug.Log("MuteEffectVolume");

        Managers.Sound.MuteSoundVolume(Constants.Sound.Effect, mute);
    }
}
