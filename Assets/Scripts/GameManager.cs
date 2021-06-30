using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private EnemySpawner enemySpawner;

    private Dictionary<EnemyType, List<GameObject>> enemyDictionary;

    private bool keysCollected = false;
    private bool KeysCollected
    {
        set
        {
            if (!keysCollected && value)
            {
                //Destroy(door);
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
        roomGenerator = RoomGenerator.Instance;
        enemyDictionary = new Dictionary<EnemyType, List<GameObject>>()
        {
            { EnemyType.Crawler, new List<GameObject>() },
            { EnemyType.Turret, new List<GameObject>() }
        };

        EventManager.AddListener(EventType.PLAYER_DIED, RestartLevel);
    }

    void OnKeyCollected()
    {
        // play sound

    }

    public void ClearEnemies()
    {

    }

    public void RestartLevel() => 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    public void WinGame()
    {

    }

    public void AddEnemyToList(EnemyType _enemyType, GameObject _instantiatedObject) =>
        enemyDictionary[_enemyType].Add(_instantiatedObject);
}
