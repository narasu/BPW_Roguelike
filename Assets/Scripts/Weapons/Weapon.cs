using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private int ammo, damage;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool automatic;

    private bool canFire = true;
    private bool CanFire
    {
        set
        {
            if (!canFire && value)
            {
                // if it's not automatic, player needs to click again
                if (!automatic)
                {
                    if (Input.GetMouseButtonUp(0)) canFire = value;
                }
                else
                {
                    canFire = value;
                }
            }
            else
            {
                canFire = value;
            }
            
        }
    }

    private float fireCooldown;

    private void Update()
    {
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }
        else
        {
            CanFire = true;
        }
    }

    public virtual void Shoot()
    {
        if (!canFire)
            return;

        Debug.Log("pew pew");

        fireCooldown = fireRate;
        Instantiate(muzzleFlash, firePoint);


        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
        CanFire = false;
        
    }
}