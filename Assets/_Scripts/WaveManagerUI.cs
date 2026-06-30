using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private TextMeshProUGUI _waveText;

    private float _waveCount;

    private void OnEnable()
    {
        _waveSpawner.OnWaveStartAction += WaveSpawner_OnWaveStartAction;
        _gameManager.OnStateChangedAction += GameManager_OnStateChangedAction;
    }

    private void GameManager_OnStateChangedAction(GameState obj)
    {
        if (obj == GameState.ChoosingUpgrade)
        {
            _waveText.text = "Choose an upgrade!";
        }
    }

    private void WaveSpawner_OnWaveStartAction()
    {
        _waveCount++;
        _waveText.text = $"Wave {_waveCount}";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waveText.text = "Pick a Weapon!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
