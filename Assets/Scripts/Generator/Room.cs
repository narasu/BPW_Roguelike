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
    //public Dictionary<Compass, Vector2Int> adjacentCoords = new Dictionary<Compass, Vector2Int>();

    public void Initialize(Compass _origin, Vector2Int _roomCoordinate)
    {
        origin = _origin;
        roomCoordinate = _roomCoordinate;
        //SetNeighborCoordinates();

        for (int i=0; i<openingDirections.Count; i++)
        {
            if (openingDirections[i] != origin)
            {
                destination = openingDirections[i];
            }
        }
        //Debug.Log(destination);
    }

    //public void SetNeighborCoordinates()
    //{
    //    adjacentCoords[Compass.N] = roomCoordinate + Vector2Int.up;
    //    adjacentCoords[Compass.E] = roomCoordinate + Vector2Int.right;
    //    adjacentCoords[Compass.S] = roomCoordinate + Vector2Int.down;
    //    adjacentCoords[Compass.W] = roomCoordinate + Vector2Int.left;
    //}

    //does this room have an opening, and an empty space at the given direction?
    public bool IsOpen(Compass _direction)
    {
        if (!openingDirections.Contains(_direction))
        {
            //Debug.Log("room is closed on side" + _direction);
            return false;
        }

        if (neighbors.ContainsKey(_direction))
        {
            //Debug.Log("there is a neighbor on side" + _direction);
            return false;
        }

        //if so, we know this to be its destination
        destination = _direction;
        return true;
    }    

    //does this room have a neighbor at its destination?
    public bool IsConnected()
    {
        if (neighbors.ContainsKey(destination))
        {
            return true;
        }
        return false;
    }

    // give this room a reference to its neighbor, and vice versa
    public void Connect(Room _neighbor)
    {
        if (neighbors.ContainsValue(_neighbor))
        {
            //Debug.Log("neighbor has already been added");
            return;
        }
        
        if (_neighbor.roomCoordinate.y > roomCoordinate.y)
        {
            //if (IsOpen(Compass.N))
            //{
                neighbors.Add(Compass.N, _neighbor);
                _neighbor.neighbors.Add(Compass.S, this);
            //}
            return;
        }
        if (_neighbor.roomCoordinate.x > roomCoordinate.x)
        {
            //if (IsOpen(Compass.E))
            //{
                neighbors.Add(Compass.E, _neighbor);
                _neighbor.neighbors.Add(Compass.W, this);
            //}
                
            return;
        }
        if (_neighbor.roomCoordinate.y < roomCoordinate.y)
        {
            //if (IsOpen(Compass.S))
            //{
                neighbors.Add(Compass.S, _neighbor);
                _neighbor.neighbors.Add(Compass.N, this);
            //}
                
            return;
        }
        if (_neighbor.roomCoordinate.x < roomCoordinate.x)
        {
            //if (IsOpen(Compass.W))
            //{
                neighbors.Add(Compass.W, _neighbor);
                _neighbor.neighbors.Add(Compass.E, this);
            //}
                
            return;
        }

    }

    public int CountDistanceFromStart()
    {
        int i = 0;
        //Debug.Log(origin);
        //Debug.Log("dafucc");
        //Debug.Log(neighbors.Count);
        //foreach (KeyValuePair<Compass, Room> kvp in neighbors)
        //{
        //    Debug.Log(kvp.Key);
        //}
        //Debug.Log(neighbors);


        Room r = this;
        //as long as currently referenced room is not the starting room
        while (r.origin != Compass.C)
        {
            if (r.neighbors.ContainsKey(r.origin))
            {
                //step into room's originating neighbor and count up
                r = r.neighbors[r.origin];
                i++;
            }
            
        }
        return i;
    }

}
