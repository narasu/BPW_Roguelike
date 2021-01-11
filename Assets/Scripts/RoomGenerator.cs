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

    private GameObject[,] roomGrid;
    private List<Room> spawnedRooms = new List<Room>();
    private Room currentRoom;
    public Vector2 roomSize;

    //public GameObject[] NorthRooms { get => northRooms; }
    //public GameObject[] SouthRooms { get => southRooms; }
    //public GameObject[] WestRooms { get => westRooms; }
    //public GameObject[] EastRooms { get => eastRooms; }

    [SerializeField] private GameObject[] northRooms, southRooms, westRooms, eastRooms;
    [SerializeField] private GameObject closedRoom;
    private void Awake()
    {
        instance = this;

        roomGrid = new GameObject[numberOfRooms, numberOfRooms];
        roomGrid[numberOfRooms / 2, numberOfRooms / 2] = startingRoom;

        currentRoom = startingRoom.GetComponent<Room>();
        currentRoom.roomCoordinate = new Vector2Int(numberOfRooms / 2, numberOfRooms / 2);

        spawnedRooms.Add(currentRoom);
    }

    private void Start()
    {
        StartCoroutine("SpawnRooms");
    }

    void Update()
    {
        
    }

    public bool IsOccupied(Vector2Int _position)
    {
        if (roomGrid[_position.x, _position.y]!=null)
        {
            Debug.Log("Position is occupied");
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

            //if (currentRoom.origin!=Compass.C && currentRoom.destination!=Compass.C)
            //{
            //    while (IsOccupied(currentRoom.adjacentCoords[currentRoom.destination]))
            //    {
            //        Debug.Log("i am doing te thing");
            //        bool allOccupied = true;
            //        //bool hasOpening = false;

            //        foreach(KeyValuePair<Compass, Vector2Int> j in currentRoom.adjacentCoords)
            //        {
            //            if (!IsOccupied(j.Value))
            //            {
            //                //if(currentRoom.IsOpen(j.Key))
            //                //{
            //                //    hasOpening = true;
            //                //}
            //                allOccupied = false;
            //            }
            //        }
            //        //if (hasOpening)
            //        //{
            //        //    Debug.Log("room has opening");
            //        //    break;
            //        //}
            //        if (allOccupied)
            //        {
            //            Debug.Log(" all spaces are occupied");
            //            break;
            //        }
                        

            //        if (currentRoom.origin==Compass.N)
            //        {
            //            GameObject replacementRoom = Instantiate(northRooms[Random.Range(1, northRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
            //            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
            //            Destroy(currentRoom);
            //            currentRoom = replacementRoom.GetComponent<Room>();
            //        }
            //        else if (currentRoom.origin == Compass.S)
            //        {
            //            GameObject replacementRoom = Instantiate(southRooms[Random.Range(1, southRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
            //            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
            //            Destroy(currentRoom);
            //            currentRoom = replacementRoom.GetComponent<Room>();
            //        }
            //        else if (currentRoom.origin == Compass.W)
            //        {
            //            GameObject replacementRoom = Instantiate(westRooms[Random.Range(1, westRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
            //            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
            //            Destroy(currentRoom);
            //            currentRoom = replacementRoom.GetComponent<Room>();
            //        }
            //        else if (currentRoom.origin == Compass.E)
            //        {
            //            GameObject replacementRoom = Instantiate(eastRooms[Random.Range(1, eastRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
            //            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
            //            Destroy(currentRoom);
            //            currentRoom = replacementRoom.GetComponent<Room>();
                        
            //        }
            //    }
            //}


            //if (IsOccupied(currentRoom.roomCoordinate + Vector2Int.up) && !currentRoom.neighbors.ContainsValue(roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y + 1].GetComponent<Room>()))
            //{
            //    GameObject replacementRoom = Instantiate(southRooms[Random.Range(1, northRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
            //    roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
            //    Destroy(currentRoom);
            //    currentRoom = replacementRoom.GetComponent<Room>();

            //}



            List<Room> nextRooms = new List<Room>();

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



            foreach (Room r in nextRooms)
            {

                //currentRoom.Connect(r);
                spawnedRooms.Add(r);
            }

            if (i == spawnedRooms.Count-1)
            {
                
                break;
            }

            i++;
            yield return new WaitForSeconds(0.1f);
        }

        
            //for (int i=0; i<spawnedRooms.Count; i++)
            //{
            //    //if (currentRoom == null)
            //    //{
            //    //    continue;
            //    //}
                
            //}

            

        // spawn neighbors of currentRoom
        // add neighbors of currentRoom to nextRooms
        // spawn nextRooms & add neighbors
        // etc or whatever



        //foreach (Room room in spawnedRooms)
        //{
        //    List<Vector2Int> neighborCoordinates = room.NeighborCoordinates();
        //    foreach (Vector2Int coordinate in neighborCoordinates)
        //    {
        //        Room neighbor = this.rooms[coordinate.x, coordinate.y];
        //        if (neighbor != null)
        //        {
        //            room.Connect(neighbor);
        //        }
        //    }
        //}

        //yield return new WaitForSeconds(0.1f);

        //if (currentRoom.spawned)
        //    return;

        ////this is embarrassing
        //switch (openingDirection)
        //{
        //    case 1:
        //        // door on north side
        //        Instantiate(northRooms[Random.Range(0, northRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
        //        break;
        //    case 2:
        //        // door on east side
        //        Instantiate(eastRooms[Random.Range(0, eastRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
        //        break;
        //    case 3:
        //        // door on south side
        //        Instantiate(southRooms[Random.Range(0, southRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
        //        break;
        //    case 4:
        //        // door on west side
        //        Instantiate(westRooms[Random.Range(0, westRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
        //        break;
        //}

        //currentRoom.spawned = true;
    }


}
