using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody rb;

    public Weapon activeWeapon;

    private static Player instance;
    public static Player Instance
    { 
        get { return instance; }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //activeWeapon.GetComponent<IWeapon>()?.Shoot();
        }
    }

    private void FixedUpdate()
    {
        Move();
        MouseLook();
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 movement = movementDirection * movementSpeed;

        rb.velocity = movement;
    }

    private void MouseLook()
    {
        //get the mouse position in world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);

        //determine the angle between player and mouse
        float angle = AngleBetweenTwoPoints(transform.position, targetPosition);
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
        
        //returns angle in degrees between two points in world space
        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.z-b.z, b.x - a.x) * Mathf.Rad2Deg;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag=="Obstacle")
    //    {
    //        Debug.Log("hit");
    //    }
    //}
}
