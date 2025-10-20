using UnityEngine;

public class Working : FSMTemplateState
{
    WorkerFSM _workerFSM;

    public Working(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }

    public override void Enter()
    {
        EventHolder.Instance.onTick.AddListener(Work);
    }

    public override void UpdateLogic()
    {
        if(_workerFSM.Progress >= 100f)
            _workerFSM.ChangeState(_workerFSM.idleState);
    }

    public override void Exit()
    {
        EventHolder.Instance.onTick.RemoveListener(Work);
        _workerFSM.Progress = 0;
        _workerFSM.TargetBuilding.GenerateResource();
    }

    private void Work() 
    {
        _workerFSM.Progress += Time.deltaTime * _workerFSM.ProgressSpeed;
    }
}
