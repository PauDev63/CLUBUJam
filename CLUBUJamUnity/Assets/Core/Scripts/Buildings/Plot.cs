using UnityEngine;

public class Plot : Building
{
    [SerializeField] private GameObject _building;

    public override void DoEffectAfterWork()
    {
        ToggleUpgradeMode();

        // TEMPORAL
        UIManager.Instance.HideSelectedUI();

        ConstructBuilding();
    }

    private void ConstructBuilding()
    {
        Instantiate(_building, transform.position, Quaternion.identity, transform.parent);
        _workerFSM.StopCurrentTask();
        Destroy(gameObject);
    }

    public bool HasBuildingResources()
    {
        for (int i = 0; i < _generation[0].resourcesRequiredForUpgrading.Length; i++)
        {
            //if(!(quantitiesDroppedForUpgrading[i] >= _generation[_upgradeLevel - 1].quantitiesRequiredForUpgrading[i]))
            if (!ResourcesManager.Instance.HasEnough(_generation[0].resourcesRequiredForUpgrading[i], _generation[0].quantitiesRequiredForUpgrading[i]))
            {
                return false;
            }
        }

        return true;
    }

}
