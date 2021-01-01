using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cardinal { None, North, West, South, East }

public class BSPLeaf
{
    public int x, y, width, height;

    public bool hasCorridor;
    public bool hasRoom;
    public Vector2Int roomSize;
    public Vector2Int roomPos;
    private int minLeafSize;
    private int minEdgeOffset;
    private DungeonGenerator owner;

    public BSPLeaf parent;
    public BSPLeaf sibling;
    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public Room room;

    public Dictionary<Cardinal, Corridor> corridors = new Dictionary<Cardinal, Corridor>();

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

    public bool Split(BSPLeaf _parent)
    {
        // do not split if the leaf already has children
        if (leftChild !=null || rightChild != null)
        {
            return false;
        }
        parent = _parent;
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
        leftChild.sibling = rightChild;
        rightChild.sibling = leftChild;
        //Debug.Log("Left child position: " + new Vector2Int(leftChild.x, leftChild.y) + " Right Child position: " + new Vector2Int(rightChild.x, rightChild.y));
        
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

            roomSize = new Vector2Int(Random.Range(width / 2, width - minEdgeOffset * 2), Random.Range(height / 2, height - minEdgeOffset * 2));
            roomPos = new Vector2Int(Random.Range(x + minEdgeOffset, x + (width - roomSize.x - minEdgeOffset)), Random.Range(y + minEdgeOffset, y + (height - roomSize.y - minEdgeOffset)));
            room = new Room(roomPos, roomSize);
            hasRoom = true;
            CreateCorridors(this, sibling);
        }
    }
    public void CreateCorridors(BSPLeaf leafA, BSPLeaf leafB) // call this method at the smallest subdivision
    {
        if (!leafA.hasRoom || !leafB.hasRoom)
        {
            return;
        }
        if (leafB == null)
        {
            return;
        }


        int corridorX=0, corridorY=0, corridorLength=0;
        Vector2Int corridorPos = new Vector2Int(0,0), corridorSize = new Vector2Int(0, 0), exit1 = new Vector2Int(0, 0), exit2 = new Vector2Int(0, 0);
        
        if (leafB.y < leafA.y)
        {
            if (HasCorridor(Cardinal.South) || leafB.HasCorridor(Cardinal.North))
            {
                return;
            }
            // create corridor down
            int Xmin = (leafA.roomPos.x > leafB.roomPos.x ? leafA.roomPos.x : leafB.roomPos.x);
            int Xmax = (leafA.roomPos.x + leafA.roomSize.x > leafB.roomPos.x + leafB.roomSize.x ? leafB.roomPos.x + leafB.roomSize.x - owner.corridorWidth : leafA.roomPos.x + leafA.roomSize.x - owner.corridorWidth);

            corridorX = Random.Range(Xmin, Xmax);
            corridorY = leafB.roomPos.y + leafB.roomSize.y;
            corridorLength = leafA.roomPos.y - (leafB.roomPos.y + leafB.roomSize.y);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(owner.corridorWidth, corridorLength);

            exit1 = new Vector2Int(corridorX + owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + owner.corridorWidth / 2, corridorPos.y + corridorSize.y);
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            leafA.corridors.Add(Cardinal.South, c);
            leafB.corridors.Add(Cardinal.North, c);
            Debug.Log("down");
            parent.CreateCorridors(parent, parent.sibling);
            //return;
        }
        else if (leafB.y > leafA.y)
        {
            if (HasCorridor(Cardinal.North) || leafB.HasCorridor(Cardinal.South))
            {
                return;
            }
            // create corridor up
            int Xmin = (leafA.roomPos.x > leafB.roomPos.x ? leafA.roomPos.x : leafB.roomPos.x);
            int Xmax = (leafA.roomPos.x + leafA.roomSize.x > leafB.roomPos.x + leafB.roomSize.x ? leafB.roomPos.x + leafB.roomSize.x - owner.corridorWidth : leafA.roomPos.x + leafA.roomSize.x - owner.corridorWidth);

            corridorX = Random.Range(Xmin, Xmax);
            corridorY = leafA.roomPos.y + leafA.roomSize.y;
            corridorLength = leafB.roomPos.y - (leafA.roomPos.y + leafA.roomSize.y);
            Debug.Log("this: " + leafA.roomPos + " leafB: " + leafB.roomPos);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(owner.corridorWidth, corridorLength);

            exit1 = new Vector2Int(corridorX + owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + owner.corridorWidth / 2, corridorPos.y + corridorSize.y);
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            corridors.Add(Cardinal.North, c);
            leafB.corridors.Add(Cardinal.South, c);
            Debug.Log("up");
            parent.CreateCorridors(parent, parent.sibling);
            //return;
        }
        else if (leafB.x < x)
        {
            if (HasCorridor(Cardinal.West) || leafB.HasCorridor(Cardinal.East))
            {
                return;
            }
            // create corridor left
            corridorX = leafB.roomPos.x + leafB.roomSize.x;
            int Ymin = (leafA.roomPos.y > leafB.roomPos.y ? leafA.roomPos.y : leafB.roomPos.y);
            int Ymax = (leafA.roomPos.y + leafA.roomSize.y > leafB.roomPos.y + leafB.roomSize.y ? leafB.roomPos.y + leafB.roomSize.y - owner.corridorWidth : leafA.roomPos.y + leafA.roomSize.y - owner.corridorWidth);

            corridorY = Random.Range(Ymin, Ymax);

            corridorLength = leafA.roomPos.x - (leafB.roomPos.x + leafB.roomSize.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int( corridorLength, owner.corridorWidth);

            exit1 = new Vector2Int(corridorPos.x, corridorY + owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + owner.corridorWidth / 2);
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            corridors.Add(Cardinal.West, c);
            leafB.corridors.Add(Cardinal.East, c);
            Debug.Log("left");
            parent.CreateCorridors(parent, parent.sibling);
            //return;
        }
        else if (leafB.x > leafA.x)
        {
            if (HasCorridor(Cardinal.East) || leafB.HasCorridor(Cardinal.West))
            {
                return;
            }
            // create corridor east
            corridorX = leafA.roomPos.x + leafA.roomSize.x;
            int Ymin = (leafA.roomPos.y > leafB.roomPos.y ? leafA.roomPos.y : leafB.roomPos.y);
            int Ymax = (leafA.roomPos.y + leafA.roomSize.y > leafB.roomPos.y + leafB.roomSize.y ? leafB.roomPos.y + leafB.roomSize.y - owner.corridorWidth : leafA.roomPos.y + leafA.roomSize.y - owner.corridorWidth);
            
            corridorY = Random.Range(leafA.roomPos.y, leafA.roomPos.y + leafA.roomSize.y - owner.corridorWidth*2);

            corridorLength = leafB.roomPos.x - (leafA.roomPos.x + leafA.roomSize.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(corridorLength, owner.corridorWidth);

            exit1 = new Vector2Int(corridorPos.x, corridorY + owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + owner.corridorWidth / 2);
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            corridors.Add(Cardinal.East, c);
            leafB.corridors.Add(Cardinal.West, c);
            Debug.Log("right");
            parent.CreateCorridors(parent, parent.sibling);
            //return;
        }

    }

    public bool HasCorridor(Cardinal _direction)
    {
        if (corridors.ContainsKey(_direction))
        {
            return true;
        }
        return false;
    }
}

