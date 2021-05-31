using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Compass { C, N, E, S, W }

public class Room : MonoBehaviour
{
    public Vector2Int roomCoordinate;

    [HideInInspector]public Compass origin = Compass.C;
    [HideInInspector]public Compass destination = Compass.C;
    public List<Compass> openingDirections;
    public Dictionary<Compass, Room> neighbors = new Dictionary<Compass, Room>();
    Dictionary<GameObject, Vector3> objectsInRoom = new Dictionary<GameObject, Vector3>();
    GameObject door;

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
    }

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
        //destination = _direction;
        return true;
    }

    public Room SetEndRoom()
    {
        // instantiate exit / boss / whatever
        
        return this;
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
        Room r = this;
        //as long as currently referenced room is not the starting room
        while (r.origin != Compass.C)
        {
            if (r.neighbors.ContainsKey(r.origin))
            {
                //step into room's origin neighbor and count up
                r = r.neighbors[r.origin];
                i++;
            }
        }
        return i;
    }

    public GameObject PlaceObject(GameObject _gameObject, Vector3 _offsetFromCenter)
    {
        if (Mathf.Abs(_offsetFromCenter.x) > 9.0f || Mathf.Abs(_offsetFromCenter.z) > 9.0f)
        {
            Debug.Log("Object is placed outside of the room!");
            return null;
        }

        Vector3 spawnPosition = new Vector3(transform.position.x + _offsetFromCenter.x, 0.1f, transform.position.z + _offsetFromCenter.z);

        GameObject placedObject = Instantiate(_gameObject, spawnPosition, Quaternion.identity); //todo: add rotation
        objectsInRoom.Add(placedObject, _offsetFromCenter);
        return placedObject;
    }

    public void PlaceDoorAtOrigin(GameObject _doorPrefab)
    {
        if (origin == Compass.N)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0.1f, transform.position.z + 10.0f);
            GameManager.Instance.door = Instantiate(_doorPrefab, spawnPosition, Quaternion.identity);
            return;
        }
        else if (origin == Compass.S)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0.1f, transform.position.z - 10.0f);
            GameManager.Instance.door = Instantiate(_doorPrefab, spawnPosition, Quaternion.identity);
            return;
        }
        else if (origin == Compass.W)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x - 10.0f, 0.1f, transform.position.z);
            GameManager.Instance.door = Instantiate(_doorPrefab, spawnPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
            return;
        }
        else if (origin == Compass.W)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 10.0f, 0.1f, transform.position.z);
            GameManager.Instance.door = Instantiate(_doorPrefab, spawnPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
            return;
        }
    }
}
