using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private Transform objectToFollow;

    public void Initialize(Transform _objectToFollow) => objectToFollow = _objectToFollow;
    private void FixedUpdate()
    {
        transform.position = objectToFollow.position;
    }
}
