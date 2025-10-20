using UnityEngine;
using System.Collections.Generic;

public class WorkerFSM : FSMTemplateMachine
{
    public Idle idleState;
    public Walking walkingState;
    public Working workingState;

    [Header("Working Settings")]
    [SerializeField] private float _progress;
    [SerializeField] private float _progressSpeed;
    [SerializeField] private Building _targetBuilding;

    public float Progress { get { return _progress; } set { _progress = value; } }
    public float ProgressSpeed { get { return _progressSpeed; } set { _progressSpeed = value; } }
    public Building TargetBuilding { get { return _targetBuilding; } }

    private Queue<Task> taskQueue;
    private Task currentTask;
    [SerializeField] private int _maxQueuedTasks = 3;
    [SerializeField] private int _workerHealth = 10;
    private Resource currentResource;


    private void Awake()
    {
        idleState = new Idle(this);
        walkingState = new Walking(this);
        workingState = new Working(this);

        taskQueue = new Queue<Task>();
        currentTask = null;
        currentResource = Resource.None;
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

        
        // get task step (task as an array of TaskStep)
        for(int i = 0; i < currentTask.steps.Length; i++){
            DoTaskStep(currentTask.steps[i]);
        }

        StopCurrentTask();

    }

    public void DoTaskStep(TaskStep step){
        
        switch(step){
            case TaskStep.MoveTowards:
                // go to targetBuilding
                break;
            case TaskStep.Fetch:
                // set currentResource to the Generated / UpgradingRequired / GenerationRequired
                break;
            case TaskStep.Work:
                // works for X time on building
                LowerHealth();
                break;
            case TaskStep.Drop:
                // leave the currentResource on town hall
                LowerHealth();
                break;
            case TaskStep.Return:
                // go to town hall
                break;
            default:
                Debug.Log("Couldn't process task step");
                break;
        }

        
    }

    public void StopCurrentTask(){
        currentTask = null;
        
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


    // other methods: GoTo(), Fetch(), Drop(), Work() [check arguments]
        // logica en los estados

    public void MoveTowards(){
        // Move towards town hall or target building
    }

    public void Fetch(){
        // set currentResource to the Generated / UpgradingRequired / GenerationRequired
            // un pick up del building

        // logica en los estados

        // if in town hall, use ResourcesManager.SubstractResource
    }

    public void Drop(){
        // if in town hall, use ResourcesManager.AddResource

        currentResource = Resource.None;
    }

    public void Work(){

        // targetBuilding.GenerateResource();

        // depende del tick

        // if generated, currentResource = targetBuilding.generated

    }


}
