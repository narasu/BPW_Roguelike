using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    private Stack<GameObject> hearts;
    
    void Start()
    {
        EventManager.AddListener(EventType.PLAYER_HIT, RemoveHeart);
        EventManager.AddListener(EventType.PLAYER_HEALED, AddHeart);
        hearts = new Stack<GameObject>();
        for (int i=0; i<Player.Instance.Health; i++)
        {
            AddHeart();
        }
    }

    public void AddHeart()
    {
        hearts.Push(Instantiate(heartPrefab, transform));
    }
    public void RemoveHeart()
    {
        if (hearts.Count > 0)
        {
            Destroy(hearts.Pop());
        }
    }
}
