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
        SpawnRooms();
    }

    void Update()
    {
        
    }

    private void SpawnRooms()
    {
        while (spawnedRooms.Count < numberOfRooms)
        {

        }


        if (currentRoom == null)
        {
            return;
        }

        List<Room> nextRooms = new List<Room>();

        if (currentRoom.IsOpen(Compass.N))
        {
            Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y + roomSize.y);
            GameObject r = Instantiate(southRooms[Random.Range(0, northRooms.Length)], neighborPosition, Quaternion.identity, transform);
            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y + 1] = r;
            nextRooms.Add(r.GetComponent<Room>());
            //currentRoom.Connect(r.GetComponent<Room>());
        }

        if (currentRoom.IsOpen(Compass.S))
        {
            Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y - roomSize.y);
            GameObject r = Instantiate(northRooms[Random.Range(0, southRooms.Length)], neighborPosition, Quaternion.identity, transform);
            roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y - 1] = r;
            nextRooms.Add(r.GetComponent<Room>());
            //currentRoom.Connect(r.GetComponent<Room>());
        }

        if (currentRoom.IsOpen(Compass.E))
        {
            Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x + roomSize.x, currentRoom.transform.position.y);
            GameObject r = Instantiate(westRooms[Random.Range(0, westRooms.Length)], neighborPosition, Quaternion.identity, transform);
            roomGrid[currentRoom.roomCoordinate.x + 1, currentRoom.roomCoordinate.y] = r;
            nextRooms.Add(r.GetComponent<Room>());
            //currentRoom.Connect(r.GetComponent<Room>());
        }

        if (currentRoom.IsOpen(Compass.W))
        {
            Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x - roomSize.x, currentRoom.transform.position.y);
            GameObject r = Instantiate(eastRooms[Random.Range(0, eastRooms.Length)], neighborPosition, Quaternion.identity, transform);
            roomGrid[currentRoom.roomCoordinate.x - 1, currentRoom.roomCoordinate.y] = r;
            nextRooms.Add(r.GetComponent<Room>());
            //currentRoom.Connect(r.GetComponent<Room>());
        }

        foreach(Room r in nextRooms)
        {
            currentRoom.Connect(r);
            spawnedRooms.Add(r);
        }
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
