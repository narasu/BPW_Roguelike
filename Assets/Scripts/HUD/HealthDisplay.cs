using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    private Stack<GameObject> hearts;

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

    void Start()
    {
        
        hearts = new Stack<GameObject>();
        for (int i=0; i<Player.pInstance.pHealth; i++)
        {
            AddHeart();
        }
    }

    private void OnEnable()
    {
        EventManager.AddListener(EventType.PLAYER_HIT, RemoveHeart);
        EventManager.AddListener(EventType.PLAYER_HEALED, AddHeart);
    }
    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.PLAYER_HIT, RemoveHeart);
        EventManager.RemoveListener(EventType.PLAYER_HEALED, AddHeart);
    }

    
    
}
