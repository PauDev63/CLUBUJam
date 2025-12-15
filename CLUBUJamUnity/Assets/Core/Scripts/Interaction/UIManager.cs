using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public IInteractable ObjectSelected { get { return objectSelected; } set { objectSelected = value; } }

    [Header("Resource Card")]
    [SerializeField] private GameObject _prefabResourceCard;
    [SerializeField] private List<Transform> _resourcePanel;
    private Dictionary<Resource, UIResourceCard> resourceCards = new();


    [Header("Worker UI")]
    [SerializeField] private GameObject _workerSelectedFlag;
    [SerializeField] private Image _workerResourceSprite;
    [SerializeField] private List<UITaskWorkerCard> _taskCardList;

    
    [Header("Building UI")]
    [SerializeField] private Image _buildingSprite;
    [SerializeField] private UIResourceCard generationRequired;
    [SerializeField] private UIResourceCard generationObtained;
    [SerializeField] private Button _upgradeBuildingButton;
    [SerializeField] private Transform _upgradeRequirementsPanel;


    [Header("Plot UI")]
    [SerializeField] private Image _buildingToken;
    [SerializeField] private UIResourceCard _tokenGeneration;
    [SerializeField] private Button _buildPlotButton;
    [SerializeField] private Transform _buildRequirementsPanel;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Mostrar botón para mostrar menú, pero no menú aún
        _isOpen = true;
        HideObjectUI(WorkerUI);
        HideObjectUI(PlotUI);
        HideObjectUI(BuildingUI);

        /*EventHolder.Instance.onSelectedObject.AddListener(OnSelectedObject);
        EventHolder.Instance.onUpgradedBuilding.AddListener(OnUpgradedBuilding);
        EventHolder.Instance.onWorkerResourceChanged.AddListener(OnWorkerResourceChanged);
        EventHolder.Instance.onTaskChanged.AddListener(OnTaskChanged);
        EventHolder.Instance.onEnergyChanged.AddListener(OnEnergyChanged);*/
        EventHolder.Instance.onUpdateGameUI.AddListener(UpdateGameUI);

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

        if (objectSelected is WorkerFSM worker)
        {
            //Sprite spriteToShow = worker.GetResourceSprite();
            Sprite spriteToShow = ResourcesManager.Instance.GetResourceSprite(worker.CurrentResource);
            _workerResourceSprite.sprite = spriteToShow;

            _workerResourceSprite.enabled = (spriteToShow != null);

            // TAREAS
            // mirar la cola y poner la info de cada una en cada botón sobre los placeholders
            // de momento solo botón Drop en la current

            // Primero la tarea Current worker.CurrentTask
            // lista de cards e ir una por una, en esta inicializar con el botón a True
            if (worker.CurrentTask != null)
            {
                int i = 0;
                _taskCardList[0].Initialize(worker.TargetBuilding.GetBuildingSprite(), true);
                foreach (Task item in worker.TaskQueue)  //Orden de Queue es FIFO, orden de los card: (current), 2,3,4
                {
                    i++;
                    _taskCardList[i].Initialize(item.targetBuilding.GetBuildingSprite(), false);
                }
                while (i < 3)
                {
                    i++;
                    _taskCardList[i].HideTaskCard();
                }
            }
            else
            {
                foreach (UITaskWorkerCard card in _taskCardList)
                {
                    card.HideTaskCard();
                }
            }
        }
        if (objectSelected is Plot plot)
        {
            //token del building _buildingToken
            _buildingToken.sprite = plot.GetFutureBuilding();


            //ResourceCard _tokenGeneration
            _tokenGeneration.UpdateSprite(ResourcesManager.Instance.GetResourceSprite(plot.GetFutureResource()));
            _tokenGeneration.UpdateQuantity(plot.GetFutureQuantitiesGenerated());

            //Build requirements
            for (int i = _buildRequirementsPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(_buildRequirementsPanel.GetChild(i).gameObject);
            }

            int cardsCounter = plot.GetQuantitiesResources() - 1;
            while (cardsCounter >= 0)
            {
                GameObject newCard = Instantiate(_prefabResourceCard, _buildRequirementsPanel);
                Resource resource = plot.GetResourceNeededById(cardsCounter);

                var cardUI = newCard.GetComponent<UIResourceCard>();
                cardUI.Initialize(ResourcesManager.Instance.GetResourceSprite(resource), plot.GetResourceAmountNeededById(cardsCounter));
                
                cardsCounter--;
            }

        }
        if (objectSelected is Building building)
        {

            _buildingSprite.sprite = building.GetBuildingSprite();

            //update both cards of generation
            if(building.QuantityNeeded == 0)
            {
                generationRequired.EmptyCard();
            }
            else
            {
                generationRequired.UpdateSprite(ResourcesManager.Instance.GetResourceSprite(building.GetResourceEnumRequired()));
                generationRequired.UpdateQuantity(building.QuantityNeeded);
            }

            generationObtained.UpdateSprite(ResourcesManager.Instance.GetResourceSprite(building.GetResourceEnumGenerated()));
            generationObtained.UpdateQuantity(building.GetAmountGenereted());


            //Upgrade requirements
            for (int i = _upgradeRequirementsPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(_upgradeRequirementsPanel.GetChild(i).gameObject);
            }

            int cardsCounter = building.GetQuantitiesResources() - 1;
            while (cardsCounter >= 0)
            {
                GameObject newCard = Instantiate(_prefabResourceCard, _upgradeRequirementsPanel);
                Resource resource = building.GetResourceNeededById(cardsCounter);

                var cardUI = newCard.GetComponent<UIResourceCard>();
                cardUI.Initialize(ResourcesManager.Instance.GetResourceSprite(resource), building.GetResourceAmountNeededById(cardsCounter));
                
                cardsCounter--;
            }
        }
    }

    public void SelectWorker()  // Not used
    {
        if (objectSelected is WorkerFSM worker)
        {
            CameraController.Instance.ActiveWorker = worker;
        }
    }

    public void DropTask(int taskId)
    {
        // método StopCurrentTask() del WorkerFSM
        // QUÉ PASA SI SE CANCELA TENIENDO UN OBJETO Y ANTES DE DEJARLO EN EL AYTO SE LE ASIGNA OTRA TAREA????
        // Solo se cancela la tarea concreta

        if (objectSelected is WorkerFSM worker)
        {
            worker.StopCurrentTask();   // no sirve la current, usar el taskId
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

    public void AddResourceCard(Resource resource)
    {
        //Instantiate ResourceCard with quantity 0
        GameObject newCard;
        if (resourceCards.Count < 3)
        {
            newCard = Instantiate(_prefabResourceCard, _resourcePanel[0]);
        } else if (resourceCards.Count < 6)
        {
            newCard = Instantiate(_prefabResourceCard, _resourcePanel[1]);
        }
        else
        {
            newCard = Instantiate(_prefabResourceCard, _resourcePanel[2]);
        }
        //GameObject newCard = Instantiate(_prefabResourceCard, _resourcePanel);    //resourcePanel es el Horizontal
        var cardUI = newCard.GetComponent<UIResourceCard>();
        cardUI.Initialize(ResourcesManager.Instance.GetResourceSprite(resource), 0);
        resourceCards.Add(resource, cardUI);
    }

    public void UpdateResourceCard(Resource resource, int quantity)
    {
        //Update the text
        resourceCards[resource].UpdateQuantity(quantity);
    }

    public void UpdateGameUI()
    {
        ChangeUI(objectSelected);
    }

    /*public void OnSelectedObject()
    {

    }


    public void OnUpgradedBuilding()
    {

    }

    public void OnWorkerResourceChanged()
    {

    }
    
    public void OnTaskChanged()
    {

    }
    
    public void OnEnergyChanged()
    {

    }*/

}
