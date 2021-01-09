using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGameManager : MonoBehaviour
{
    private static OldGameManager instance;
    public static OldGameManager Instance
    {
        get { return instance; }
    }

    private List<BSPLeaf> leafList = new List<BSPLeaf>();
    private List<OldCorridor> corridorList = new List<OldCorridor>();
    private List<OldRoom2> roomList = new List<OldRoom2>();

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        foreach (BSPLeaf leaf in leafList)
        {
            

            // leaf corners
            Vector2 leafTopLeft = new Vector2(leaf.x, leaf.y);
            Vector2 leafTopRight = new Vector2(leaf.x + leaf.width, leaf.y);
            Vector2 leafBottomLeft = new Vector2(leaf.x, leaf.y + leaf.height);
            Vector2 leafBottomRight = new Vector2(leaf.x + leaf.width, leaf.y + leaf.height);

            

            Debug.DrawLine(leafTopLeft, leafTopRight, Color.gray);
            Debug.DrawLine(leafTopRight, leafBottomRight, Color.gray);
            Debug.DrawLine(leafTopLeft, leafBottomLeft, Color.gray);
            Debug.DrawLine(leafBottomLeft, leafBottomRight, Color.gray);

            
        }

        foreach(OldRoom2 room in roomList)
        {
            //room corners
            Vector2 roomBottomLeft = room.position;
            Vector2 roomBottomRight = new Vector2(room.position.x + room.size.x, room.position.y);
            Vector2 roomTopLeft = new Vector2(room.position.x, room.position.y + room.size.y);
            Vector2 roomTopRight = room.position + room.size;

            Debug.DrawLine(roomTopLeft, roomTopRight);
            Debug.DrawLine(roomTopRight, roomBottomRight);
            Debug.DrawLine(roomTopLeft, roomBottomLeft);
            Debug.DrawLine(roomBottomLeft, roomBottomRight);
        }

        foreach (OldCorridor c in corridorList)
        {
            //room corners
            Vector2 corridorTopLeft = c.position;
            Vector2 corridorTopRight = new Vector2(c.position.x + c.size.x, c.position.y);
            Vector2 corridorBottomLeft = new Vector2(c.position.x, c.position.y + c.size.y);
            Vector2 corridorBottomRight = c.position + c.size;


            Debug.DrawLine(corridorTopLeft, corridorTopRight);
            Debug.DrawLine(corridorTopRight, corridorBottomRight);
            Debug.DrawLine(corridorTopLeft, corridorBottomLeft);
            Debug.DrawLine(corridorBottomLeft, corridorBottomRight);


        }
    }

    public void SetLeafList(List<BSPLeaf> _leaves) => leafList = _leaves;
    public void AddCorridor(OldCorridor _corridor) => corridorList.Add(_corridor);
    public void AddRoom(OldRoom2 _room) => roomList.Add(_room);
}
