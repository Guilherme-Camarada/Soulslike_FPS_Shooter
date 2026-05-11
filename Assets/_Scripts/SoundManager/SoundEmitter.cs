using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public SoundData SoundData { get; private set; }
    private AudioSource _audioSource;
    private Coroutine _playingCoroutine;

    private Func<bool> _stopCondition;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(SoundData data)
    {
        SoundData = data;
        _audioSource.clip = data._audioClip;
        _audioSource.outputAudioMixerGroup = data._mixerGroup;
        _audioSource.loop = data._loop;
        _audioSource.playOnAwake = data._playOnAwake;
        _audioSource.volume = data._volume;
    }

    public void Play()
    {
        if (_playingCoroutine != null) 
        {
            StopCoroutine(_playingCoroutine);
        }

        _audioSource.Play();
        _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    public void Stop()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }

        _stopCondition = null;

        _audioSource.Stop();
        SoundManager.Instance.ReturnToPool(this);
    }

    private IEnumerator WaitForSoundToEnd()
    {
        while (_audioSource.isPlaying)
        {
            if (_stopCondition != null && _stopCondition.Invoke())
            {
                Stop();
                yield break;
            }

            yield return null;
        }

        _playingCoroutine = null;
        _stopCondition = null;
        SoundManager.Instance.ReturnToPool(this);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        _audioSource.pitch = 1f;
        _audioSource.pitch += Random.Range(min, max);
    }

    public void WithSetPitch(float pitch)
    {
        _audioSource.pitch = pitch;     
    }

    public void StopWhen(Func<bool> stopCondition)
    {
        _stopCondition = stopCondition;
    }
}
