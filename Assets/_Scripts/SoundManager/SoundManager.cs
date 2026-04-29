using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private IObjectPool<SoundEmitter> _soundEmitterPool;
    private readonly List<SoundEmitter> _activeEmitters = new();
    public readonly Dictionary<SoundData, int> Counts = new();

    [Header("Sound Emitter Prefab")]
    [SerializeField] private SoundEmitter _soundEmitterPrefab;

    [Header("Pool Settings")]
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private int _maxSoundInstances = 30;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializePool();
    }

    public SoundBuilder CreateSound()
    {
        return new SoundBuilder(this);
    }

    public bool CanPlaySound(SoundData data)
    {
        if (Counts.TryGetValue(data, out var count))
        {
            if (count >= _maxSoundInstances)
            {
                return false;
            }
        }
        return true;
    }

    public SoundEmitter Get()
    {
        return _soundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter emitter)
    {
        _soundEmitterPool.Release(emitter);
    }

    private void InitializePool()
    {
        _soundEmitterPool = new ObjectPool<SoundEmitter>(
            CreateSoundEmitter, 
            OnTakeFromPool, 
            OnReturnedToPool, 
            OnDestroyPoolObject,
            _collectionCheck,
            _defaultCapacity,
            _maxPoolSize);
    }

    private SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter soundEmitter = Instantiate(_soundEmitterPrefab);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }

    private void OnTakeFromPool(SoundEmitter emitter)
    {
        emitter.gameObject.SetActive(true);
        _activeEmitters.Add(emitter);
    }

    private void OnReturnedToPool(SoundEmitter emitter)
    {
        if (Counts.TryGetValue(emitter.SoundData, out var count))
        {
            Counts[emitter.SoundData] = count - 1;
        }
        else
        {
            Counts[emitter.SoundData] = 0;
        }
        emitter.gameObject.SetActive(false);
        _activeEmitters.Remove(emitter);
    }

    private void OnDestroyPoolObject(SoundEmitter emitter)
    {
        Destroy(emitter.gameObject);
    }
}
