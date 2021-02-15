using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Crawler : Enemy
{
    AIDestinationSetter destinationSetter;
    
    private void Start()
    {
        
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = player;
    }
}

