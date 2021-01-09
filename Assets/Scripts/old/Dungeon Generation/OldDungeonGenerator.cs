using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldDungeonGenerator : MonoBehaviour
{
    BSPLeaf root;
    [SerializeField] private int minLeafSize = 75;
    public int MinLeafSize { get => minLeafSize; }
    [SerializeField] private int minEdgeOffset = 3;
    public int MinEdgeOffset { get => minEdgeOffset; }
    //const int MAX_LEAF_SIZE = 50;
    [SerializeField] private Vector2Int dungeonSize;
    
    [HideInInspector] public List<BSPLeaf> leaves = new List<BSPLeaf>();

    public int corridorWidth;

    DCEL dcel;
    void Start()
    {
        dcel = new DCEL();
        CreateRooms();
        
    }
    void CreateRooms()
    {
        root = new BSPLeaf(0, 0, dungeonSize.x, dungeonSize.y, this);
        dcel.outerFace = new DCEL.Face(new Vector2Int(0, 0), dungeonSize,true);
        dcel.rootFace = new DCEL.Face(new Vector2Int(0, 0), dungeonSize);
        DCEL.Face.ConnectInnerOuterFace(dcel.outerFace, dcel.rootFace);

        leaves.Add(root);

        bool didSplit = true;

        while (didSplit)
        {
            didSplit = false;
            for (int i = 0; i < leaves.Count; i++)
            {
                if (leaves[i].leftChild == null && leaves[i].rightChild == null) //if this leaf is not already split
                {
                    if (leaves[i].Split())
                    {
                        //If we did split, add child leaves to list
                        leaves.Add(leaves[i].leftChild);
                        leaves.Add(leaves[i].rightChild);
                        didSplit = true;
                    }
                }
            }
        }
        root.CreateRooms();
        OldGameManager.Instance.SetLeafList(leaves);
    }

    //public void CreateCorridors()
    //{
    //    for (int i = 0; i < leaves.Count; i++)
    //    {
    //        if (leaves[i].leftChild == null && leaves[i].rightChild == null) //if this leaf is at the lowest level
    //        {
                
    //        }
    //    }

    //    // determine in which direction the adjacent room is


    //}
}
