using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private GameObject GameplayMenuUI;
    //[SerializeField] private CameraController;    //usar Instance
    [SerializeField] private GameObject WorkerUI;
    [SerializeField] private GameObject BuildingUI;
    [SerializeField] private GameObject PlotUI;
    [SerializeField] private GameObject ShowUIButton;
    //[SerializeField] private Button _assignTaskWorkerButton;
    [SerializeField] private Button _upgradeBuildingButton;
    [SerializeField] private Button _buildPlotButton;

    private bool _isOpen;
    public bool IsOpen { get { return _isOpen; } }

    private IInteractable objectSelected;
    public IInteractable ObjectSelected { get { return objectSelected; } set { objectSelected = value; } }

    [SerializeField] private GameObject _workerSelectedFlag;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Mostrar botón para mostrar menú, pero no menú aún
        _isOpen = true;
    }

    public void ToggleUI()
    {
        if (_isOpen)
        {
            CloseUI();
        }
        else
        {
            ShowUI();
        }
    }

    public void CloseUI()
    {
        // Hide UI
        GameplayMenuUI.SetActive(false);
        _isOpen = false;

        HideSelectedUI();

        objectSelected = null;

        // Show button
        ShowUIButton.SetActive(true);
    }

    public void ShowUI()
    {
        // Show UI
        GameplayMenuUI.SetActive(true);
        _isOpen = true;

        // Hide button
        ShowUIButton.SetActive(false);
    }

    public void HideSelectedUI()
    {
        switch (objectSelected)
        {
            case WorkerFSM worker:
                HideObjectUI(WorkerUI);
                break;
            case Plot plot:
                HideObjectUI(PlotUI);
                break;
            case Building building:
                HideObjectUI(BuildingUI);
                break;
        }
    }

    //public void InteractableSelected(IInteractable selected)
    public void ChangeUI(IInteractable selected)
    {

        HideSelectedUI();
        objectSelected = selected;

        switch (selected)
        {
            case WorkerFSM worker:
                ShowObjectUI(WorkerUI);
                break;
            case Plot plot:
                _buildPlotButton.interactable = !plot.UpgradeMode && plot.HasBuildingResources();
                ShowObjectUI(PlotUI);
                break;
            case Building building:
                _upgradeBuildingButton.interactable = !building.UpgradeMode && building.CanUpgrade();
                ShowObjectUI(BuildingUI);
                break;
        }

    }

    public void HideObjectUI(GameObject UIToHide)
    {
        UIToHide.SetActive(false);
    }
    
    public void ShowObjectUI(GameObject UIToShow)
    {
        UIToShow.SetActive(true);
        //Show the information of objectSelected
    }

    public void SelectWorker()  // Not used
    {
        if (objectSelected is WorkerFSM worker)
        {
            CameraController.Instance.ActiveWorker = worker;
        }
    }

    public void DropTask()
    {
        // método StopCurrentTask() del WorkerFSM
            // QUÉ PASA SI SE CANCELA TENIENDO UN OBJETO Y ANTES DE DEJARLO EN EL AYTO SE LE ASIGNA OTRA TAREA????
        // Solo se cancela la tarea concreta

        if (objectSelected is WorkerFSM worker)
        {
            CameraController.Instance.ActiveWorker.StopCurrentTask();
        }
    }

    public void AssignTaskWorker()
    {
        if (objectSelected is WorkerFSM worker)
        {
            CameraController.Instance.ActiveWorker = worker;

            //hacer que la UI del worker sea fija
            _workerSelectedFlag.SetActive(true);
        }
    }

    public void AssignTaskBuilding()    // Not used
    {
        // comprobar si hay worker seleccionado
        //CameraController.Instance.ActiveWorker.QueueTask(_task);      //cómo lo hacemos con la task? hacer método en Building?
        if (objectSelected is Building building)
        {
            CameraController.Instance.ActiveWorker.QueueTask(building.BuildingTask);
            //BuildingUI.transform.Find("AssignTaskButton").GetComponent<Button>().interactable = false;
            CameraController.Instance.ActiveWorker = null;
        }
    }

    public void UpgradeBuilding()
    {
        // comprobar si hay worker seleccionado
        // quitar interactable del boton si no hay worker

        if (objectSelected is Building building)
        {
            //CameraController.Instance.ActiveWorker.QueueTask(building.BuildingTask);
            //building.TryUpgrade();
            Debug.Log("Upgrade building");
            CameraController.Instance.ActiveWorker = null;

            // setea una flag y la siguiente assign task del propio edificio asignará la tarea de Upgrade
            building.ToggleUpgradeMode();
            _upgradeBuildingButton.interactable = false;
        }

    }

    public void BuildPlot()
    {
        // comprobar si hay worker seleccionado
        // con el tipo seleccionado, se construye

        if (objectSelected is Plot plot)
        {
            //CameraController.Instance.ActiveWorker.QueueTask(plot.BuildingTask);
            _buildPlotButton.interactable = false;
            CameraController.Instance.ActiveWorker = null;

            // setea una flag y la siguiente assign task del propio edificio asignará la tarea de Upgrade
            plot.ToggleUpgradeMode();
        }
    }
    
    public void DeselectWorker()
    {
        _workerSelectedFlag.SetActive(false);
    }

}
