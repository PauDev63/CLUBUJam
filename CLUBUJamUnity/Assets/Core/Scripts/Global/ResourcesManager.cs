using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;

    private Dictionary<Resource, int> _resourcesDictionary;
    [SerializeField] private Sprite[] _resourceSprites;
    [SerializeField] private Transform _townHall;

    public Transform TownHall { get { return _townHall; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    /*private void Update()
    {
        
    }*/

    private void Initialize()
    {
        _resourcesDictionary = new Dictionary<Resource, int>();

        //TODO: Set initial values
    }

    public void AddResource(Resource resource, int quantity)
    {
        if (_resourcesDictionary.ContainsKey(resource))
        {
            _resourcesDictionary[resource] += quantity;
        }
        else
        {
            _resourcesDictionary.Add(resource, quantity);
            UIManager.Instance.AddResourceCard(resource);
        }

        UIManager.Instance.UpdateResourceCard(resource, _resourcesDictionary[resource]);
        //ShowResources();
        EventHolder.Instance.onUpdateGameUI?.Invoke();

        //Debug.Log("Player has x" + _resourcesDictionary[resource] + " " + resource.ToString());
    }

    public void SubtractResource(Resource resource, int quantity)
    {
        if (_resourcesDictionary.ContainsKey(resource))
        {
            _resourcesDictionary[resource] = Mathf.Max(0, _resourcesDictionary[resource] - quantity);
        }
        else
        {
            _resourcesDictionary.Add(resource, 0);
            UIManager.Instance.AddResourceCard(resource);
        }

        UIManager.Instance.UpdateResourceCard(resource, _resourcesDictionary[resource]);
        //ShowResources();
        EventHolder.Instance.onUpdateGameUI?.Invoke();
    }

    public bool HasEnough(Resource resource, int quantity)
    {
        if (_resourcesDictionary.ContainsKey(resource))
            return _resourcesDictionary[resource] >= quantity;
        return false;
    }

    public int GetResourceQuantity(Resource resource)
    {
        return _resourcesDictionary[resource];
    }

    public Sprite GetResourceSprite(Resource resource)
    {
        if (((int)resource) == 0)
            return null;
        return _resourceSprites[((int)resource) - 1];
    }

    private void ShowResources()
    {
        string str = "";
        foreach (KeyValuePair<Resource, int> keypair in _resourcesDictionary)
        {
            str += keypair.Key.ToString() + " x" + keypair.Value.ToString() + "\n";
        }
        Debug.Log(str);
    }
    

}
