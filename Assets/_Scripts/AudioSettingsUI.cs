using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _masterSlider;

    [SerializeField] private GameObject _settingsUI;

    [Header("Exposed Parameter Names")]
    [SerializeField] private string _musicParameterName = "MusicVolume";
    [SerializeField] private string _sfxParameterName = "SFXVolume";
    [SerializeField] private string _masterParameterName = "MasterVolume";

    private void OnEnable()
    {
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        _masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
    }

    private void Start()
    {
        if (_audioMixer.GetFloat(_musicParameterName, out float musicCurrentDecibels))
        {
            _musicSlider.value = Mathf.Pow(10, musicCurrentDecibels / 20f);
        }
        if (_audioMixer.GetFloat(_sfxParameterName, out float sfxCurrentDecibels))
        {
            _sfxSlider.value = Mathf.Pow(10, sfxCurrentDecibels / 20f);
        }
        if (_audioMixer.GetFloat(_masterParameterName, out float masterCurrentDecibels))
        {
            _masterSlider.value = Mathf.Pow(10, masterCurrentDecibels / 20f);
        }
    }

    private void SetMusicVolume(float value) => SetVolume(_musicParameterName, value);
    private void SetSFXVolume(float value) => SetVolume(_sfxParameterName, value);
    private void SetMasterVolume(float value) => SetVolume(_masterParameterName, value);


    private void SetVolume(string parameterName, float sliderValue)
    {
        float safeValue = Mathf.Max(sliderValue, 0.0001f);

        float decibelValue = Mathf.Log10(safeValue) * 20f;
        _audioMixer.SetFloat(parameterName, decibelValue);
    }

    public void OpenSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(true);

            _settingsUI.transform.DOKill();

            _settingsUI.transform.localScale = Vector3.zero;

            _settingsUI.transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
        
    }

    public void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.transform.DOKill();

            _settingsUI.transform.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _settingsUI.SetActive(false);
                });
        }


        }
}