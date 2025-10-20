using UnityEngine;

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


    private void Awake()
    {
        idleState = new Idle(this);
        walkingState = new Walking(this);
        workingState = new Working(this);
    }

    protected override void GetInitialState(out FSMTemplateState stateMachine)
    {
        stateMachine = idleState;
    }
}
