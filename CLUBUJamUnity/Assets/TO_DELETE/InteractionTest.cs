using UnityEngine;

public class InteractionTest : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}
