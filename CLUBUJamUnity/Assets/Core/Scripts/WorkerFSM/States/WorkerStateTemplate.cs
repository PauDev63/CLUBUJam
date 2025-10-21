using UnityEngine;

public class WorkerStateTemplate : FSMTemplateState
{
    protected WorkerFSM _workerFSM;
    public WorkerStateTemplate(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }
}
