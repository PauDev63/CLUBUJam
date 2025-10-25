using UnityEngine;

public class Bilboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
