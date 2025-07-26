using System;
using System.Collections.Generic;
using Emesefe.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private PlayerCharacter player;
    
    [SerializeField] private Transform bouncyGrenadePrefab;
    [SerializeField] private Transform stickyGrenadePrefab;
    [SerializeField] private Transform grenadeExplosionPrefab;
    [SerializeField] private Transform enemyPrefab;

    private List<Enemy> enemiesList = new List<Enemy>();
    private float grenadeDamageRadius = 5f;
    private float meleeDamageRadius = 2f;
    
    [SerializeField] private Vector3[] enemySpawnPositionsArray;

    enum WeaponType
    {
        Melee,
        StickyGrenade,
        BouncyGrenade
    }

    private WeaponType weaponType = WeaponType.BouncyGrenade;
    
    private void Start()
    {
        FunctionPeriodic.Create(SpawnEnemy, 1);
    }

    private void Update()
    {
        HandleWeaponType();
    }

    private void HandleWeaponType()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) weaponType = WeaponType.Melee;
        if (Input.GetKeyDown(KeyCode.Alpha2)) weaponType = WeaponType.BouncyGrenade;
        if (Input.GetKeyDown(KeyCode.Alpha3)) weaponType = WeaponType.StickyGrenade;
        
        switch (weaponType)
        {
            case WeaponType.Melee:
                HandleMeleeInput();
                break;
            case WeaponType.BouncyGrenade:
                HandleBouncyGrenadeInput();
                break;
            case WeaponType.StickyGrenade:
                HandleStickyGrenadeInput();
                break;
        }
    }

    private void HandleMeleeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DamageEnemiesInRange(weapon.GetShootingPosition(), meleeDamageRadius);
        }
    }
    
    private void HandleBouncyGrenadeInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (BouncyGrenade.CanReload())
            {
                BouncyGrenade.Reload();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootBouncyGrenade();
        }
    }

    private void HandleStickyGrenadeInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (StickyGrenade.CanReload())
            {
                StickyGrenade.Reload();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootStickyGrenade();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            StickyGrenade.ExplodeAllGrenades();
        }
    }

    private void ShootBouncyGrenade()
    {
        if (BouncyGrenade.HasAmmo()) 
        {
            BouncyGrenade.Create(bouncyGrenadePrefab, weapon.GetShootingPosition(), Utils.GetMouseWorldPosition(), OnGrenadeExplode);
        }
    }
    
    private void ShootStickyGrenade()
    {
        if (StickyGrenade.HasAmmo()) 
        {
            StickyGrenade.Create(stickyGrenadePrefab, weapon.GetShootingPosition(), Utils.GetMouseWorldPosition(), OnGrenadeExplode);
        }
    }

    private void OnGrenadeExplode(Vector3 position)
    {
        Instantiate(grenadeExplosionPrefab, position, Quaternion.identity);
        DamageEnemiesInRange(position, grenadeDamageRadius);
    }

    private void DamageEnemiesInRange(Vector3 position, float range)
    {
        // Damage enemies in blast radius
        List<Enemy> enemiesListCopy = new List<Enemy>(enemiesList);
        foreach (Enemy enemy in enemiesListCopy)
        {
            if (Vector3.Distance(enemy.GetPosition(), position) < range)
            {
                enemy.Die();
                enemiesList.Remove(enemy);
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomPosition = enemySpawnPositionsArray[Random.Range(0, enemySpawnPositionsArray.Length)];
        Transform enemyTransform = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        Enemy enemy = enemyTransform.gameObject.GetComponent<Enemy>();
        
        enemiesList.Add(enemy);
    }
}
