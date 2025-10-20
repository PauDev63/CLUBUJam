using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;

    private Dictionary<Resource, int> _resourcesDictionary;

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

    private void Initialize()
    {
        _resourcesDictionary = new Dictionary<Resource, int>();

        //TODO: Set initial values
    }

    public void AddResource(Resource resource, int quantity)
    {
        if(_resourcesDictionary.ContainsKey(resource))
            _resourcesDictionary[resource] += quantity;
        else
            _resourcesDictionary.Add(resource, quantity);

        Debug.Log("Player has x" + _resourcesDictionary[resource] + " " + resource.ToString());
    }

    public void SubtractResource(Resource resource, int quantity)
    {
        if (_resourcesDictionary.ContainsKey(resource))
            _resourcesDictionary[resource] = Mathf.Max(0, _resourcesDictionary[resource] - quantity);
        else
            _resourcesDictionary.Add(resource, 0);
    }

    public bool HasEnough(Resource resource, int quantity)
    {
        if(_resourcesDictionary.ContainsKey(resource))
            return _resourcesDictionary[resource] >= quantity;
        return false;
    }

}
