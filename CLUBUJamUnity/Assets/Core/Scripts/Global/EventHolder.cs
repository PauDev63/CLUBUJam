using System;
using UnityEngine;
using UnityEngine.Events;

public class EventHolder : MonoBehaviour
{
    public static EventHolder Instance;

    [NonSerialized] public UnityEvent onTick;
    [NonSerialized] public UnityEvent onInteract;
    [NonSerialized] public UnityEvent onZoomChange;
    [NonSerialized] public UnityEvent onPause;
    [NonSerialized] public UnityEvent onExitUI;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        onTick = new UnityEvent();
        onInteract = new UnityEvent();
        onZoomChange = new UnityEvent();

        onPause = new UnityEvent();
        onExitUI = new UnityEvent();
    }
}
