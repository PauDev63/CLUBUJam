using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    private InputActions _inputActions;

    private InputAction _mousePositionInputAction;
    private InputAction _mouseDeltaInputAction;

    private bool _isHoldEnabled;
    private float _zoomLevel;
    private Vector2 _mousePosition;
    private Vector2 _mouseDelta;

    public bool IsHoldEnabled { get { return _isHoldEnabled; } }
    public float ZoomLevel { get { return _zoomLevel; } }
    public Vector2 MousePosition { get { return _mousePosition; } }
    public Vector2 MouseDelta { get { return _mouseDelta; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputActions = new InputActions();

        _inputActions.Gameplay.Interact.started += StartHolding;
        _inputActions.Gameplay.Interact.performed += Interact;
        _inputActions.Gameplay.Interact.canceled += EndHolding;

        _inputActions.Gameplay.Zoom.performed += Zoom;

        _mousePositionInputAction = _inputActions.Gameplay.CursorPosition;
        _mouseDeltaInputAction = _inputActions.Gameplay.Drag;

        _inputActions.Gameplay.Enable();
    }

    private void Update()
    {
        _mousePosition = _mousePositionInputAction.ReadValue<Vector2>();
        _mouseDelta = _mouseDeltaInputAction.ReadValue<Vector2>();
    }

    private void StartHolding(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            _isHoldEnabled = true;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if(context.interaction is TapInteraction)
        {
            EventHolder.Instance.onInteract?.Invoke();
        }
    }

    private void EndHolding(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            _isHoldEnabled = false;
        }
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        _zoomLevel = context.ReadValue<Vector2>().y;
        EventHolder.Instance.onZoomChange?.Invoke();
    }
}
