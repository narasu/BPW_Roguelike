using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class instantiates and stores all rooms in the level, connects them together and closes off any loose ends.
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    private static RoomGenerator instance;
    public static RoomGenerator Instance
    {
        get 
        { 
            if (instance==null)
            {
                instance = FindObjectOfType<RoomGenerator>();
            }
            return instance; 
        }
    }
    [Header("Generation parameters")]
    public bool UseSeed;
    public int Seed;
    [SerializeField] [Range(5, 50)] private int numberOfRooms;
    public Vector2 roomSize;
    [Header("Room Prefabs")]
    [SerializeField] private GameObject startingRoom;
    [SerializeField] private GameObject[] northRooms, southRooms, westRooms, eastRooms;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private EnemySpawner enemySpawner;

    public Room endRoom;
    private GameObject[,] roomGrid;
    public List<Room> spawnedRooms = new List<Room>();
    public List<Room> corridors = new List<Room>();
    public List<Room> closedRooms = new List<Room>();
    private Room currentRoom;
    private bool roomsSpawned = false;
    private bool RoomsSpawned
    {
        set
        {
            if (!roomsSpawned && value)
            {
                navMeshSurface.BuildNavMesh();
                endRoom = closedRooms[0].SetEndRoom();
                endRoom.PlaceDoorAtOrigin(doorPrefab);
                
                for (int i = 0; i < corridors.Count; i++)
                {
                    enemySpawner.SpawnEnemies(EnemyType.Crawler, corridors[i]);
                }

                SpawnKeys();
                roomsSpawned = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (UseSeed)
        {
            Random.InitState(Seed);
        }
        roomGrid = new GameObject[numberOfRooms, numberOfRooms];
        roomGrid[numberOfRooms / 2, numberOfRooms / 2] = startingRoom;
        currentRoom = startingRoom.GetComponent<Room>();
        currentRoom.Initialize(Compass.C, new Vector2Int(numberOfRooms / 2, numberOfRooms / 2));
        spawnedRooms.Add(currentRoom);
    }

    private void Start()
    {
        GenerateRooms();
    }

    private bool IsOccupied(Vector2Int _position)
    {
        if (roomGrid[_position.x, _position.y]!=null)
        {
            return true;
        }
        return false;
    }

    private void GenerateRooms()
    {
        int i = 0;
        while (spawnedRooms.Count < numberOfRooms)
        {
            currentRoom = spawnedRooms[i];
            CreateNewRoom(Compass.N);
            CreateNewRoom(Compass.E);
            CreateNewRoom(Compass.S);
            CreateNewRoom(Compass.W);
            if (i == spawnedRooms.Count-1)
            {
                break;
            }

            i++;
        }
        StartCoroutine(CloseRoomsWithoutDestination());
    }

    public void SpawnKeys()
    {

        // the room at index 0 will be used as boss room
        for (int i = 1; i < closedRooms.Count; i++)
        {
            GameObject placedKey = closedRooms[i].PlaceObject(keyPrefab, Vector3.zero);
            GameManager.Instance.keysInLevel.Add(placedKey.GetComponent<Key>());
        }
    }

    private void CreateNewRoom(Compass _direction)
    {
        if (!currentRoom.IsOpen(_direction))
        {
            return;
        }

        Compass opposite = Compass.C;
        Vector2Int nextPlace = new Vector2Int(0, 0);
        Vector3 nextTransform = new Vector3(0, 0);
        GameObject[] roomArray = new GameObject[0];

        switch (_direction)
        {
            case Compass.N:
                opposite = Compass.S;
                nextPlace = currentRoom.roomCoordinate + Vector2Int.up;
                nextTransform = new Vector3(currentRoom.transform.position.x, 0, currentRoom.transform.position.z + roomSize.y);
                roomArray = southRooms; 
                break;
            case Compass.E:
                opposite = Compass.W;
                nextPlace = currentRoom.roomCoordinate + Vector2Int.right;
                nextTransform = new Vector3(currentRoom.transform.position.x + roomSize.x, 0, currentRoom.transform.position.z);
                roomArray = westRooms;
                break;
            case Compass.S:
                opposite = Compass.N;
                nextPlace = currentRoom.roomCoordinate + Vector2Int.down;
                nextTransform = new Vector3(currentRoom.transform.position.x, 0, currentRoom.transform.position.z - roomSize.y);
                roomArray = northRooms;
                break;
            case Compass.W:
                opposite = Compass.E;
                nextPlace = currentRoom.roomCoordinate + Vector2Int.left;
                nextTransform = new Vector3(currentRoom.transform.position.x - roomSize.x, 0, currentRoom.transform.position.z);
                roomArray = eastRooms;
                break;
            case Compass.C:
                return;
        }
        
        if (IsOccupied(nextPlace))
        {
            return;
        }

        GameObject temp = roomArray[Random.Range(1, roomArray.Length)];
        bool nextRoomOpen = false;

        while (!nextRoomOpen)
        {
            Room tempRoom = temp.GetComponent<Room>();
            // if the next room is completely enclosed, there is no possible exit to the loop
            bool allOccupied = true;
            foreach (KeyValuePair<Compass, Vector2Int> j in GetAdjacentCoords(nextPlace))
            {
                if (!IsOccupied(j.Value))
                {
                    allOccupied = false;
                }
            }
            // so break manually
            if (allOccupied)
            {
                Debug.Log("all spaces are occupied");
                break;
            }

            for (int j = 0; j < tempRoom.openingDirections.Count; j++)
            {
                //skip origin
                if (tempRoom.openingDirections[j] == opposite)
                {
                    continue;
                }

                Dictionary<Compass, Vector2Int> possibleCoords = GetPossibleCoords(nextPlace);
                if (possibleCoords.ContainsKey(tempRoom.openingDirections[j]))
                {
                    nextRoomOpen = true;
                }
                else
                {
                    // select a new random prefab in the array and try again
                    temp = roomArray[Random.Range(1, roomArray.Length)];
                }
            }
        }

        GameObject o = Instantiate(temp, nextTransform, Quaternion.identity, transform);
        Room newRoom = o.GetComponent<Room>();
        newRoom.Initialize(opposite, nextPlace);
        roomGrid[nextPlace.x, nextPlace.y] = o;
        currentRoom.Connect(newRoom);
        spawnedRooms.Add(newRoom);
    }

    private IEnumerator CloseRoomsWithoutDestination()
    {
        for(int i=0; i<spawnedRooms.Count; i++)
        {
            if(spawnedRooms[i]==null)
            {
                continue;
            }

            if (spawnedRooms[i].HasNeighborAtDestination())
            {
                corridors.Add(spawnedRooms[i]);
                continue;
            }

            if (spawnedRooms[i].origin == Compass.N)
            {
                GameObject replacementRoom = Instantiate(northRooms[0], spawnedRooms[i].transform.position, Quaternion.identity, transform);
                replacementRoom.GetComponent<Room>().Initialize(Compass.N, spawnedRooms[i].roomCoordinate);
                roomGrid[spawnedRooms[i].roomCoordinate.x, spawnedRooms[i].roomCoordinate.y] = replacementRoom;
                GameObject oldRoom = spawnedRooms[i].gameObject;
                spawnedRooms[i] = replacementRoom.GetComponent<Room>();
                closedRooms.Add(replacementRoom.GetComponent<Room>());
                Destroy(oldRoom);
                yield return new WaitForEndOfFrame();
                continue;
            }

            else if (spawnedRooms[i].origin == Compass.S)
            {
                GameObject replacementRoom = Instantiate(southRooms[0], spawnedRooms[i].transform.position, Quaternion.identity, transform);
                replacementRoom.GetComponent<Room>().Initialize(Compass.S, spawnedRooms[i].roomCoordinate);
                roomGrid[spawnedRooms[i].roomCoordinate.x, spawnedRooms[i].roomCoordinate.y] = replacementRoom;
                GameObject oldRoom = spawnedRooms[i].gameObject;
                spawnedRooms[i] = replacementRoom.GetComponent<Room>();
                closedRooms.Add(replacementRoom.GetComponent<Room>());
                Destroy(oldRoom);
                yield return new WaitForEndOfFrame();
                continue;
            }

            else if (spawnedRooms[i].origin == Compass.W)
            {
                GameObject replacementRoom = Instantiate(westRooms[0], spawnedRooms[i].transform.position, Quaternion.identity, transform);
                replacementRoom.GetComponent<Room>().Initialize(Compass.W, spawnedRooms[i].roomCoordinate);
                roomGrid[spawnedRooms[i].roomCoordinate.x, spawnedRooms[i].roomCoordinate.y] = replacementRoom;
                GameObject oldRoom = spawnedRooms[i].gameObject;
                spawnedRooms[i] = replacementRoom.GetComponent<Room>();
                closedRooms.Add(replacementRoom.GetComponent<Room>());
                Destroy(oldRoom);
                yield return new WaitForEndOfFrame();
                continue;
            }

            else if (spawnedRooms[i].origin == Compass.E)
            {
                GameObject replacementRoom = Instantiate(eastRooms[0], spawnedRooms[i].transform.position, Quaternion.identity, transform);
                replacementRoom.GetComponent<Room>().Initialize(Compass.E, spawnedRooms[i].roomCoordinate);
                roomGrid[spawnedRooms[i].roomCoordinate.x, spawnedRooms[i].roomCoordinate.y] = replacementRoom;
                GameObject oldRoom = spawnedRooms[i].gameObject;
                spawnedRooms[i] = replacementRoom.GetComponent<Room>();
                closedRooms.Add(replacementRoom.GetComponent<Room>());
                Destroy(oldRoom);
                yield return new WaitForEndOfFrame();
            }
        }

        RoomsSpawned = true;
    }

    private Dictionary<Compass, Vector2Int> GetAdjacentCoords(Vector2Int _coord)
    {
        Dictionary<Compass, Vector2Int> adjCoords = new Dictionary<Compass, Vector2Int>();
        adjCoords.Add(Compass.W, new Vector2Int(_coord.x - 1, _coord.y));
        adjCoords.Add(Compass.E, new Vector2Int(_coord.x + 1, _coord.y));
        adjCoords.Add(Compass.N, new Vector2Int(_coord.x, _coord.y + 1));
        adjCoords.Add(Compass.S, new Vector2Int(_coord.x, _coord.y - 1));

        return adjCoords;
    }

    private Dictionary<Compass, Vector2Int> GetPossibleCoords(Vector2Int _coord)
    {
        Dictionary<Compass, Vector2Int> adjCoords = new Dictionary<Compass, Vector2Int>();
        
        if (!IsOccupied(_coord + Vector2Int.left)) adjCoords.Add(Compass.W, _coord + Vector2Int.left);
        if (!IsOccupied(_coord + Vector2Int.right)) adjCoords.Add(Compass.E, _coord + Vector2Int.right);
        if (!IsOccupied(_coord + Vector2Int.up)) adjCoords.Add(Compass.N, _coord + Vector2Int.up);
        if (!IsOccupied(_coord + Vector2Int.down)) adjCoords.Add(Compass.S, _coord + Vector2Int.down);

        return adjCoords;
    }
}