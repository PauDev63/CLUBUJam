using UnityEngine;

public class Drop : WorkerStateTemplate
{
    public Drop(FSMTemplateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        // Drop item
        Debug.Log("Dropping item...");
        if (!_workerFSM.HasWorkedDuringThisTask)
        {
            _workerFSM.TargetBuilding.DropRequiredResource(_workerFSM.CurrentResource);
        }
        else
        {
            ResourcesManager.Instance.AddResource(_workerFSM.CurrentResource, 1);
        }
        
        _workerFSM.ShowResource(false);
        _workerFSM.CurrentResource = Resource.None;
    }

    public override void UpdateLogic()
    {
        if(_workerFSM.CurrentResource.Equals(Resource.None))
            _workerFSM.ChangeState(_workerFSM.walkingState);
    }

    public override void Exit()
    {
        _workerFSM.SetNewDestination();
    }
}
