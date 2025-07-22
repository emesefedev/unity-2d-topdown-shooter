using Emesefe.Utilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform bouncyGrenadePrefab;
    [SerializeField] private Transform stickyGrenadePrefab;
    [SerializeField] private Transform grenadeExplosionPrefab;

    private float damageRadius = 5f;

    private void OnEnable()
    {
        Weapon.OnShoot += Weapon_OnShoot;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // if (BouncyGrenade.CanReload())
            // {
            //     BouncyGrenade.Reload();
            // }
            
            if (StickyGrenade.CanReload())
            {
                StickyGrenade.Reload();
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            StickyGrenade.ExplodeAllGrenades();
        }
    }

    private void Weapon_OnShoot()
    {
        // if (BouncyGrenade.HasAmmo()) 
        // {
        //     BouncyGrenade.Create(bouncyGrenadePrefab, weapon.GetShootingPosition(), Utils.GetMouseWorldPosition(), OnGrenadeExplode);
        // }

        if (StickyGrenade.HasAmmo())
        {
            StickyGrenade.Create(stickyGrenadePrefab, weapon.GetShootingPosition(), Utils.GetMouseWorldPosition(), OnGrenadeExplode);
        }
        
    }

    private void OnGrenadeExplode(Vector3 position)
    {
        Instantiate(grenadeExplosionPrefab, position, Quaternion.identity);
    }
}
