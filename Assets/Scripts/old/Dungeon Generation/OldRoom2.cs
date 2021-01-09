using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldRoom2 
{
    public Vector2Int size, position;
    public OldRoom2 neighbor;

    public OldRoom2(Vector2Int _position, Vector2Int _size, OldRoom2 _neighbor)
    {
        size = _size;
        position = _position;
        neighbor = _neighbor;
        OldGameManager.Instance.AddRoom(this);
    }
}
