public class Idle : FSMTemplateState
{
    WorkerFSM _workerFSM;

    public Idle(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }

    public override void UpdateLogic()
    {
        _workerFSM.ChangeState(_workerFSM.workingState);
    }
}
