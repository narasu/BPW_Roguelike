using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Turret, Crawler, SomeOther }

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject crawlerPrefab;

    private Dictionary<EnemyType, GameObject> enemyPrefabs;
    private int maxEnemiesPerRoom = 5;

    private void Awake()
    {
        enemyPrefabs = new Dictionary<EnemyType, GameObject>()
        {
            { EnemyType.Turret, turretPrefab },
            { EnemyType.Crawler, crawlerPrefab }
        };
    }

    public void SpawnEnemies(EnemyType _enemyType, Room _room)
    {
        List<SpawnPoint> emptyPoints = _room.GetEmptySpawnPoints();

        for (int i = 0; i < maxEnemiesPerRoom; i++)
        {
            if (emptyPoints.Count == 0)
            {
                break;
            }
            int j = Random.Range(0, emptyPoints.Count);
            Transform t = emptyPoints[j].transform;
            
            GameObject o = Instantiate(enemyPrefabs[_enemyType], t.position, Quaternion.identity);
            o.GetComponent<Enemy>()?.Initialize(emptyPoints[j]);
            emptyPoints[j].Empty = false;
            emptyPoints.RemoveAt(j);
            GameManager.Instance.AddEnemyToList(_enemyType, o);
        }
    }
}
