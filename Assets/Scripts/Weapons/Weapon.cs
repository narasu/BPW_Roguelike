using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject lineEffect;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private int ammo, damage;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool automatic;
    public LayerMask layerMask;

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
        else if (collisionCount == 0)
        {
            CanFire = true;
        }
        if (collisionCount > 0) CanFire = false;
        else CanFire = true;
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

        CanFire = false;
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