﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private List<BSPLeaf> roomList = new List<BSPLeaf>();

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        foreach (BSPLeaf leaf in roomList)
        {
            //room corners
            Vector2 roomTopLeft = leaf.roomPos;
            Vector2 roomTopRight = new Vector2(leaf.roomPos.x + leaf.roomSize.x, leaf.roomPos.y);
            Vector2 roomBottomLeft = new Vector2(leaf.roomPos.x, leaf.roomPos.y + leaf.roomSize.y);
            Vector2 roomBottomRight = leaf.roomPos + leaf.roomSize;

            // leaf corners
            Vector2 leafTopLeft = new Vector2(leaf.x, leaf.y);
            Vector2 leafTopRight = new Vector2(leaf.x + leaf.width, leaf.y);
            Vector2 leafBottomLeft = new Vector2(leaf.x, leaf.y + leaf.height);
            Vector2 leafBottomRight = new Vector2(leaf.x + leaf.width, leaf.y + leaf.height);

            Debug.DrawLine(roomTopLeft, roomTopRight);
            Debug.DrawLine(roomTopRight, roomBottomRight);
            Debug.DrawLine(roomTopLeft, roomBottomLeft);
            Debug.DrawLine(roomBottomLeft, roomBottomRight);

            Debug.DrawLine(leafTopLeft, leafTopRight, Color.gray);
            Debug.DrawLine(leafTopRight, leafBottomRight, Color.gray);
            Debug.DrawLine(leafTopLeft, leafBottomLeft, Color.gray);
            Debug.DrawLine(leafBottomLeft, leafBottomRight, Color.gray);


        }
    }

    public void SetRoomList(List<BSPLeaf> _leaves) => roomList = _leaves;
}
