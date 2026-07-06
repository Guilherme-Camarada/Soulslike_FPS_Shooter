using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemiesLeftText;

    private float _waveCount;

    private void OnEnable()
    {
        _waveSpawner.OnWaveStartAction += WaveSpawner_OnWaveStartAction;
        _gameManager.OnStateChangedAction += GameManager_OnStateChangedAction;

        _waveSpawner.OnEnemyKilledAction += WaveSpawner_OnEnemyKilledAction;
    }

    private void OnDisable()
    {
        _waveSpawner.OnWaveStartAction -= WaveSpawner_OnWaveStartAction;
        _gameManager.OnStateChangedAction -= GameManager_OnStateChangedAction;
        _waveSpawner.OnEnemyKilledAction -= WaveSpawner_OnEnemyKilledAction;
    }

    private void GameManager_OnStateChangedAction(GameState obj)
    {
        if (obj == GameState.ChoosingUpgrade)
        {
            _waveText.text = "Choose an upgrade!";
            _enemiesLeftText.text = "";
        }
    }

    private void WaveSpawner_OnWaveStartAction()
    {
        _waveCount++;
        _waveText.text = $"Wave {_waveCount}";
        _enemiesLeftText.text = $"ENEMIES LEFT {_waveSpawner.GetCurrentWaveData().WaveEndKillCount}";
    }

    private void WaveSpawner_OnEnemyKilledAction(int enemiesLeft)
    {
        _enemiesLeftText.text = $"ENEMIES LEFT {enemiesLeft}";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waveText.text = "Pick a Weapon!";
        _enemiesLeftText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
