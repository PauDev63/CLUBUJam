using UnityEngine;

public class Working : WorkerStateTemplate
{
    bool hasEndedWorking;

    public Working(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }

    public override void Enter()
    {
        Debug.Log("Working: enter");
        hasEndedWorking = false;
        EventHolder.Instance.onTick.AddListener(Work);
        _workerFSM.AnimatorWorker.Play("FishWork");
    }

    public override void UpdateLogic()
    {
        if (hasEndedWorking)
            _workerFSM.ChangeState(_workerFSM.walkingState);
    }

    public override void UpdatePhysics()
    {
        if (_workerFSM.Progress >= 100f)
        {
            _workerFSM.SetNewDestination();
            hasEndedWorking = true;
        }
    }

    public override void Exit()
    {
        Debug.Log("Working: exit");
        EventHolder.Instance.onTick.RemoveListener(Work);
        _workerFSM.Progress = 0;
        _workerFSM.TargetBuilding.DoEffectAfterWork();
    }

    private void Work() 
    {
        _workerFSM.Progress += Time.deltaTime * _workerFSM.ProgressSpeed;
    }
}
