using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private static RoomGenerator instance;
    public static RoomGenerator Instance
    {
        get { return instance; }
    }

    [SerializeField] [Range(1, 25)] private int numberOfRooms;

    private OldRoom[,] rooms;
    private List<OldRoom> roomList;

    private OldRoom startRoom;
    private OldRoom endRoom;
    private OldRoom bossRoom;

    private void Awake()
    {
        instance = this;

        startRoom = GenerateRooms();
    }

    private void Start()
    {
        //GenerateRooms();
        PrintGrid();
    }

    private OldRoom GenerateRooms()
    {
        int gridSize = numberOfRooms;
        rooms = new OldRoom[gridSize, gridSize];

        //create the first room on the middle coordinate in the grid
        Vector2Int firstCoordinate = new Vector2Int((gridSize / 2) - 1, (gridSize / 2) - 1);
        
        //queue new rooms
        Queue<OldRoom> roomsToCreate = new Queue<OldRoom>();
        roomsToCreate.Enqueue(new OldRoom(firstCoordinate.x, firstCoordinate.y));
        
        //add created rooms to list
        List<OldRoom> createdRooms = new List<OldRoom>();
        while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms)
        {
            OldRoom currentRoom = roomsToCreate.Dequeue();
            rooms[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbors(currentRoom,roomsToCreate);
        }

        foreach (OldRoom room in createdRooms)
        {
            List<Vector2Int> neighborCoordinates = room.NeighborCoordinates();
            foreach(Vector2Int coordinate in neighborCoordinates)
            {
                OldRoom neighbor = rooms[coordinate.x, coordinate.y];
                if (neighbor!=null)
                {
                    room.Connect(neighbor);
                }
            }
        }

        return rooms[firstCoordinate.x, firstCoordinate.y];
    }

    private void AddNeighbors(OldRoom currentRoom, Queue<OldRoom> roomsToCreate)
    {
        List<Vector2Int> neighborCoordinates = currentRoom.NeighborCoordinates();
        List<Vector2Int> availableNeighbors = new List<Vector2Int>();

        foreach (Vector2Int coordinate in neighborCoordinates)
        {
            if (rooms[coordinate.x, coordinate.y]==null)
            {
                availableNeighbors.Add(coordinate);
            }
        }

        int numberOfNeighbors = Random.Range(1, availableNeighbors.Count);

        for (int i=0; i<numberOfNeighbors; i++)
        {
            float randomNumber = Random.value;
            float roomFrac = 1f / availableNeighbors.Count;

            Vector2Int chosenNeighbor = new Vector2Int(0, 0);
            foreach (Vector2Int coordinate in availableNeighbors)
            {
                if (randomNumber < roomFrac)
                {
                    chosenNeighbor = coordinate;
                    break;
                }
                else
                {
                    roomFrac += 1f / availableNeighbors.Count;
                }
            }
            roomsToCreate.Enqueue(new OldRoom(chosenNeighbor));
            availableNeighbors.Remove(chosenNeighbor);
        }
    }


    private void PrintGrid()
    {
        for (int i = 0; i < rooms.GetLength(1); i++)
        {
            string row = "";
            for (int j = 0; j < rooms.GetLength(0); j++)
            {
                if (rooms[j,i] == null)
                {
                    row += "X";
                }
                else
                {
                    row += "R";
                }
            }
            Debug.Log(row);
        }
    }
}
