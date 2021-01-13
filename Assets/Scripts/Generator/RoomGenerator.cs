using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //private int Bla
    //{
    //    get { return 1; }
    //}

    [SerializeField][Range(1,50)] private int numberOfRooms;

    [SerializeField] private GameObject startingRoom;
    private GameObject bossRoom;

    private GameObject[,] roomGrid;
    private List<Room> spawnedRooms = new List<Room>();
    private Room currentRoom;
    public Vector2 roomSize;

    private bool roomsSpawned = false;
    //private bool RoomsSpawned
    //{
    //    set
    //    {
    //        if (!roomsSpawned && value)
    //        {
                
    //            roomsSpawned = true;

    //        }
    //    }
    //}

    public bool useSeed;
    public int seed;

    //public GameObject[] NorthRooms { get => northRooms; }
    //public GameObject[] SouthRooms { get => southRooms; }
    //public GameObject[] WestRooms { get => westRooms; }
    //public GameObject[] EastRooms { get => eastRooms; }

    [SerializeField] private GameObject[] northRooms, southRooms, westRooms, eastRooms;
    [SerializeField] private GameObject closedRoom;
    private void Awake()
    {
        instance = this;

        if (useSeed) Random.InitState(seed);

        roomGrid = new GameObject[numberOfRooms, numberOfRooms];
        roomGrid[numberOfRooms / 2, numberOfRooms / 2] = startingRoom;

        currentRoom = startingRoom.GetComponent<Room>();
        currentRoom.roomCoordinate = new Vector2Int(numberOfRooms / 2, numberOfRooms / 2);
        currentRoom.SetNeighborCoordinates();
        spawnedRooms.Add(currentRoom);
    }

    private void Start()
    {
        StartCoroutine("SpawnRooms");
    }

    private bool IsOccupied(Vector2Int _position)
    {
        if (roomGrid[_position.x, _position.y]!=null)
        {
            //Debug.Log("Position is occupied");
            return true;
        }
        return false;
    }

    private IEnumerator SpawnRooms()
    {
        int i = 0;
        
        
        while (spawnedRooms.Count < numberOfRooms)
        {
            currentRoom = spawnedRooms[i];

            // if this isn't the starting room
            if (i != 0 && currentRoom.destination!=Compass.C)
            {
                //if the destination of this room is occupied by another
                while (IsOccupied(currentRoom.adjacentCoords[currentRoom.destination]))
                {
                    // if the room is completely enclosed, there is no possible exit to the loop
                    bool allOccupied = true;
                    foreach (KeyValuePair<Compass, Vector2Int> j in currentRoom.adjacentCoords)
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

                    // keep replacing the current room until there is a valid exit
                    if (currentRoom.origin == Compass.N)
                    {
                        GameObject replacementRoom = Instantiate(northRooms[Random.Range(1, northRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.N, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                    }
                    else if (currentRoom.origin == Compass.S)
                    {
                        GameObject replacementRoom = Instantiate(southRooms[Random.Range(1, southRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.S, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                    }
                    else if (currentRoom.origin == Compass.W)
                    {
                        GameObject replacementRoom = Instantiate(westRooms[Random.Range(1, westRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.W, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                    }
                    else if (currentRoom.origin == Compass.E)
                    {
                        GameObject replacementRoom = Instantiate(eastRooms[Random.Range(1, eastRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.E, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                    }
                }
            }

            List<Room> nextRooms = new List<Room>();

            //check at each opening if there is space available and spawn a new room there

            if (currentRoom.IsOpen(Compass.N) && !IsOccupied(currentRoom.roomCoordinate + Vector2Int.up))
            {
                Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y + roomSize.y);
                GameObject o = Instantiate(southRooms[Random.Range(1, southRooms.Length)], neighborPosition, Quaternion.identity, transform);

                Room newRoom = o.GetComponent<Room>();
                newRoom.Initialize(Compass.S, currentRoom.roomCoordinate + Vector2Int.up);
                
                roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y + 1] = o;
                nextRooms.Add(newRoom);

                currentRoom.Connect(newRoom);
            }

            if (currentRoom.IsOpen(Compass.S) && !IsOccupied(currentRoom.roomCoordinate + Vector2Int.down))
            {
                Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y - roomSize.y);
                GameObject o = Instantiate(northRooms[Random.Range(1, northRooms.Length)], neighborPosition, Quaternion.identity, transform);

                Room newRoom = o.GetComponent<Room>();
                newRoom.Initialize(Compass.N, currentRoom.roomCoordinate + Vector2Int.down);
                
                roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y - 1] = o;
                nextRooms.Add(newRoom);

                currentRoom.Connect(newRoom);
            }

            if (currentRoom.IsOpen(Compass.E) && !IsOccupied(currentRoom.roomCoordinate + Vector2Int.right))
            {
                Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x + roomSize.x, currentRoom.transform.position.y);
                GameObject o = Instantiate(westRooms[Random.Range(1, westRooms.Length)], neighborPosition, Quaternion.identity, transform);

                Room newRoom = o.GetComponent<Room>();
                newRoom.Initialize(Compass.W, currentRoom.roomCoordinate + Vector2Int.right);
                
                roomGrid[currentRoom.roomCoordinate.x + 1, currentRoom.roomCoordinate.y] = o;
                nextRooms.Add(newRoom);

                currentRoom.Connect(newRoom);
            }

            if (currentRoom.IsOpen(Compass.W) && !IsOccupied(currentRoom.roomCoordinate + Vector2Int.left))
            {
                Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x - roomSize.x, currentRoom.transform.position.y);
                GameObject o = Instantiate(eastRooms[Random.Range(1, eastRooms.Length)], neighborPosition, Quaternion.identity, transform);

                Room newRoom = o.GetComponent<Room>();
                newRoom.Initialize(Compass.E, currentRoom.roomCoordinate + Vector2Int.left);
                
                roomGrid[currentRoom.roomCoordinate.x - 1, currentRoom.roomCoordinate.y] = o;
                nextRooms.Add(newRoom);

                currentRoom.Connect(newRoom);
            }


            //add all newly created rooms to the list
            foreach (Room r in nextRooms)
            {
                spawnedRooms.Add(r);
            }


            if (i == spawnedRooms.Count-1)
            {
                break;
            }

            i++;
            //yield return new WaitForEndOfFrame();
        }
        Debug.Log(spawnedRooms.Count);

        StartCoroutine("CloseRooms");
    }

    private IEnumerator CloseRooms()
    {
        for(int i=0; i<spawnedRooms.Count; i++)
        {
            if(spawnedRooms[i]==null)
            { 
                continue;
            }
            if (spawnedRooms[i].IsConnected())
            {
                continue;
            }

            if (spawnedRooms[i].origin == Compass.N)
            {
                GameObject replacementRoom = Instantiate(northRooms[0], spawnedRooms[i].transform.position, Quaternion.identity, transform);
                replacementRoom.GetComponent<Room>().Initialize(Compass.N, spawnedRooms[i].roomCoordinate);
                roomGrid[spawnedRooms[i].roomCoordinate.x, spawnedRooms[i].roomCoordinate.y] = replacementRoom;
                GameObject oldRoom = spawnedRooms[i].gameObject;
                spawnedRooms[i] = replacementRoom.GetComponent<Room>();
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
                Destroy(oldRoom);
                yield return new WaitForEndOfFrame();
                continue;
            }
        }
        
    }


}