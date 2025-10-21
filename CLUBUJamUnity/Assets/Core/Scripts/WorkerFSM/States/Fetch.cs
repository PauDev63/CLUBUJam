using System.Resources;
using UnityEngine;

public class Fetch : WorkerStateTemplate
{
    public Fetch(FSMTemplateMachine fsm) : base(fsm)
    {
    }
    public override void Enter()
    {
        // Drop item
        Debug.Log("Fetching item...");
        if (!_workerFSM.HasWorkedDuringThisTask)
        {
            _workerFSM.CurrentResource = _workerFSM.TargetBuilding.GetRequiredResource();
            ResourcesManager.Instance.SubtractResource(_workerFSM.CurrentResource, 1);
        }
        else
            _workerFSM.CurrentResource = _workerFSM.TargetBuilding.GetGeneratedResource();
    }

    public override void UpdateLogic()
    {
        if (!_workerFSM.CurrentResource.Equals(Resource.None))
            _workerFSM.ChangeState(_workerFSM.walkingState);
    }

    public override void Exit()
    {
        _workerFSM.SetNewDestination();
    }
}
