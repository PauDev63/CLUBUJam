using System.Resources;
using UnityEngine;

public class Fetch : WorkerStateTemplate
{
    bool goToIdle;

    public Fetch(FSMTemplateMachine fsm) : base(fsm)
    {
    }
    public override void Enter()
    {
        goToIdle = false;
        // Fetch item
        if (!_workerFSM.HasWorkedDuringThisTask)
        {
            Debug.Log("Looking item to fetch...");
            if (ResourcesManager.Instance.HasEnough(_workerFSM.TargetBuilding.GetRequiredResource(), 1))
            {
                _workerFSM.CurrentResource = _workerFSM.TargetBuilding.GetRequiredResource();
                _workerFSM.ShowResource(true);
                ResourcesManager.Instance.SubtractResource(_workerFSM.CurrentResource, 1);
                Debug.Log($"Player fetched this {_workerFSM.CurrentResource}, there are: {ResourcesManager.Instance.GetResourceQuantity(_workerFSM.CurrentResource)}");
            }
            else
            {
                _workerFSM.StopCurrentTask();
                goToIdle = true;
            }
        }
        else
        {
            if (_workerFSM.TargetBuilding.QuantityGenerated > 0)
            {
                _workerFSM.CurrentResource = _workerFSM.TargetBuilding.GetGeneratedResource();
                _workerFSM.ShowResource(true);
            }
            else
            {
                _workerFSM.StopCurrentTask();
                goToIdle = true;
            }
        }
    }

    public override void UpdateLogic()
    {
        if (goToIdle)
            _workerFSM.ChangeState(_workerFSM.idleState);
            
        if (!_workerFSM.CurrentResource.Equals(Resource.None))
            _workerFSM.ChangeState(_workerFSM.walkingState);
    }

    public override void Exit()
    {
        if(!goToIdle)
            _workerFSM.SetNewDestination();
    }
}
