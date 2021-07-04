using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        GameManager.Instance.keysInLevel.Remove(this);
        EventManager.RaiseEvent(EventType.KEY_COLLECTED);
        Destroy(gameObject);
    }
}
