using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(Player.pInstance.transform.position.x, transform.position.y, Player.pInstance.transform.position.z);
    }
}
