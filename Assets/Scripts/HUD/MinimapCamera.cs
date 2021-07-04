using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(Player.Instance.transform.position.x, transform.position.y, Player.Instance.transform.position.z);
    }
}
