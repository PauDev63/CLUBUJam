using UnityEngine;

public class Building : MonoBehaviour
{
    [Tooltip("Add each level on the array to cycle on upgrade")]
    [SerializeField] private Generation[] _generation;
    [SerializeField] private int _upgradeLevel;

    private void Start()
    {
        EventHolder.Instance.onTick.AddListener(TryUpgrade);
    }

    public void GenerateResource()
    {
        ResourcesManager.Instance.AddResource(_generation[_upgradeLevel - 1].resourceGenerated, _generation[_upgradeLevel - 1].quantityGenerated);
    }

    public void TryUpgrade()
    {
        if (CanUpgrade())
        {
            Upgrade();
        }
    }

    private bool CanUpgrade()
    {
        if(_upgradeLevel < _generation.Length)
            return HasUpgradingResources();

        return false;
    }

    private bool HasUpgradingResources()
    {
        for(int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForUpgrading.Length; i++)
        {
            if(!ResourcesManager.Instance.HasEnough(_generation[_upgradeLevel - 1].resourcesRequiredForUpgrading[i], _generation[_upgradeLevel - 1].quantitiesRequiredForUpgrading[i]))
            {
                return false;
            }
        }

        return true;
    }

    private void Upgrade()
    {
        // TODO - Add VFX and SFX on Upgrade
        for (int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForUpgrading.Length; i++)
        {
            ResourcesManager.Instance.SubtractResource(_generation[_upgradeLevel - 1].resourcesRequiredForUpgrading[i], _generation[_upgradeLevel - 1].quantitiesRequiredForUpgrading[i]);
        }
        _upgradeLevel++;
        //Debug.Log($"Building {gameObject.name} has been upgraded to level {_upgradeLevel}.");
    }
}
