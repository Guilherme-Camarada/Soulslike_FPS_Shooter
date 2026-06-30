using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    private Upgrade _upgrade;

    [SerializeField] private TextMeshProUGUI _upgradeNameText;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _upgradeFlavourText;

    private Transform _lookAtTransform;

    private void Awake()
    {
        _upgrade = GetComponent<Upgrade>();
    }

    private void Start()
    {
        _lookAtTransform = Camera.main.transform;

        _upgradeNameText.text = _upgrade.GetUpgradeName();

        _upgradeDescriptionText.text = "";

        foreach (UpgradeEffect effect in _upgrade.GetUpgradeEffectList())
        {
            _upgradeDescriptionText.text += effect.GetDescription() + " ";
        }

        _upgradeFlavourText.text = _upgrade.GetUpgradeFlavour();
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _lookAtTransform.rotation * Vector3.forward, _lookAtTransform.rotation * Vector3.up);
    }



}
