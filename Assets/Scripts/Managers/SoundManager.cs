using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource 배열 (BGM, 효과음을 따로 틀기 위한 용도)
    private AudioSource[] audioSources = new AudioSource[(int)Constants.Sound.Max];
    // 효과음 AudioClip 캐싱 딕셔너리
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    // AudioSource 오브젝트의 부모 (빈 오브젝트)
    private GameObject soundRoot = null;
    // 마스터 볼륨 값
    private float masterVolume = 1f;
    // BGM, Effect 볼륨 값
    private Dictionary<Constants.Sound, float> soundVolumes = new Dictionary<Constants.Sound, float>();
    private bool masterMute = false;
    // BGM, Effect 음소거 상태인지 체크
    private Dictionary<Constants.Sound, bool> soundMutes = new Dictionary<Constants.Sound, bool>();
    public void Init()
    {
        if (soundRoot == null)
        {
            // 씬에서 @SoundRoot 찾기
            soundRoot = GameObject.Find("@SoundRoot");
            // 없으면
            if (soundRoot == null)
            {
                // 새로 만들기
                soundRoot = new GameObject { name = "@SoundRoot" };
                // 씬 바뀌어도 파괴되지 않도록
                UnityEngine.Object.DontDestroyOnLoad(soundRoot);
                // Sound 타입의 값을 가져와서
                string[] soundTypeNames = Enum.GetNames(typeof(Constants.Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    // 해당 이름의 오브젝트 생성 후
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    // AudioSource 컴포넌트 추가
                    audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = soundRoot.transform;
                }
                // BGM AudioSource는 loop 효과
                audioSources[(int)Constants.Sound.Bgm].loop = true;
                // 볼륨 딕셔너리에 추가 후 볼륨 1로 세팅 & 음소거 상태 아님 체크
                // TODO 볼륨, 음소거 세팅 시 데이터 저장 후 불러오기
                foreach (Constants.Sound sound in Enum.GetValues(typeof(Constants.Sound)))
                {
                    if (sound != Constants.Sound.Max)
                    {
                        soundVolumes[sound] = 1f;
                        soundMutes[sound] = false;
                    }
                }
            }
        }
    }
    // 모든 AudioSource 스탑 & AudioClip 딕셔너리 클리어
    // 씬이 넘어갈 때 클리어 후 해당 씬에 맞는 BGM 재생
    public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
    // Sound 타입, Sound 이름을 받아 재생, 필요 시 pitch 값 설정 가능
    public bool Play(Constants.Sound type, string path, float pitch = 1.0f)
    {
        // 타입에 맞는 AudioSource 선택
        AudioSource audioSource = audioSources[(int)type];            
        // pitch 설정
        audioSource.pitch = pitch;

        if (type == Constants.Sound.Bgm)
        {
            // BGM이라면 바로 Load
            AudioClip audioClip = Managers.Resource.Load<AudioClip>($"Sounds/{path}");
            // 없으면 false
            if (audioClip == null)
            {
                return false;
            }
            // 재생하고 있는 오디오가 있다면 중지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // Load한 clip 적용
            audioSource.clip = audioClip;
            // 재생
            audioSource.Play();
            return true;
        }
        else if (type == Constants.Sound.Effect)
        {
            // 효과음은 캐싱 적용 
            AudioClip audioClip = GetAudioClip($"Sounds/{path}");
            // 없으면 false
            if (audioClip == null)
            {
                return false;
            }
            // 효과음은 한번만 재생
            audioSource.PlayOneShot(audioClip);
            return true;
        }

        return false;
    }
    // 세팅에서 마스터 볼륨 설정 시 적용
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        ApplyVolume();
    }
    // 세팅에서 Bgm, Effect 볼륨 설정 시 적용
    public void SetSoundVolume(Constants.Sound sound, float volume)
    {
        soundVolumes[sound] = volume;
        ApplyVolume();
    }
    // 마스터 볼륨 음소거
    public void MuteMasterVolume(bool mute)
    {
        // 음소거 시
        if (mute)
        {
            // 모든 오디오소스 볼륨 0으로 세팅
            foreach (Constants.Sound sound in Enum.GetValues(typeof(Constants.Sound)))
            {
                if (sound != Constants.Sound.Max)
                {
                    audioSources[(int)sound].volume = 0f;
                }
            }

            masterMute = true;
        }
        else
        {
            masterMute = false;

            // 음소거 해제 시 기존 값으로 적용
            ApplyVolume();
        }
    }
    // 사운드 볼륨 음소거
    public void MuteSoundVolume(Constants.Sound sound, bool mute)
    {
        if (mute)
        {
            audioSources[(int)sound].volume = 0f;
            soundMutes[sound] = true;
        }
        else
        {
            soundMutes[sound] = false;
            ApplyVolume();
        }
    }

    // 볼륨 적용
    private void ApplyVolume()
    {
        foreach (Constants.Sound sound in Enum.GetValues(typeof(Constants.Sound)))
        {
            if (sound != Constants.Sound.Max)
            {
                // 마스터 음소거 or sound 음소거 시 볼륨 세팅 하지 않음
                if (masterMute || soundMutes[sound])
                {
                    continue;
                }

                audioSources[(int)sound].volume = masterVolume * soundVolumes[sound];
            }
        }
    }
        
    // Sound 타입을 받아 해당 타입의 AudioSource 중지
    public void Stop(Constants.Sound type)
    {
        audioSources[(int)type].Stop();
    }

    // path에 해당하는 AudioClip 가져오기
    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;
        // 캐싱 된 것이 있다면 return
        if (_audioClips.TryGetValue(path, out audioClip))
        {
            return audioClip;
        }
        // 없으면 Load & 딕셔너리에 추가 후 return
        audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add(path, audioClip);
        return audioClip;
    }
}
