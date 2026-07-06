using DG.Tweening;
using System.Security.Cryptography;
using UnityEditor.Rendering;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private SoundData _onPauseSoundData;

    private bool _isPaused;

    private void OnEnable()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction()
    {
        if (!_isPaused)
        {
            Pause(); 
        } else
        {
            Unpause();
        }
        
    }

    private void OnDisable()
    {
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
    }

    public void Pause()
    {
        SoundManager.Instance.CreateSound()
                .WithSoundData(_onPauseSoundData)
                .WithRandomPitch()
                .Play();

        _isPaused = true;

        _pauseMenu.SetActive(true);
        _pauseMenu.transform.localScale = Vector3.zero;

        _pauseMenu.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBounce).SetUpdate(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Unpause()
    {
        SoundManager.Instance.CreateSound()
                .WithSoundData(_onPauseSoundData)
                .WithRandomPitch()
                .Play();

        _isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _pauseMenu.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBounce).SetUpdate(true)
            .OnComplete(() =>
            {
                _pauseMenu.SetActive(false);
            });
    }

    public void UnlockAndUnpause()
    {
        _isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _pauseMenu.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBounce).SetUpdate(true)
            .OnComplete(() =>
            {
                _pauseMenu.SetActive(false);
            });
    }
}
