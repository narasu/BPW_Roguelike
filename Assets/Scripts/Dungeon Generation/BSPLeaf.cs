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
            CreateCorridors(this);
        }
    }
    public static void CreateCorridors(BSPLeaf _leaf) // call this method at the smallest subdivision
    {
        if (_leaf.sibling == null)
        {
            return;
        }

        BSPLeaf[] twinLeaves = { _leaf, _leaf.sibling };
        Room[] twinRooms = new Room[2];

        for (int i=0; i < twinLeaves.Length; i++)
        {
            if (twinLeaves[i].room==null)
            {
                return;
                //int n = Mathf.RoundToInt(Random.Range(0.0f, 1.0f));
                
                //if (twinLeaves[i].leftChild!=null)
                //    twinLeaves[i] = twinLeaves[i].leftChild;
                //else if(twinLeaves[i].rightChild != null)
                //    twinLeaves[i] = twinLeaves[i].rightChild;
            }
        }


        /*
        if (roomA == null || roomB == null)
        {
            // if the child of either leaf has a room
            // then assign the child's room to roomA / roomB
            // ruling out any room that has an unfavorable position / size
            // else return
            return;
        }*/



        int corridorX=0, corridorY=0, corridorLength=0;
        Vector2Int corridorPos = new Vector2Int(0,0), corridorSize = new Vector2Int(0, 0), exit1 = new Vector2Int(0, 0), exit2 = new Vector2Int(0, 0);
        
        if (_leaf.sibling.y < _leaf.y)
        {
            //return if this side already has a corridor
            if (_leaf.HasCorridor(Cardinal.South) || _leaf.sibling.HasCorridor(Cardinal.North))
            {
                return;
            }
            // make sure the corridor stays within the edges of the two rooms
            int Xmin = (_leaf.roomPos.x > _leaf.sibling.roomPos.x ? _leaf.roomPos.x : _leaf.sibling.roomPos.x);
            int Xmax = (_leaf.roomPos.x + _leaf.roomSize.x > _leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x ? _leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x - _leaf.owner.corridorWidth : _leaf.roomPos.x + _leaf.roomSize.x - _leaf.owner.corridorWidth);
            
            //set corridor position and size
            corridorX = Random.Range(Xmin, Xmax);
            corridorY = _leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y;
            corridorLength = _leaf.roomPos.y - (_leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(_leaf.owner.corridorWidth, corridorLength);

            //create exits at both ends
            exit1 = new Vector2Int(corridorX + _leaf.owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + _leaf.owner.corridorWidth / 2, corridorPos.y + corridorSize.y);
            
            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            _leaf.corridors.Add(Cardinal.South, c);
            _leaf.sibling.corridors.Add(Cardinal.North, c);
            Debug.Log("down");

            //move one level up in subdivision tree
            //CreateCorridors(_leaf.parent);
            //return;
        }
        else if (_leaf.sibling.y > _leaf.y)
        {
            //return if this side already has a corridor
            if (_leaf.HasCorridor(Cardinal.North) || _leaf.sibling.HasCorridor(Cardinal.South))
            {
                return;
            }
            // make sure the corridor stays within the edges of the two rooms
            int Xmin = (_leaf.roomPos.x > _leaf.sibling.roomPos.x ? _leaf.roomPos.x : _leaf.sibling.roomPos.x);
            int Xmax = (_leaf.roomPos.x + _leaf.roomSize.x > _leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x ? _leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x - _leaf.owner.corridorWidth : _leaf.roomPos.x + _leaf.roomSize.x - _leaf.owner.corridorWidth);

            //set corridor position and size
            corridorX = Random.Range(Xmin, Xmax);
            corridorY = _leaf.roomPos.y + _leaf.roomSize.y;
            corridorLength = _leaf.sibling.roomPos.y - (_leaf.roomPos.y + _leaf.roomSize.y);
            Debug.Log("this: " + _leaf.roomPos + " leafB: " + _leaf.sibling.roomPos);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(_leaf.owner.corridorWidth, corridorLength);

            //create exits at both ends
            exit1 = new Vector2Int(corridorX + _leaf.owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + _leaf.owner.corridorWidth / 2, corridorPos.y + corridorSize.y);

            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            _leaf.corridors.Add(Cardinal.North, c);
            _leaf.sibling.corridors.Add(Cardinal.South, c);
            Debug.Log("up");

            //move one level up in subdivision tree
            //CreateCorridors(_leaf.parent);
            //return;
        }
        else if (_leaf.sibling.x < _leaf.x)
        {
            //return if this side already has a corridor
            if (_leaf.HasCorridor(Cardinal.West) || _leaf.sibling.HasCorridor(Cardinal.East))
            {
                return;
            }
            // make sure the corridor stays within the edges of the two rooms
            int Ymin = (_leaf.roomPos.y > _leaf.sibling.roomPos.y ? _leaf.roomPos.y : _leaf.sibling.roomPos.y);
            int Ymax = (_leaf.roomPos.y + _leaf.roomSize.y > _leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y ? _leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y - _leaf.owner.corridorWidth : _leaf.roomPos.y + _leaf.roomSize.y - _leaf.owner.corridorWidth);
            
            //set corridor position and size
            corridorX = _leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x;
            corridorY = Random.Range(Ymin, Ymax);
            corridorLength = _leaf.roomPos.x - (_leaf.sibling.roomPos.x + _leaf.sibling.roomSize.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int( corridorLength, _leaf.owner.corridorWidth);

            // create exits
            exit1 = new Vector2Int(corridorPos.x, corridorY + _leaf.owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + _leaf.owner.corridorWidth / 2);

            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            _leaf.corridors.Add(Cardinal.West, c);
            _leaf.sibling.corridors.Add(Cardinal.East, c);
            Debug.Log("left");

            //move one level up in the subdivision tree
            //CreateCorridors(_leaf.parent);
            //return;
        }
        else if (_leaf.sibling.x > _leaf.x)
        {
            //return if this side already has a corridor
            if (_leaf.HasCorridor(Cardinal.East) || _leaf.sibling.HasCorridor(Cardinal.West))
            {
                return;
            }
            // make sure the corridor stays within the edges of the two rooms
            
            int Ymin = (_leaf.roomPos.y > _leaf.sibling.roomPos.y ? _leaf.roomPos.y : _leaf.sibling.roomPos.y);
            int Ymax = (_leaf.roomPos.y + _leaf.roomSize.y > _leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y ? _leaf.sibling.roomPos.y + _leaf.sibling.roomSize.y - _leaf.owner.corridorWidth : _leaf.roomPos.y + _leaf.roomSize.y - _leaf.owner.corridorWidth);

            //set corridor position and size
            corridorX = _leaf.roomPos.x + _leaf.roomSize.x;
            corridorY = Random.Range(Ymin, Ymax);
            corridorLength = _leaf.sibling.roomPos.x - (_leaf.roomPos.x + _leaf.roomSize.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(corridorLength, _leaf.owner.corridorWidth);

            //create exits at the two ends
            exit1 = new Vector2Int(corridorPos.x, corridorY + _leaf.owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + _leaf.owner.corridorWidth / 2);

            //create corridor with above parameters and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            _leaf.corridors.Add(Cardinal.East, c);
            _leaf.sibling.corridors.Add(Cardinal.West, c);
            Debug.Log("right");

            //move one level up in the subdivision tree
            //CreateCorridors(_leaf.parent);
            //return;
        }

    }

    public bool HasCorridor()
    {
        if (corridors.Count>0)
        {
            return true;
        }
        return false;
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

