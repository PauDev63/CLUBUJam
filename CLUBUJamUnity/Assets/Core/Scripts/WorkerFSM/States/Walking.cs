using UnityEngine;

public class Walking : WorkerStateTemplate
{
    public Walking(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }

    public override void Enter()
    {
        //Debug.Log("Walking: enter");
        _workerFSM.NavMeshAgent.SetDestination(_workerFSM.TargetDestination);
        if (_workerFSM.NavMeshAgent.isStopped)
            _workerFSM.NavMeshAgent.isStopped = false;
    }

    public override void UpdateLogic()
    {
        if (_workerFSM.IsOnDestination())
        {
            switch (_workerFSM.CurrentTaskStep)
            {
                case TaskStep.None:
                    _workerFSM.ChangeState(_workerFSM.idleState);
                    break;
                case TaskStep.Fetch:
                    _workerFSM.ChangeState(_workerFSM.fetchingState);
                    break;
                case TaskStep.Drop:
                    _workerFSM.ChangeState(_workerFSM.droppingState);
                    break;
                case TaskStep.Work:
                    _workerFSM.ChangeState(_workerFSM.workingState);
                    break;
                case TaskStep.Return:
                    _workerFSM.SetNewDestination();  
                    break;
                default:
                    throw new System.Exception("Unable to filter TaskStep " + _workerFSM.CurrentTaskStep);
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log("Walking: exit");
        _workerFSM.NavMeshAgent.isStopped = true;
    }
}
