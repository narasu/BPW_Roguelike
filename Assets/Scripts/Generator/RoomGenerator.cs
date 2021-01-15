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

    

    
    private GameObject bossRoom;
    private GameObject[,] roomGrid;
    private List<Room> spawnedRooms = new List<Room>();
    private Room currentRoom;
    private bool roomsSpawned = false;
    private bool RoomsSpawned
    {
        set
        {
            if (!roomsSpawned && value)
            {
                for (int i=0; i<spawnedRooms.Count; i++)
                {
                    Debug.Log(spawnedRooms[i]);
                }
                //FillRooms();


                roomsSpawned = value;

            }
        }
    }

    [Header("Generation parameters")]
    public bool useSeed;
    public int seed;
    [SerializeField] [Range(5, 50)] private int numberOfRooms;
    public Vector2 roomSize;

    [Header("Room Prefabs")]
    [SerializeField] private GameObject startingRoom;
    [SerializeField] private GameObject[] northRooms, southRooms, westRooms, eastRooms;
    [SerializeField] private GameObject[] encountersEasy, encountersMedium;

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
            if (i != 0)
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
                        //spawnedRooms.Add(currentRoom);
                        //spawnedRooms[i] = currentRoom;
                    }
                    else if (currentRoom.origin == Compass.S)
                    {
                        GameObject replacementRoom = Instantiate(southRooms[Random.Range(1, southRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.S, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                        //spawnedRooms[i] = currentRoom;
                    }
                    else if (currentRoom.origin == Compass.W)
                    {
                        GameObject replacementRoom = Instantiate(westRooms[Random.Range(1, westRooms.Length)], currentRoom.transform.position, Quaternion.identity, transform);
                        replacementRoom.GetComponent<Room>().Initialize(Compass.W, currentRoom.roomCoordinate);
                        roomGrid[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = replacementRoom;
                        Destroy(currentRoom.gameObject);
                        yield return new WaitForEndOfFrame();
                        currentRoom = replacementRoom.GetComponent<Room>();
                        //spawnedRooms[i] = currentRoom;
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
            //spawnedRooms[i] = currentRoom;

            List<Room> nextRooms = new List<Room>();

            //check at each opening if there is space available and spawn a new room there

            if (currentRoom.IsOpen(Compass.N) && !IsOccupied(currentRoom.roomCoordinate + Vector2Int.up))
            {
                // neighbor transform
                Vector2 neighborPosition = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y + roomSize.y);

                //// create a reference to a random GameObject in the corresponding array
                //GameObject temp = southRooms[Random.Range(1, southRooms.Length)];

                //for(int j=0; j<temp.GetComponent<Room>().openingDirections.Count; j++)
                //{
                //    Room tempRoom = temp.GetComponent<Room>();
                    
                //    if (tempRoom.openingDirections[j] == Compass.S)
                //    {
                //        continue;
                //    }

                //    //adjacent coords of the intended position of new room
                //    Dictionary<Compass, Vector2Int> possibleCoords = GetAdjacentCoords(new Vector2Int(currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y + 1));
                    
                //    //remove all coords that are occupied
                //    foreach (KeyValuePair<Compass, Vector2Int> kvp in possibleCoords)
                //    {
                //        if (IsOccupied(kvp.Value))
                //        {
                //            possibleCoords.Remove(kvp.Key);
                //        }
                //    }

                //    if (possibleCoords.ContainsKey(tempRoom.openingDirections[j]))
                //    {
                        
                //    }
                //}

                //while(temp.GetComponent<Room>().openingDirections.C)

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
                
                //while (IsOccupied(newRoom.adjacentCoords[newRoom.destination]))
                //{

                //}    

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


        for (int j = 0; j < spawnedRooms.Count; j++)
        {
            Debug.Log(spawnedRooms[j]);
        }
        //Debug.Log(spawnedRooms.Count);
        yield return new WaitForEndOfFrame();
        //StartCoroutine("CloseRooms");
        //RoomsSpawned = true;
    }

    private IEnumerator CloseRooms()
    {
        for(int i=0; i<spawnedRooms.Count; i++)
        {
            if(spawnedRooms[i]==null)
            {
                
                //spawnedRooms.RemoveAt(i);
                continue;
            }
            foreach (KeyValuePair<Compass, Room> kvp in spawnedRooms[i].neighbors)
            {
                Debug.Log(kvp.Key);
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
        yield return new WaitForEndOfFrame();

        RoomsSpawned = true;
    }

    private void FillRooms()
    {


        //foreach (Room room in spawnedRooms)
        //{

        //    Debug.Log(room + "" + room.IsConnected());
        //}

        //Debug.Log("Distance: " + spawnedRooms[spawnedRooms.Count - 1].CountDistanceFromStart());

        for (int i=1; i<spawnedRooms.Count; i++)
        {
            //if (spawnedRooms[i] == null)
            //    continue;

            Vector3 rotation = new Vector3(0, 0, 0);

            if (spawnedRooms[i].origin == Compass.E)
            {
                rotation.z = 0;
            }
            if (spawnedRooms[i].origin == Compass.S)
            {
                rotation.z = -90;
            }
            if (spawnedRooms[i].origin == Compass.W)
            {
                rotation.z = 180;
            }
            if (spawnedRooms[i].origin == Compass.N)
            {
                rotation.z = 90;
            }
            Debug.Log(spawnedRooms[i]);
            Instantiate(encountersEasy[Random.Range(0, encountersEasy.Length)],spawnedRooms[i].transform.position, Quaternion.Euler(rotation));
        }
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
}