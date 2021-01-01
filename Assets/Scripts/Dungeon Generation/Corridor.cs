using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor
{
    public Vector2Int position, size, exit1, exit2;

    public Corridor(Vector2Int _position, Vector2Int _size, Vector2Int _exit1, Vector2Int _exit2)
    {
        position = _position;
        size = _size;
        exit1 = _exit1;
        exit2 = _exit2;
        GameManager.Instance.AddCorridor(this);
    }
}
