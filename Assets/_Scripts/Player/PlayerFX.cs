using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerFX : MonoBehaviour
{
    [Header("References")]
    private AudioSource _audioSource;
    [SerializeField] private Volume _globalVolume;

    [SerializeField] private GameInput _gameInput;
    private PlayerMovement _playerMovement;

    [Header("AudioFX")]
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _landingAudio;
    [SerializeField] private AudioClip _dashAudio;
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] private float _walkStepInterval = 0.5f;
    [SerializeField] private float _sprintStepInterval = 0.3f;

    [Header("VisualFX")]
    [SerializeField] private ParticleSystem _jumpParticles;

    private bool _isDashing;
    private float _stepTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _playerMovement.OnJumpAction += PlayerMovement_OnJumpAction;
        _playerMovement.OnGroundedAction += PlayerMovement_OnGroundedAction;
        _gameInput.OnSprintStartAction += GameInput_OnSprintStartAction;
        _gameInput.OnSprintCancelAction += GameInput_OnSprintCancelAction;
 
        
        _playerMovement.OnDashAction += PlayerMovement_OnDashAction;
    }

    private void PlayerMovement_OnGroundedAction(float timeInAir)
    {
        if (timeInAir > 0.2f)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_landingAudio);
        }
        
    }

    private void PlayerMovement_OnDashAction(bool obj)
    {
        _isDashing = obj;

        _globalVolume.profile.TryGet(out ChromaticAberration chromaticAberration);
        _globalVolume.profile.TryGet(out MotionBlur motionBlur);


        if (_isDashing)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_dashAudio);

            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = 1f;
            }
            if (motionBlur != null)
            {
                motionBlur.active = true;
                motionBlur.intensity.value = 1f;
            }
        }
        else
        {
            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = 0f;
            }
            if (motionBlur != null)
            {
                motionBlur.active = false;
                motionBlur.intensity.value = 0f;
            }
        }

        

    }

    private void GameInput_OnSprintCancelAction()
    {
        
    }

    private void GameInput_OnSprintStartAction()
    {
       
    }

    private void PlayerMovement_OnJumpAction()
    {
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_jumpAudio);
        
        
    }

    private void OnDisable()
    {
        _playerMovement.OnJumpAction -= PlayerMovement_OnJumpAction;
        _playerMovement.OnGroundedAction -= PlayerMovement_OnGroundedAction;
        _gameInput.OnSprintStartAction -= GameInput_OnSprintStartAction;
        _gameInput.OnSprintCancelAction -= GameInput_OnSprintCancelAction;
        _playerMovement.OnDashAction -= PlayerMovement_OnDashAction;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
 
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (_stepTimer > 0f)
        {
            _stepTimer -= Time.deltaTime;
        }

        if (_playerMovement.IsWalking() && _playerMovement.IsGrounded() && !_playerMovement.IsDashing())
        {
            if (_stepTimer <= 0f)
            {
                PlayRandomFootstep();

                _stepTimer = _playerMovement.IsSprinting() ? _sprintStepInterval : _walkStepInterval;
            }
        }
    }

    private void PlayRandomFootstep()
    {
        if (_footstepSounds.Length == 0) return;

        int randomIndex = Random.Range(0, _footstepSounds.Length);
        AudioClip selectedStep = _footstepSounds[randomIndex];

        _audioSource.pitch = Random.Range(0.85f, 1.15f);

        _audioSource.PlayOneShot(selectedStep);
    }
}
