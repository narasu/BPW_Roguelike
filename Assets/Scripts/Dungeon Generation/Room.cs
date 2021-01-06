using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2Int size, position;

    public Room(Vector2Int _position, Vector2Int _size)
    {
        size = _size;
        position = _position;
        GameManager.Instance.AddRoom(this);
    }
}
