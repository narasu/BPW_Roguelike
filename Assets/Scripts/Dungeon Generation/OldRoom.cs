﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cardinal { None, North, West, South, East }

public class OldRoom
{
    public Vector2Int roomCoordinate;
    public Dictionary<Cardinal, OldRoom> neighbors;
    public Cardinal[] exits;

    public OldRoom(int x, int y)
    {
        roomCoordinate = new Vector2Int(x, y);
        neighbors = new Dictionary<Cardinal, OldRoom>();
    }

    public OldRoom(Vector2Int roomCoordinate)
    {
        this.roomCoordinate = roomCoordinate;
        neighbors = new Dictionary<Cardinal, OldRoom>();
    }

    public List<Vector2Int> NeighborCoordinates()
    {
        List<Vector2Int> neighborCoordinates = new List<Vector2Int>();
        neighborCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y - 1));
        neighborCoordinates.Add(new Vector2Int(roomCoordinate.x + 1, roomCoordinate.y));
        neighborCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y + 1));
        neighborCoordinates.Add(new Vector2Int(roomCoordinate.x - 1, roomCoordinate.y));

        return neighborCoordinates;
    }

    public void Connect(OldRoom neighbor)
    {
        Cardinal direction = Cardinal.None;
        if (neighbor.roomCoordinate.y < roomCoordinate.y)
        {
            direction = Cardinal.North;
        }
        if (neighbor.roomCoordinate.y > roomCoordinate.y)
        {
            direction = Cardinal.South;
        }
        if (neighbor.roomCoordinate.x < roomCoordinate.x)
        {
            direction = Cardinal.West;
        }
        if (neighbor.roomCoordinate.x > roomCoordinate.x)
        {
            direction = Cardinal.East;
        }
        neighbors.Add(direction, neighbor);
    }

}
