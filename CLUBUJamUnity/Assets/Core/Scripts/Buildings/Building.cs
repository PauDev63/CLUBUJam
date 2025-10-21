using System;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [Tooltip("Add each level on the array to cycle on upgrade")]
    [SerializeField] private Generation[] _generation;
    [SerializeField] private int _upgradeLevel;
    [SerializeField] private Task _task;
    [SerializeField] private WorkerFSM _workerFSM;
    [SerializeField] private int[] quantitiesDropped;
    [SerializeField] private int quantityGenerated;
    [SerializeField] private int quantityNeeded;

    public int QuantityGenerated { get { return quantityGenerated; } }
    public int QuantityNeeded {  get { return quantityNeeded; } }

    private void Start()
    {
        quantitiesDropped = new int[_generation[_upgradeLevel - 1].quantitiesRequiredForGeneration.Length];
        CalculateQuantitiesNeeded();
    }

    private void CalculateQuantitiesNeeded()
    {
        foreach (int quantity in _generation[_upgradeLevel - 1].quantitiesRequiredForGeneration)
            quantityNeeded += quantity;
    }

    public void Interact()
    {
        _workerFSM.QueueTask(_task);
    }

    public void GenerateResource()
    {
        for (int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForGeneration.Length; i++)
        {
            quantitiesDropped[i] -= _generation[_upgradeLevel - 1].quantitiesRequiredForGeneration[i];
        }
        quantityGenerated += _generation[_upgradeLevel - 1].quantityGenerated;
        //ResourcesManager.Instance.AddResource(_generation[_upgradeLevel - 1].resourceGenerated, _generation[_upgradeLevel - 1].quantityGenerated);
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
        CalculateQuantitiesNeeded();
        //Debug.Log($"Building {gameObject.name} has been upgraded to level {_upgradeLevel}.");
    }

    public Resource GetRequiredResource()
    {
        for(int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForGeneration.Length; i++)
        {
            if (quantitiesDropped[i] == _generation[_upgradeLevel - 1].quantitiesRequiredForGeneration[i])
                continue;
            else
            {
                return _generation[_upgradeLevel - 1].resourcesRequiredForGeneration[i];
            }
        }

        return Resource.None;
    }

    public void DropRequiredResource(Resource resource)
    {
        for (int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForGeneration.Length; i++)
        {
            if (_generation[_upgradeLevel - 1].resourcesRequiredForGeneration[i].Equals(resource))
            {
                if (quantitiesDropped[i] < _generation[_upgradeLevel - 1].quantitiesRequiredForGeneration[i])
                {
                    quantitiesDropped[i]++;
                    break;
                }
            }
        }
    }

    public Resource GetGeneratedResource()
    {
        quantityGenerated--;
        return _generation[_upgradeLevel - 1].resourceGenerated;
    }
}
