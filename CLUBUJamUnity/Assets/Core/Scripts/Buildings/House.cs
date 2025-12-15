using UnityEngine;

public class House : Building
{
    [SerializeField] private WorkerFSM _workerGenerated;


    public override void DoEffectAfterWork()
    {
        //ToggleUpgradeMode();

        for(int i = 0; i < quantitiesDropped.Length; i++)
        {
            quantitiesDropped[i] = 0;
        }

        // TEMPORAL
        //UIManager.Instance.HideSelectedUI();
        EventHolder.Instance.onUpdateGameUI?.Invoke();

        

        //_workerFSM.StopCurrentTask();

        InvokeWorker();
    }

    private void InvokeWorker()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, 100f))
        {
            Vector3 spawnPos = hit.point;
            spawnPos.y -= 0.5f; // hunde un poco en el suelo
            Instantiate(_workerGenerated, spawnPos, Quaternion.identity, transform.parent);
        }

        //_workerFSM.StopCurrentTask();
        //Destroy(gameObject);
    }


}
