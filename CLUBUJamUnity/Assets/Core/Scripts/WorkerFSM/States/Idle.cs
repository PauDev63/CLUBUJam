using UnityEngine;

public class Idle : WorkerStateTemplate
{
    private float _counter;
    private float _stopTime;

    public Idle(FSMTemplateMachine fsm) : base(fsm)
    {
        _workerFSM = (WorkerFSM)fsm;
    }

    public override void Enter()
    {
        //Debug.Log("Idle: enter");
        _stopTime = Random.Range(_workerFSM.MinIdlingTime, _workerFSM.MaxIdlingTime);
        _workerFSM.AnimatorWorker.Play("IdleFish");
    }

    public override void UpdateLogic()
    {
        if (!_workerFSM.IsOnDestination())
        {
            _workerFSM.ChangeState(_workerFSM.walkingState);
        }
        else 
        {
            //Debug.Log("ON DESTINATION");
            switch (_workerFSM.CurrentTaskStep)
            {
                case TaskStep.Fetch:
                    _workerFSM.ChangeState(_workerFSM.fetchingState);
                    break;
                case TaskStep.Work:
                    _workerFSM.ChangeState(_workerFSM.workingState);    
                            // el problema es que hace primero el cambio de TaskStep y se ejecuta esto antes de que se haga el cambio de _targetPosition en workerFSM?????
                    break;
            }
        }
    }

    public override void UpdatePhysics()
    {
        _counter += Time.deltaTime;

        if(_counter > _stopTime)
        {
            _workerFSM.SetRandomDestination();
        }
    }

    public override void Exit()
    {
        //Debug.Log("Idle: exit");
        _counter = 0;
    }
}
