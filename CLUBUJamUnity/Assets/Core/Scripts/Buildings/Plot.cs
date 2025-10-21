using UnityEngine;

public class Plot : Building
{
    [SerializeField] private GameObject _building;

    public override void DoEffectAfterWork()
    {
        ConstructBuilding();
    }
    
    private void ConstructBuilding()
    {
        Instantiate(_building, transform.position, Quaternion.identity, transform.parent);
        _workerFSM.StopCurrentTask();
        Destroy(gameObject);
    }

}
