using System;
using Emesefe.Utilities;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float angle;
    private Vector3 position;
    private Vector3 lookDirection = Vector3.up;
    
    [SerializeField] private Transform shootingPosition;

    public static event Action OnShoot;

    private void Update()
    {
        HandlePositionAndRotation();
        HandleShooting();
    }

    private void HandlePositionAndRotation()
    {
        lookDirection = (Utils.GetMouseWorldPosition() - transform.position).normalized;    
        angle = Utils.GetAngleFromVectorFloat(lookDirection, false);
        position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        
        transform.localPosition = position;
        transform.up = lookDirection;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnShoot?.Invoke();
        }
    }

    public Vector3 GetShootingPosition()
    {
        return shootingPosition.position;
    }
}
