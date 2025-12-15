using UnityEngine;

public class Rotate_Item : MonoBehaviour
{

    public float rotateAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateAmount);
    }
}
