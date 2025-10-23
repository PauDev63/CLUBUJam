using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class WorkerFSM : FSMTemplateMachine, IInteractable
{
    public Idle idleState;
    public Walking walkingState;
    public Working workingState;
    public Fetch fetchingState;
    public Drop droppingState;

    [Header("Working Settings")]
    [SerializeField] private float _progress;
    [SerializeField] private float _progressSpeed;
    [SerializeField] private float _targetDistanceToBeOnDestination;
    [SerializeField] private Building _targetBuilding;
    [SerializeField] private Transform _townHall;

    [Header("Idling Settings")]
    [SerializeField] private float _minIdlingTime;
    [SerializeField] private float _maxIdlingTime;

    [SerializeField] private Vector3 _targetDestination;

    public float Progress { get { return _progress; } set { _progress = value; } }
    public float ProgressSpeed { get { return _progressSpeed; } set { _progressSpeed = value; } }
    public Building TargetBuilding { get { return _targetBuilding; } set { _targetBuilding = value; } }
    public Vector3 TargetDestination { get { return _targetDestination; } }

    public float MinIdlingTime { get { return _minIdlingTime; } }
    public float MaxIdlingTime { get { return _maxIdlingTime; } }

    public bool HasWork { get { return taskQueue.Count > 0; } }

    private Queue<Task> taskQueue;
    private Task currentTask;
    private TaskStep currentTaskStep;
    [SerializeField] private int _maxQueuedTasks = 3;
    [SerializeField] private int _workerHealth = 10;
    [SerializeField] private Resource currentResource;

    public bool doNextTaskStep;
    private bool hasWorkedDuringThisTask;
    public bool HasWorkedDuringThisTask { get { return hasWorkedDuringThisTask; } }

    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    public TaskStep CurrentTaskStep { get { return currentTaskStep; } }
    public Resource CurrentResource { get { return currentResource; } set { currentResource = value; } }

    private Coroutine executingTaskCoroutine;



    private void Awake()
    {
        idleState = new Idle(this);
        walkingState = new Walking(this);
        workingState = new Working(this);
        fetchingState = new Fetch(this);
        droppingState = new Drop(this);

        taskQueue = new Queue<Task>();
        currentTask = null;
        currentResource = Resource.None;

        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    protected override void GetInitialState(out FSMTemplateState stateMachine)
    {
        stateMachine = idleState;
    }

    public void LowerHealth(){
        _workerHealth--;
        if(_workerHealth <= 0){
            Debug.Log("Se muere");
        }
    }

    public bool CanQueue(){
        return taskQueue.Count < _maxQueuedTasks;
    }

    public void QueueTask(Task newTask){

        if(CanQueue()){
            taskQueue.Enqueue(newTask);

            if (currentTask == null)
                DoTask();
        }        
    }

    public void UnqueueTask(){
        if(taskQueue.Count > 0){
            currentTask = taskQueue.Dequeue();
        }
    }

    public void DoTask(){

        // get task
        if(currentTask == null){
            UnqueueTask();
        }

        executingTaskCoroutine = StartCoroutine(ExecuteTaskStepByStep());
    }

    IEnumerator ExecuteTaskStepByStep()
    {
        hasWorkedDuringThisTask = false;
        _targetBuilding = currentTask.targetBuilding;

        //get task step (task as an array of TaskStep)
        if(_targetBuilding.QuantityNeeded > 0)
        {
            if(_targetBuilding.QuantityGenerated == 0)
            {
                for (int i = 0; i < currentTask.resourcesNeededSteps.Length; i++)
                {
                    currentTaskStep = currentTask.resourcesNeededSteps[i];
                    DoTaskStep();
                    Debug.Log($"Preworking Current task step: {currentTaskStep}");
                    yield return new WaitUntil(() => doNextTaskStep); // WaitUntilTheTaskStepIsDone
                    doNextTaskStep = false;

                    if (_targetBuilding.QuantityDropped != _targetBuilding.QuantityNeeded && i == currentTask.resourcesNeededSteps.Length-1)
                    {
                        i = -1;
                    }
                }
            }
        }

        if (_targetBuilding.QuantityGenerated == 0)
        {
            for (int i = 0; i < currentTask.workingSteps.Length; i++)
            {
                currentTaskStep = currentTask.workingSteps[i];
                DoTaskStep();
                Debug.Log($"Working Current task step: {currentTaskStep}");
                yield return new WaitUntil(() => doNextTaskStep); // WaitUntilTheTaskStepIsDone
                doNextTaskStep = false;
            }
        }

        hasWorkedDuringThisTask = true;

        //get task step (task as an array of TaskStep)
        for (int i = 0; i < currentTask.resourcesGeneratedSteps.Length; i++)
        {
            currentTaskStep = currentTask.resourcesGeneratedSteps[i];
            DoTaskStep();
            Debug.Log($"After Working Current task step: {currentTaskStep}");
            yield return new WaitUntil(() => doNextTaskStep); // WaitUntilTheTaskStepIsDone
            doNextTaskStep = false;

            if (_targetBuilding.QuantityGenerated > 0 && i == currentTask.resourcesGeneratedSteps.Length-1)
            {
                i = -1;
            }
        }

        StopCurrentTask();
    }

    public void DoTaskStep(){
        
        switch(currentTaskStep){
            case TaskStep.Fetch:
                if(hasWorkedDuringThisTask)
                    _targetDestination = _targetBuilding.transform.position;
                else
                    _targetDestination = _townHall.position;
                break;
            case TaskStep.Work:
                _targetDestination = _targetBuilding.transform.position;
                LowerHealth();
                break;
            case TaskStep.Drop:
                if (hasWorkedDuringThisTask)
                    _targetDestination = _townHall.position;
                else
                    _targetDestination = _targetBuilding.transform.position;
                LowerHealth();
                break;
            case TaskStep.Return:
                _targetDestination = _townHall.position;
                break;
            default:
                Debug.Log("Couldn't process task step");
                break;
        }        
        navMeshAgent.SetDestination( _targetDestination );  
    }

    public void StopCurrentTask(){
        currentTask = null;
        currentTaskStep = TaskStep.None;
        doNextTaskStep = false;

        if (executingTaskCoroutine != null)
        {
            StopCoroutine(executingTaskCoroutine);
        }
        
        if(currentResource != Resource.None){
            // leaves resource it at the town hall
                // Move Towards town hall
                // Drop (without losing life)
        }

        if(taskQueue.Count > 0){
            DoTask();
        }else{
            // Idle
        }
    }

    public bool IsOnDestination()
    {
        if(_targetDestination == null)
            return false;

        if (Vector3.Distance(transform.position, _targetDestination) <= _targetDistanceToBeOnDestination)
            return true;

        return false;
    }


    public void SetRandomDestination()
    {
        //Change this to random destination
        _targetDestination = Vector3.right * -5f;
    }

    public void SetNewDestination()
    {
        doNextTaskStep = true;
    }

    public void Interact()
    {
        //_workerFSM.QueueTask(_task);
        Debug.Log("Worker selected");
        //CameraController.Instance.ActiveWorker = this;

        if (UIManager.Instance.IsOpen)
        {
            // mostrar worker en UI y ahí hacer el select
            UIManager.Instance.InteractableSelected(this);
        }
        else
        {
            CameraController.Instance.ActiveWorker = this;    
        }
        
    }

}
