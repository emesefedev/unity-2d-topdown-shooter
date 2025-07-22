using System;
using UnityEngine;
using System.Collections.Generic;

public class StickyGrenade : MonoBehaviour
{
    private static List<StickyGrenade> stickyGrenadesList = new List<StickyGrenade>();
    private static int ammo = AMMO_MAX;
    private const int AMMO_MAX = 8;
    
    private float speedMultiplier = 4f;
    private float minSpeed = 10f;
    private float maxSpeed = 100f;
    private float angularVelocity = -100f;
    
    private Rigidbody2D rigidbody;
    private Action<Vector3> _OnExplodeAction;

    private int scaleState = 0;
    private float scaleSpeed = 10f;
    private float maxScale = 0.8f;
    private float minScale = 0.3f;

    private void Update()
    {
        HandleScale();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StopMoving();
    }

    private void HandleScale()
    {
        switch (scaleState)
        {
            case 0:
                transform.localScale += Vector3.one * (scaleSpeed * Time.deltaTime);
                if (transform.localScale.x > maxScale) scaleState = 1;
                break;
            case 1:
                transform.localScale -= Vector3.one * (scaleSpeed * Time.deltaTime);
                if (transform.localScale.x < minScale) scaleState = 2;
                break;
            case 2:
                break;
        }
    }
    
    public static bool HasAmmo()
    {
        return ammo > 0;
    }

    public static void Reload()
    {
        ammo = AMMO_MAX;
    }

    public static bool CanReload()
    {
        return ammo < AMMO_MAX;
    }
    
    public static void Create(Transform stickyGrenadePrefab, Vector3 spawnPosition, Vector3 targetPosition, Action<Vector3> OnExplodeAction)
    {
        StickyGrenade stickyGrenade = Instantiate(stickyGrenadePrefab, spawnPosition, Quaternion.identity).GetComponent<StickyGrenade>();
        stickyGrenade.Setup(targetPosition, OnExplodeAction);
        
        stickyGrenadesList ??= new List<StickyGrenade>();
        stickyGrenadesList.Add(stickyGrenade);

        if (stickyGrenadesList.Count > AMMO_MAX)
        {
            stickyGrenadesList[0].ExplodeGrenade();
            stickyGrenadesList.RemoveAt(0);
        }
        ammo--;
    }

    public static void ExplodeAllGrenades()
    {
        foreach (StickyGrenade stickyGrenade in stickyGrenadesList)
        {
            stickyGrenade.ExplodeGrenade();
        }   
        stickyGrenadesList.Clear();
    }

    private void Setup(Vector3 targetPosition, Action<Vector3> OnExplodeAction)
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        rigidbody = GetComponent<Rigidbody2D>();
        
        float distance = Vector2.Distance(transform.position, targetPosition);
        float speed = Mathf.Clamp(distance * speedMultiplier, minSpeed, maxSpeed);
        rigidbody.velocity = moveDirection * speed;
        rigidbody.angularVelocity = angularVelocity;
        
        transform.up = moveDirection;

        _OnExplodeAction = OnExplodeAction;
    }

    private void ExplodeGrenade()
    {
        _OnExplodeAction(transform.position);
        Destroy(gameObject);
    }

    private void StopMoving()
    {
        Destroy(rigidbody);
        Destroy(gameObject.GetComponent<CircleCollider2D>());
    }
    
    
}
