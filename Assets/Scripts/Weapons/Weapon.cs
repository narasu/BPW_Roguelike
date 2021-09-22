﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    private WeaponDecorator weaponDecorator;
    public LayerMask layerMask;
    public int pDamage 
    {
        get => damage;
        set => damage = value;
    }
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject lineEffect;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private int ammo, damage;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform firePoint;
    [HideInInspector] public bool automatic;
    private float fireCooldown;
    
    private bool canFire = true;
    private bool pCanFire
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

    public void Decorate(WeaponDecorator _decorator)
    {
        weaponDecorator = _decorator;
    }

    public void RemoveDecorator()
    {
        weaponDecorator = null;
    }

    public virtual void Shoot()
    {
        if (!canFire)
            return;


        fireCooldown = fireRate;
        Instantiate(muzzleFlash, firePoint);

        if (Physics.Raycast(firePoint.position, firePoint.right, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Vector3[] points = new Vector3[2];
            points[0] = firePoint.position;
            points[1] = hit.point;
            GameObject line = Instantiate(lineEffect);
            line.GetComponent<LineRenderer>()?.SetPositions(points);
            Instantiate(bulletImpact, hit.point, firePoint.rotation);
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        //hit.collider?.GetComponent<IDamageable>()?.TakeDamage(damage);

        pCanFire = false;
    }

    private void Start()
    {
        automatic = false;
    }

    private void Update()
    {
        if (collisionCount > 0) pCanFire = false;

        if (fireCooldown > 0)
        {
            pCanFire = false;
            fireCooldown -= Time.deltaTime;
        }
        else if (collisionCount == 0)
        {
            pCanFire = true;
        }

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

    }

    
    int collisionCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Wall")
        {
            collisionCount++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            collisionCount--;
        }
    }
}