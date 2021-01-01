using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    void Start()
    {
        CreateRooms();
    }

    BSPLeaf root;
    [SerializeField] private int minLeafSize = 75;
    public int MinLeafSize { get => minLeafSize; }
    [SerializeField] private int minEdgeOffset = 3;
    public int MinEdgeOffset { get => minEdgeOffset; }
    //const int MAX_LEAF_SIZE = 50;
    [SerializeField] private Vector2Int dungeonSize;
    
    [HideInInspector] public List<BSPLeaf> leaves = new List<BSPLeaf>();

    
    

    void CreateRooms()
    {
        root = new BSPLeaf(0, 0, dungeonSize.x, dungeonSize.y, this);
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
        GameManager.Instance.SetRoomList(leaves);
    }

    class HalfEdge
    {
        public HalfEdge twin, next, prev;
        public Vertex origin;
        public Face incidentFace;

        public HalfEdge(Vertex _origin, Face _incidentFace)
        {
            origin = _origin;
            incidentFace = _incidentFace;
        }
    }

    class Vertex
    {
        public Vector2Int coords;
        public HalfEdge outgoingEdge;
        public Vertex(Vector2Int _coords)
        {
            coords = _coords;
        }
    }

    class Face
    {
        public HalfEdge incidentEdge;
    }
}
