using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour //FSMTemplateMachine
{
    public static CameraController Instance;

    private Camera _cam;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _minZoomHeight;
    [SerializeField] private float _maxZoomHeight;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _panSpeed;

    private WorkerFSM activeWorker;
    public WorkerFSM ActiveWorker { get { return activeWorker; } set { activeWorker = value; } }
    private Building activeBuilding;
    public WorkerFSM ActiveBuilding { get { return activeBuilding; } set { activeBuilding = value; } }


    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _cam = GetComponent<Camera>();
        activeWorker = null;

        EventHolder.Instance.onInteract.AddListener(TryInteract);
        EventHolder.Instance.onZoomChange.AddListener(Zoom);
    }

    private void OnEnable()
    {
        if(EventHolder.Instance != null)
            EventHolder.Instance.onInteract.AddListener(TryInteract);
    }

    private void OnDisable()
    {
        if (EventHolder.Instance != null)
            EventHolder.Instance.onInteract.RemoveListener(TryInteract);
    }

    private void LateUpdate()
    {
        if (InteractionManager.Instance != null)
        {
            if (InteractionManager.Instance.IsHoldEnabled)
            {
                Pan();
            }
        }
    }

    /*private void Awake()
    {
        nonSelectedState = new nonSelected(this);
        _cam = GetComponent<Camera>();

        _activeWorker = null;

    }

    protected override void GetInitialState(out FSMTemplateState stateMachine)
    {
        stateMachine = nonSelectedState;
    }*/


    void TryInteract()
    {
        Ray ray = _cam.ScreenPointToRay(InteractionManager.Instance.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
                hit.collider.gameObject.GetComponent<IInteractable>().Interact();
        }
    }

    private void Zoom()
    {
        if(transform.position.y <= _minZoomHeight && InteractionManager.Instance.ZoomLevel > 0 || transform.position.y >= _maxZoomHeight && InteractionManager.Instance.ZoomLevel < 0)
            return;

        transform.Translate(transform.forward * InteractionManager.Instance.ZoomLevel * _zoomSpeed * Time.deltaTime, Space.World);

        //Clamp "zoom" between two values
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, _minZoomHeight, _maxZoomHeight);
        transform.position = position;
    }

    private void Pan()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        transform.Translate(forward * InteractionManager.Instance.MouseDelta.y * _panSpeed * Time.deltaTime, Space.World);
        transform.Translate(right * InteractionManager.Instance.MouseDelta.x * _panSpeed * Time.deltaTime, Space.World);
    }
}
