using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Key> keysInLevel = new List<Key>();
    public GameObject door;
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
    private FSM<GameManager> fsm;
    RoomGenerator roomGenerator;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private EnemySpawner enemySpawner;
    private Dictionary<EnemyType, List<GameObject>> enemyDictionary;
    private bool keysCollected = false;
    private bool KeysCollected
    {
        set
        {
            if (!keysCollected && value)
            {
                Debug.Log("all keys collected");
                Destroy(door);
                keysCollected = true;
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddEnemyToList(EnemyType _enemyType, GameObject _instantiatedObject) 
    { 
        enemyDictionary[_enemyType].Add(_instantiatedObject);
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
        fsm = new FSM<GameManager>();
        fsm.Initialize(this);
        fsm.AddState(new SetupState(fsm));
        fsm.AddState(new PlayingState(fsm));
        fsm.AddState(new WinState(fsm));
        fsm.AddState(new LoseState(fsm));
        DontDestroyOnLoad(this);
        roomGenerator = RoomGenerator.pInstance;
        enemyDictionary = new Dictionary<EnemyType, List<GameObject>>()
        {
            { EnemyType.Crawler, new List<GameObject>() },
            { EnemyType.Turret, new List<GameObject>() }
        };

        EventManager.AddListener(EventType.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(EventType.PLAYER_WIN, OnPlayerWin);
        EventManager.AddListener(EventType.KEY_COLLECTED, OnKeyCollected);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnKeyCollected()
    {
        if (keysInLevel.Count == 0)
        {
            Destroy(door);
        }
    }

    void OnPlayerDied()
    {
        MenuManager.pInstance.OpenMenu(MenuType.MENU_DEAD);
        StartCoroutine(DelayedRestart());
    }

    void OnPlayerWin()
    {
        MenuManager.pInstance.OpenMenu(MenuType.MENU_WIN);
        Debug.Log(this);
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(1.0f);
        RestartLevel();
    }

    public void ClearEnemies()
    {

    }

    
}
