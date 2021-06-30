using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed;
    public Weapon activeWeapon;
    public static Player Instance { get => instance; }
    private static Player instance;

    private Rigidbody rb;
    public bool IsHit { get => isHit; }
    private bool isHit = false;
    

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

    
    public void TakeDamage()
    {
        if (!isHit)
        {
            isHit = true;
            rb.velocity = Vector3.zero;
            health--;
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
        
        yield return new WaitForSeconds(0.65f);
        isHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit"))
        {
            GameManager.Instance.WinGame();
            return;
        }
        Debug.Log("hit: " + other.gameObject);
        other.GetComponent<ICollectible>()?.Collect();
    }
}
