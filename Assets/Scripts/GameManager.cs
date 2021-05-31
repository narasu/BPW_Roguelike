using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    RoomGenerator roomGenerator;
    [SerializeField] private GameObject keyPrefab;
    public List<Key> keysInLevel = new List<Key>();
    public GameObject door;

    private bool keysCollected = false;
    private bool KeysCollected
    {
        set
        {
            if (!keysCollected && value)
            {
                Destroy(door);
                keysCollected = true;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        roomGenerator = RoomGenerator.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnKeys()
    {

        // the room at index 0 will be used as end room
        for (int i = 1; i < roomGenerator.closedRooms.Count; i++)
        {
            GameObject placedKey = roomGenerator.closedRooms[i].PlaceObject(keyPrefab, Vector3.zero);
            keysInLevel.Add(placedKey.GetComponent<Key>());
        }
    }

    void OnKeyCollected()
    {
        // play sound

    }

    //private void SpawnEncounters()
    //{
    //    for (int i = 1; i < spawnedRooms.Count; i++)
    //    {
    //        if (closedRooms.Contains(spawnedRooms[i]))
    //        {
    //            continue;
    //        }
    //        //if (spawnedRooms[i] == null)
    //        //    continue;

    //        Vector3 rotation = new Vector3(0, 0, 0);

    //        if (spawnedRooms[i].origin == Compass.E)
    //        {
    //            rotation.z = 0;
    //        }
    //        if (spawnedRooms[i].origin == Compass.S)
    //        {
    //            rotation.z = -90;
    //        }
    //        if (spawnedRooms[i].origin == Compass.W)
    //        {
    //            rotation.z = 180;
    //        }
    //        if (spawnedRooms[i].origin == Compass.N)
    //        {
    //            rotation.z = 90;
    //        }
    //        Instantiate(encounters[Random.Range(0, encounters.Length)], spawnedRooms[i].transform.position, Quaternion.Euler(rotation));
    //    }

    //}
}
