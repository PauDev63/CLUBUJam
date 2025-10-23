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

    private bool _isOpen;
    public bool IsOpen { get { return _isOpen; } }

    private IInteractable objectSelected;

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
        _isOpen = false;
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

    public void InteractableSelected(IInteractable selected)
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

        objectSelected = selected;

        switch (selected)
        {
            case WorkerFSM worker:
                ShowObjectUI(WorkerUI);
                // solo activeworker si se le da a assign task
                break;
            case Plot plot:
                ShowObjectUI(PlotUI);
                BuildingUI.transform.Find("BuildPlotButton").GetComponent<Button>().interactable = (CameraController.Instance.ActiveWorker != null);
                break;
            case Building building:
                ShowObjectUI(BuildingUI);
                BuildingUI.transform.Find("AssignTaskButton").GetComponent<Button>().interactable = (CameraController.Instance.ActiveWorker != null);
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

    public void SelectWorker()
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

        if (objectSelected is WorkerFSM worker)
        {
            CameraController.Instance.ActiveWorker.StopCurrentTask();
        }
    }

    public void AssignTask()
    {
        // comprobar si hay worker seleccionado
        //CameraController.Instance.ActiveWorker.QueueTask(_task);      //cómo lo hacemos con la task? hacer método en Building?
        if (objectSelected is Building building)
        {
            CameraController.Instance.ActiveWorker.QueueTask(building.BuildingTask);
            BuildingUI.transform.Find("AssignTaskButton").GetComponent<Button>().interactable = false;
            CameraController.Instance.ActiveWorker = null;
        }
    }

    public void UpgradeBuilding()
    {
        // comprobar si hay worker seleccionado
        // lo que toque de eso
    }
    
    public void BuildPlot()
    {
        // comprobar si hay worker seleccionado
        // con el tipo seleccionado, se construye

        if (objectSelected is Plot plot)
        {
            //plot.ConstructBuilding();   // pasar el tipo de edificio seleccionado???

            // NO es ConstructBuilding() porque eso es para cuando ha terminado, será asignar su tarea al worker
            CameraController.Instance.ActiveWorker.QueueTask(plot.BuildingTask);
            BuildingUI.transform.Find("BuildPlotButton").GetComponent<Button>().interactable = false;
            CameraController.Instance.ActiveWorker = null;
        }
    }

}
