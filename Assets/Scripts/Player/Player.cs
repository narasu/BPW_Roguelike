using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    public Weapon activeWeapon;
    private static Player instance;
    public static Player Instance
    { 
        get { return instance; }
    }
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //shoot
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 movement = movementDirection * movementSpeed;
        rb.velocity = movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<ICollectible>()?.Collect();
    }
}
