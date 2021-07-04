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
    private GameFSM fsm;
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
                Debug.Log("all keys collected");
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
        fsm = new GameFSM();
        fsm.Initialize(this);
        fsm.AddState(GameStateType.Setup, new SetupState());
        fsm.AddState(GameStateType.Playing, new PlayingState());
        fsm.AddState(GameStateType.Win, new WinState());
        fsm.AddState(GameStateType.Lose, new LoseState());
        DontDestroyOnLoad(this);
        roomGenerator = RoomGenerator.Instance;
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
        MenuManager.Instance.OpenMenu(MenuType.MENU_DEAD);
        StartCoroutine(DelayedRestart());
    }

    void OnPlayerWin()
    {
        MenuManager.Instance.OpenMenu(MenuType.MENU_WIN);
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

    public void RestartLevel() => 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void AddEnemyToList(EnemyType _enemyType, GameObject _instantiatedObject) =>
        enemyDictionary[_enemyType].Add(_instantiatedObject);
}
