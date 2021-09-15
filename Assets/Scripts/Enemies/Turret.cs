using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    private Vector3 direction;
    
    private void Update()
    {
        direction = (Player.pInstance.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        if (Input.GetKeyDown(KeyCode.F)) Shoot();
    }

    private void Shoot()
    {
        GameObject b = Instantiate(bullet, firePoint.position, Quaternion.Euler(direction));
    }
}