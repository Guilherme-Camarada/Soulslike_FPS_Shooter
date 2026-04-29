using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundData
{
    public AudioClip _audioClip;
    public AudioMixerGroup _mixerGroup;
    public bool _loop;
    public bool _playOnAwake;
    public float _volume = 1f;
    
}
