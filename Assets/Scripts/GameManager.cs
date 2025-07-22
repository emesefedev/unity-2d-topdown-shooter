using Emesefe.Utilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform bouncyGrenadePrefab;
    [SerializeField] private Transform grenadeExplosionPrefab;

    private float damageRadius = 5f;

    private void OnEnable()
    {
        Weapon.OnShoot += Weapon_OnShoot;
    }

    private void Weapon_OnShoot()
    {
        BouncyGrenade.Create(bouncyGrenadePrefab, weapon.GetShootingPosition(), Utils.GetMouseWorldPosition(), OnGrenadeExplode);
    }

    private void OnGrenadeExplode(Vector3 position)
    {
        Instantiate(grenadeExplosionPrefab, position, Quaternion.identity);
    }
}
