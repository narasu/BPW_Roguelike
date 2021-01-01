using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2Int size, position;

    public Room(Vector2Int _size, Vector2Int _position)
    {
        size = _size;
        position = _position;
        GameManager.Instance.AddRoom(this);
    }
}
