using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPLeaf
{
    public int x, y, width, height;

    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public bool hasRoom;
    public Vector2 roomSize;
    public Vector2 roomPos;
    private int minLeafSize;
    private int minEdgeOffset;
    private DungeonGenerator owner;

    public BSPLeaf(int _x, int _y, int _width, int _height, DungeonGenerator _owner)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        owner = _owner;
        minLeafSize = owner.MinLeafSize;
        minEdgeOffset = owner.MinEdgeOffset;
        //GameManager.Instance.leafList.Add(this);
    }

    public bool Split()
    {
        // do not split if the leaf already has children
        if (leftChild !=null || rightChild != null)
        {
            return false;
        }

        // randomize split direction
        bool splitH = Random.Range(0f, 1f) > 0.5f;

        // If the room is wider than it is tall, split it vertically instead
        if (width > height && height / width >= 0.5f)
        {
            splitH = false;
        }
        else if (height > width && width / height >= 0.5f)
        {
            splitH = true;
        }

        // Determine the max size of the new leaf
        int max = (splitH ? height : width) - minLeafSize;

        // If it's too small, don't split
        if (max <= minLeafSize)
        {
            return false;
        }

        // Generate a new dimension for the split
        int split = Random.Range(minLeafSize, max);

        // Do the split
        if (splitH)
        {
            leftChild = new BSPLeaf(x, y, width, split, owner);
            rightChild = new BSPLeaf(x, y + split, width, height-split, owner);
        }
        if (!splitH)
        {
            leftChild = new BSPLeaf(x, y, split, height, owner);
            rightChild = new BSPLeaf(x + split, y, width - split, height, owner);
        }

        return true;
    }

    public void CreateRooms()
    {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null)
            {
                leftChild.CreateRooms();
            }
            if (rightChild != null)
            {
                rightChild.CreateRooms();
            }
            hasRoom = false;
        }
        else
        {

            roomSize = new Vector2(Random.Range(width / 2, width - minEdgeOffset * 2), Random.Range(height / 2, height - minEdgeOffset * 2));
            roomPos = new Vector2(Random.Range(x + minEdgeOffset, x + (width - roomSize.x - minEdgeOffset)), Random.Range(y + minEdgeOffset, y + (height - roomSize.y - minEdgeOffset)));
            //roomPos = new Vector2(x, y);
            hasRoom = true;
        }

        //Debug.Log(leftChild);
        //Debug.Log(rightChild);
        //Debug.Log(hasRoom);
    }

    
}
