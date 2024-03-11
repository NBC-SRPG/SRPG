using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource 배열 (BGM, 효과음을 따로 틀기 위한 용도)
    private AudioSource[] _audioSources = new AudioSource[(int)Constants.Sound.Max];
    // 효과음 AudioClip 캐싱 딕셔너리
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    // AudioSource 오브젝트의 부모 (빈 오브젝트)
    private GameObject _soundRoot = null;

    public void Init()
    {
        if (_soundRoot == null)
        {
            // 씬에서 @SoundRoot 찾기
            _soundRoot = GameObject.Find("@SoundRoot");
            // 없으면
            if (_soundRoot == null)
            {
                // 새로 만들기
                _soundRoot = new GameObject { name = "@SoundRoot" };
                // 씬 바뀌어도 파괴되지 않도록
                UnityEngine.Object.DontDestroyOnLoad(_soundRoot);
                // Sound 타입의 값을 가져와서
                string[] soundTypeNames = Enum.GetNames(typeof(Constants.Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    // 해당 이름의 오브젝트 생성 후
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    // AudioSource 컴포넌트 추가
                    _audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }
                // BGM AudioSource는 loop 효과
                _audioSources[(int)Constants.Sound.Bgm].loop = true;
            }
        }
    }
    // 모든 AudioSource 스탑 & AudioClip 딕셔너리 클리어
    // 씬이 넘어갈 때 클리어 후 해당 씬에 맞는 BGM 재생
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
    // Sound 타입, Sound 이름을 받아 재생, 필요 시 volume, pitch 값 설정 가능
    public bool Play(Constants.Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
    {
        // 타입에 맞는 AudioSource 선택
        AudioSource audioSource = _audioSources[(int)type];            
        // volume, pitch 설정
        audioSource.volume = volume;
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
    // Sound 타입을 받아 해당 타입의 AudioSource 중지
    public void Stop(Constants.Sound type)
    {
        _audioSources[(int)type].Stop();
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
