using System;
using Emesefe.Utilities;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float angle;
    private Vector3 position;
    private Vector3 lookDirection = Vector3.up;
    
    [SerializeField] private GameObject shootingPosition;

    public static event Action OnShoot;

    private void Update()
    {
        HandlePositionAndRotation();
        HandleShooting();
    }

    private void HandlePositionAndRotation()
    {
        lookDirection = Utils.GetMouseWorldPosition().normalized;
        angle = Utils.GetAngleFromVectorFloat(lookDirection, false);
        position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        
        transform.position = position;
        transform.up = lookDirection;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("PUM");
            OnShoot?.Invoke();
        }
    }
}
