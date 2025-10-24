using System;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [Tooltip("Add each level on the array to cycle on upgrade")]
    [SerializeField] protected Generation[] _generation;
    [SerializeField] private int _upgradeLevel;
    [SerializeField] protected Task _task;
    [SerializeField] protected WorkerFSM _workerFSM;
    protected int[] quantitiesDropped;
    [SerializeField] private int quantityGenerated;
    protected int quantityNeeded;    // total quantity required for generation

    private bool upgradeMode;
    [SerializeField] private GameObject _upgradingFlag;


    public int QuantityGenerated { get { return quantityGenerated; } }
    public int QuantityNeeded { get { return quantityNeeded; } }
    public int QuantityDropped
    {
        get
        {
            int i = 0;

            foreach (int quantity in quantitiesDropped)
            {
                i += quantity;
            }

            return i;
        }
    }

    public Task BuildingTask { get { return _task; } }
    public bool UpgradeMode { get { return upgradeMode; } set { upgradeMode = value; } }



    private void Start()
    {
        _task = new Task(this);
        upgradeMode = false;

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

        /*if (UIManager.Instance.IsOpen)
        {
            // mostrar building en UI (y asignar ahí tarea o que se asigne directamente?)
            UIManager.Instance.InteractableSelected(this);
            //sirve para Plot también
        }
        else
        {

        }*/
        if (UIManager.Instance.IsOpen)
        {
            UIManager.Instance.InteractableSelected(this);
        }
        
        if (CameraController.Instance.ActiveWorker != null)
        {
            CameraController.Instance.ActiveWorker.QueueTask(_task);
            //Debug.Log("Task assigned");
        }

        Debug.Log("Building interacted");
        CameraController.Instance.ActiveWorker = null;
    }

    private void GenerateResource()
    {
        for (int i = 0; i < _generation[_upgradeLevel - 1].resourcesRequiredForGeneration.Length; i++)
        {
            quantitiesDropped[i] -= _generation[_upgradeLevel - 1].quantitiesRequiredForGeneration[i];
        }
        quantityGenerated += _generation[_upgradeLevel - 1].quantityGenerated;
        //ResourcesManager.Instance.AddResource(_generation[_upgradeLevel - 1].resourceGenerated, _generation[_upgradeLevel - 1].quantityGenerated);
    }

    public virtual void DoEffectAfterWork(){
        GenerateResource();
    }

    public void TryUpgrade()
    {
        if (CanUpgrade())
        {
            Upgrade();
        }
    }

    public bool CanUpgrade()
    {
        if(_upgradeLevel < _generation.Length)
            return HasUpgradingResources();

        return false;
    }

    public bool HasUpgradingResources()
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


        // Just change flag, that will change the main loops
        // quitar quantities dropped al poner el flag
        // el quantity dropped debe ser 0 y el Quantity needed que sea el required for upgrade, no el for generation
        // también cambiar el fetch con el flag y el drop
        // y el DoAfterWork() del Working
    }
    
    public void ToggleUpgradeMode()
    {
        /*if (upgradeMode)
        {
            upgradeMode = false;
            _upgradingFlag.SetActive(false);
        }
        else
        {
            // se enciende
            upgradeMode = true;
            _upgradingFlag.SetActive(true);
        }*/

        upgradeMode = !upgradeMode;
        _upgradingFlag.SetActive(upgradeMode);
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
