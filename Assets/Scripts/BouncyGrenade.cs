using System;
using UnityEngine;

public class BouncyGrenade : MonoBehaviour
{
    private static int ammo = AMMO_MAX;
    private const int AMMO_MAX = 5;
    
    private float speed = 50f;
    private Rigidbody2D rigidbody;
    private Action<Vector3> _OnExplodeAction;

    private float time;
    private float timeToExplode = 2.5f;

    private int scaleState = 0;
    private float scaleSpeed = 10f;
    private float maxScale = 0.8f;
    private float minScale = 0.3f;

    private void Update()
    {
        HandleScale();
        HandleExplosion();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ExplodeOnContact>() != null)
        {
            ExplodeGrenade();   
        }
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

    private void HandleExplosion()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            ExplodeGrenade();
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
    
    public static void Create(Transform bouncyGrenadePrefab, Vector3 spawnPosition, Vector3 targetPosition, Action<Vector3> OnExplodeAction)
    {
        BouncyGrenade bouncyGrenade = Instantiate(bouncyGrenadePrefab, spawnPosition, Quaternion.identity).GetComponent<BouncyGrenade>();
        bouncyGrenade.Setup(targetPosition, OnExplodeAction);

        ammo--;
    }

    private void Setup(Vector3 targetPosition, Action<Vector3> OnExplodeAction)
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = moveDirection * speed;
        
        transform.up = moveDirection;

        _OnExplodeAction = OnExplodeAction;

        time = timeToExplode;
    }

    private void ExplodeGrenade()
    {
        _OnExplodeAction(transform.position);
        Destroy(gameObject);
    }
    
    
}
