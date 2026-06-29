using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeReference] private List<UpgradeEffect> _upgradeEffectList;

    [SerializeField] private string _upgradeName;

    [TextArea]
    [SerializeField] private string _upgradeFlavour;

    public string GetUpgradeName()
    {
        return _upgradeName;
    }

    public string GetUpgradeFlavour()
    {
        return _upgradeFlavour;
    }

    public List<UpgradeEffect> GetUpgradeEffectList()
    {
        return _upgradeEffectList;
    }
}
