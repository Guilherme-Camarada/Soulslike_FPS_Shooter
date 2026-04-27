using UnityEngine;

public class SoundBuilder
{
    private readonly SoundManager _soundManager;
    private SoundData _soundData;
    private Vector3 _soundPosition = Vector3.zero;
    private bool _randomPitch;
    private bool _setPitch;
    private float _pitch;

    public SoundBuilder(SoundManager soundManager)
    {
        _soundManager = soundManager; 
    }
    
    public SoundBuilder WithSoundData(SoundData soundData)
    {
        _soundData = soundData;
        return this;
    }

    public SoundBuilder WithPosition(Vector3 position)
    {
        _soundPosition = position;
        return this;
    }

    public SoundBuilder WithRandomPitch()
    {
        this._randomPitch = true;
        return this;
    }

    public SoundBuilder WithSetPitch(float value)
    {
       _setPitch = true;
        _pitch = value;
        return this;
    }

    public void Play()
    {
        if (!_soundManager.CanPlaySound(_soundData))
        {
            return;
        }

        SoundEmitter soundEmitter = _soundManager.Get();
        soundEmitter.Initialize(_soundData);
        soundEmitter.transform.position = _soundPosition;
        soundEmitter.transform.parent = _soundManager.transform;

        if (_randomPitch)
        {
            soundEmitter.WithRandomPitch();
        }
        if (_setPitch)
        {
            soundEmitter.WithSetPitch(_pitch);
        }

        if (_soundManager.Counts.TryGetValue(_soundData, out var count))
        {
            _soundManager.Counts[_soundData] = count + 1;
        } else
        {
            _soundManager.Counts[_soundData] = 1;
        }

        soundEmitter.Play();
    }
}
