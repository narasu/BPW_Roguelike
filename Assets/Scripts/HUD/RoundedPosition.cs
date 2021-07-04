using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float roundX = Mathf.Floor(transform.position.x);
        float roundZ = Mathf.Floor(transform.position.z);
        Vector3 roundPosition = new Vector3(roundX, transform.position.y, roundZ);
        transform.position = roundPosition;
    }
}
