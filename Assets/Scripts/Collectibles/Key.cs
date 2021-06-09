using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        GameManager.Instance.keysInLevel.Remove(this);
        // TODO: make OnKeyCollected event in GameManager, invoke it here
        Debug.Log("collected");
        Destroy(gameObject);
    }
}
