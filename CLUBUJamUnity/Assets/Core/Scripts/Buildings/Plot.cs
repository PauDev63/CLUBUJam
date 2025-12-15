using UnityEngine;

public class Plot : Building
{
    [SerializeField] private GameObject _building;

    public override void DoEffectAfterWork()
    {
        ToggleUpgradeMode();

        // TEMPORAL
        //UIManager.Instance.HideSelectedUI();
        EventHolder.Instance.onUpdateGameUI?.Invoke();

        ConstructBuilding();
    }

    private void ConstructBuilding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, 100f))
        {
            Vector3 spawnPos = hit.point;
            spawnPos.y -= 0.5f; // hunde un poco en el suelo
            Instantiate(_building, spawnPos, Quaternion.identity, transform.parent);
        }

        //_workerFSM.StopCurrentTask();
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

    public Sprite GetFutureBuilding()
    {
        return _building.GetComponent<Building>().GetBuildingSprite();
    }

    public Resource GetFutureResource()
    {
        return _building.GetComponent<Building>().GetResourceEnumGenerated();
    }

    public int GetFutureQuantitiesGenerated()
    {
        return _building.GetComponent<Building>().GetAmountGenereted();
    }


}
