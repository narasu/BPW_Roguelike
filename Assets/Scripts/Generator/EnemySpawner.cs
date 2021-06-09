using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Turret, Crawler, SomeOther }

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject crawlerPrefab;

    private Dictionary<EnemyType, GameObject> enemyPrefabs;

    private void Awake()
    {
        enemyPrefabs = new Dictionary<EnemyType, GameObject>()
        {
            { EnemyType.Turret, turretPrefab },
            { EnemyType.Crawler, crawlerPrefab }
        };
    }

    public void SpawnEnemy(EnemyType _enemyType, Vector3 _position)
    {
        GameObject o = Instantiate(enemyPrefabs[_enemyType], _position, Quaternion.identity);
        GameManager.Instance.AddEnemyToList(_enemyType, o);
    }
}
