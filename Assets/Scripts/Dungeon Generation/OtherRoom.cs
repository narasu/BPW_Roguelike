using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherRoom 
{
    public Vector2Int size, position;

    public OtherRoom(Vector2Int _size, Vector2Int _position)
    {
        size = _size;
        position = _position;
        GameManager.Instance.AddRoom(this);
    }
}
