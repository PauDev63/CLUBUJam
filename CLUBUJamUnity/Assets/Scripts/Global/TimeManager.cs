using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [SerializeField] private float _tickTime;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(_tickTime);
        EventHolder.Instance.onTick?.Invoke();
        StartCoroutine(Tick());
    }
}
