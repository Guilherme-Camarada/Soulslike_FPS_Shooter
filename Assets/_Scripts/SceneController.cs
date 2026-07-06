using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [SerializeField] private Image _transitionImage;
    [SerializeField] private float _fadeDuration = 0.5f;

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

    public void ChangeScene(int sceneIndex)
    {
        _transitionImage.gameObject.SetActive(true);

        _transitionImage.DOKill();

        _transitionImage.DOFade(1f, _fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

            loadOperation.completed += operation =>
            {
                _transitionImage.DOKill();

                Debug.Log("Before fade out");

                _transitionImage.DOFade(0f, _fadeDuration).SetDelay(1f).SetUpdate(true).OnComplete(() =>
                {
                    _transitionImage.gameObject.SetActive(false);
                    Debug.Log("Reacheed active falde");
                });
            };
        });   
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
