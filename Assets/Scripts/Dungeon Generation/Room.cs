using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2Int size, position;
    public Room neighbor;

    public Room(Vector2Int _position, Vector2Int _size, Room _neighbor)
    {
        size = _size;
        position = _position;
        neighbor = _neighbor;
        GameManager.Instance.AddRoom(this);
    }
}
