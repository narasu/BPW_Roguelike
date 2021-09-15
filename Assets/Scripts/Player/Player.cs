using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour, IDetectable
{
    
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed;
    public int Health { get => health; }
    [HideInInspector]public Weapon activeWeapon;
    public static Player Instance { get => instance; }
    private static Player instance;
    private Rigidbody rb;
    private NavMeshObstacle navMeshObstacle;
    public bool IsHit { get => isHit; }
    private bool isHit = false;
    
    public IDetectable ReturnTarget()
    {
        return this;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //shoot
            activeWeapon.Shoot();
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

    
    public void TakeDamage()
    {
        if (!isHit)
        {
            isHit = true;
            rb.velocity = Vector3.zero;
            health--;
            EventManager.RaiseEvent(EventType.PLAYER_HIT);
            if (health <= 0)
            {
                Die();
                return;
            }
            StartCoroutine(ImmuneTimer());
        }
    }

    private void Die()
    {
        Debug.Log("ded");
        EventManager.RaiseEvent(EventType.PLAYER_DIED);
    }

    private IEnumerator ImmuneTimer()
    {
        navMeshObstacle.enabled = true;
        yield return new WaitForSeconds(0.65f);
        isHit = false;
        navMeshObstacle.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit: " + other.gameObject);
        if (other.gameObject.CompareTag("Exit"))
        {
            EventManager.RaiseEvent(EventType.PLAYER_WIN);
            return;
        }
        
        other.GetComponent<ICollectible>()?.Collect();
    }
}