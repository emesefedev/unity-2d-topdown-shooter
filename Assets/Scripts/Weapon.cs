using Emesefe.Utilities;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Vector3 lookDirection = Vector3.up;
    
    void Update()
    {
        lookDirection = Utils.GetMouseWorldPosition().normalized;
        transform.up = lookDirection;
    }
}
