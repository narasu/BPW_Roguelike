using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Compass { C, N, E, S, W }

public class Room : MonoBehaviour
{
    public Vector2Int roomCoordinate;

    public Compass origin = Compass.C;
    public Compass destination = Compass.C;
    public List<Compass> openingDirections;
    public Dictionary<Compass, Room> neighbors = new Dictionary<Compass, Room>();
    public Dictionary<Compass, Vector2Int> adjacentCoords = new Dictionary<Compass, Vector2Int>();

    public void Initialize(Compass _origin, Vector2Int _roomCoordinate)
    {
        SetNeighborCoordinates();
        origin = _origin;
        roomCoordinate = _roomCoordinate;

        for(int i=0; i<openingDirections.Count; i++)
        {
            if (openingDirections[i] != origin)
            {
                destination = openingDirections[i];
            }
        }
    }

    public void SetNeighborCoordinates()
    {
        adjacentCoords[Compass.N] = roomCoordinate + Vector2Int.up;
        adjacentCoords[Compass.E] = roomCoordinate + Vector2Int.right;
        adjacentCoords[Compass.S] = roomCoordinate + Vector2Int.down;
        adjacentCoords[Compass.W] = roomCoordinate + Vector2Int.left;
    }

    //public List<Vector2Int> NeighborCoordinates()
    //{
    //    List<Vector2Int> neighborCoordinates = new List<Vector2Int>();
    //    neighborCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y - 1));
    //    neighborCoordinates.Add(new Vector2Int(roomCoordinate.x + 1, roomCoordinate.y));
    //    neighborCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y + 1));
    //    neighborCoordinates.Add(new Vector2Int(roomCoordinate.x - 1, roomCoordinate.y));

    //    return neighborCoordinates;
    //}


    public bool IsOpen(Compass _direction)
    {
        if (!openingDirections.Contains(_direction))
        {
            Debug.Log("room is closed on side" + _direction);
            return false;
        }

        if (neighbors.ContainsKey(_direction))
        {
            Debug.Log("there is a neighbor on side" + _direction);
            return false;
        }

        destination = _direction;
        return true;
    }    

    public void Connect(Room _neighbor)
    {
        if (neighbors.ContainsValue(_neighbor))
        {
            Debug.Log("neighbor has already been added");
            return;
        }
        
        if (_neighbor.roomCoordinate.y > roomCoordinate.y)
        {
            if (IsOpen(Compass.N))
            {
                neighbors.Add(Compass.N, _neighbor);
                _neighbor.neighbors.Add(Compass.S, this);
            }
                
            return;
        }
        if (_neighbor.roomCoordinate.x > roomCoordinate.x)
        {
            if (IsOpen(Compass.E))
            {
                neighbors.Add(Compass.E, _neighbor);
                _neighbor.neighbors.Add(Compass.W, this);
            }
                
            return;
        }
        if (_neighbor.roomCoordinate.y < roomCoordinate.y)
        {
            if (IsOpen(Compass.S))
            {
                neighbors.Add(Compass.S, _neighbor);
                _neighbor.neighbors.Add(Compass.N, this);
            }
                
            return;
        }
        if (_neighbor.roomCoordinate.x < roomCoordinate.x)
        {
            if (IsOpen(Compass.W))
            {
                neighbors.Add(Compass.W, _neighbor);
                _neighbor.neighbors.Add(Compass.E, this);
            }
                
            return;
        }

    }

}
