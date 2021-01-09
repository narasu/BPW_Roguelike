using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BSPLeaf
{
    public int x, y, width, height;
    private Vector2Int position;
    public bool hasCorridor;
    public bool hasRoom;
    
    private int minLeafSize;
    private int minEdgeOffset;
    private DungeonGenerator owner;

    public BSPLeaf parent;
    public BSPLeaf sibling;
    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public Room room;

    public List<Corridor> corridors = new List<Corridor>();

    public BSPLeaf(int _x, int _y, int _width, int _height, DungeonGenerator _owner)
    {
        x = _x;
        y = _y;
        position = new Vector2Int(x, y);
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
        leftChild.parent = this;
        rightChild.parent = this;
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

            Vector2Int roomSize = new Vector2Int(Random.Range(width / 2, width - minEdgeOffset * 2), Random.Range(height / 2, height - minEdgeOffset * 2));
            Vector2Int roomPos = new Vector2Int(Random.Range(x + minEdgeOffset, x + (width - roomSize.x - minEdgeOffset)), Random.Range(y + minEdgeOffset, y + (height - roomSize.y - minEdgeOffset)));
            room = new Room(roomPos, roomSize, sibling.room);
            hasRoom = true;
            CreateCorridors(this, sibling);
        }
    }
    public static void CreateCorridors(BSPLeaf _leafA, BSPLeaf _leafB) // call this method at the smallest subdivision
    {
        //if there is no parent then we're at root level, so return
        if (_leafA.parent==null)
        {
            return;
        }
        // if either leaf is null, return
        if (_leafA == null || _leafB == null)
        {
            return;
        }
        
        
        BSPLeaf[] twinLeaves = { _leafA, _leafB };
        Room[] twinRooms = new Room[2];

        for (int i = 0; i < twinLeaves.Length; i++)
        {
            twinRooms[i] = twinLeaves[i].room;
            // if one of the two leaves has no room
            if (twinLeaves[i].room == null)
            {
                //return;

                if (twinLeaves[i].leftChild == null || twinLeaves[i].rightChild == null)
                {
                    return;
                }

                // if one of the children has no room (i.e. has children of their own) return, since they'll be handled by another call
                if (twinLeaves[i].leftChild.room == null || twinLeaves[i].rightChild.room == null)
                {
                    return;
                }
                //if children are connected, return
                //if (RoomsAreConnected(twinLeaves[i].leftChild, twinLeaves[i].rightChild))
                //{
                //    return;
                //}



                int n = Mathf.RoundToInt(Random.Range(0.0f, 1.0f));
                if (n == 0)
                {
                    twinRooms[i] = twinLeaves[i].leftChild.room;
                    //twinLeaves[i] = twinLeaves[i].leftChild;
                    Debug.Log(twinLeaves[i].leftChild);
                }
                else
                {
                    twinRooms[i] = twinLeaves[i].rightChild.room;
                    //twinLeaves[i] = twinLeaves[i].rightChild;
                    Debug.Log(twinLeaves[i].rightChild);
                }

                // the smallest level of subdivision is connected now
                //right
                //so this if condition is met when we step up into a parent
                // here we want to check
                // if the child of either leaf has a room

                // then assign the child's room to roomA / roomB
                // ruling out any room that has an unfavorable position / size
                //return;


            }
            //twinRooms[i] = twinLeaves[i].room;
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
        
        if (twinLeaves[1].y < twinLeaves[0].y)
        {
            // make sure the corridor stays within the edges of the two rooms
            int Xmin = (twinRooms[0].position.x > twinRooms[1].position.x ? twinRooms[0].position.x : twinRooms[1].position.x);
            int Xmax = (twinRooms[0].position.x + twinRooms[0].size.x > twinRooms[1].position.x + twinRooms[1].size.x ? twinRooms[1].position.x + twinRooms[1].size.x - twinLeaves[0].owner.corridorWidth : twinRooms[0].position.x + twinRooms[0].size.x - twinLeaves[0].owner.corridorWidth);
            
            //set corridor position and size
            corridorX = Random.Range(Xmin, Xmax);
            corridorY = twinRooms[1].position.y + twinRooms[1].size.y;
            corridorLength = twinRooms[0].position.y - (twinRooms[1].position.y + twinRooms[1].size.y);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(twinLeaves[0].owner.corridorWidth, corridorLength);

            //create exits at both ends
            exit1 = new Vector2Int(corridorX + twinLeaves[0].owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + twinLeaves[0].owner.corridorWidth / 2, corridorPos.y + corridorSize.y);
            
            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            _leafA.parent.corridors.Add(c);
            //Debug.Log("down");

            //move one level up in subdivision tree
            Debug.Log(_leafA.parent);
            CreateCorridors(_leafA.parent, _leafA.parent.sibling);
            
            //return;
        }
        else if (twinLeaves[1].y > twinLeaves[0].y)
        {
            
            // make sure the corridor stays within the edges of the two rooms
            int Xmin = (twinRooms[0].position.x > twinRooms[1].position.x ? twinRooms[0].position.x : twinRooms[1].position.x);
            int Xmax = (twinRooms[0].position.x + twinRooms[0].size.x > twinRooms[1].position.x + twinRooms[1].size.x ? twinRooms[1].position.x + twinRooms[1].size.x - twinLeaves[0].owner.corridorWidth : twinRooms[0].position.x + twinRooms[0].size.x - twinLeaves[0].owner.corridorWidth);

            //set corridor position and size
            corridorX = Random.Range(Xmin, Xmax);
            corridorY = twinRooms[0].position.y + twinRooms[0].size.y;
            corridorLength = twinRooms[1].position.y - (twinRooms[0].position.y + twinRooms[0].size.y);
            //Debug.Log("this: " + twinRooms[0].position + " leafB: " + twinRooms[1].position);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(twinLeaves[0].owner.corridorWidth, corridorLength);

            //create exits at both ends
            exit1 = new Vector2Int(corridorX + twinLeaves[0].owner.corridorWidth / 2, corridorPos.y);
            exit2 = new Vector2Int(corridorX + twinLeaves[0].owner.corridorWidth / 2, corridorPos.y + corridorSize.y);

            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            //twinLeaves[0].corridors.Add(c);
            //twinLeaves[1].corridors.Add(c);
            _leafA.parent.corridors.Add(c);
            //Debug.Log("up");

            //move one level up in subdivision tree
            Debug.Log(_leafA.parent);
            CreateCorridors(_leafA.parent, _leafA.parent.sibling);
            //return;
        }
        else if (twinLeaves[1].x < twinLeaves[0].x)
        {
            // make sure the corridor stays within the edges of the two rooms
            int Ymin = (twinRooms[0].position.y > twinRooms[1].position.y ? twinRooms[0].position.y : twinRooms[1].position.y);
            int Ymax = (twinRooms[0].position.y + twinRooms[0].size.y > twinRooms[1].position.y + twinRooms[1].size.y ? twinRooms[1].position.y + twinRooms[1].size.y - twinLeaves[0].owner.corridorWidth : twinRooms[0].position.y + twinRooms[0].size.y - twinLeaves[0].owner.corridorWidth);
            
            //set corridor position and size
            corridorX = twinRooms[1].position.x + twinRooms[1].size.x;
            corridorY = Random.Range(Ymin, Ymax);
            corridorLength = twinRooms[0].position.x - (twinRooms[1].position.x + twinRooms[1].size.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int( corridorLength, twinLeaves[0].owner.corridorWidth);

            // create exits
            exit1 = new Vector2Int(corridorPos.x, corridorY + twinLeaves[0].owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + twinLeaves[0].owner.corridorWidth / 2);

            //create corridor and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            //twinLeaves[0].corridors.Add(c);
            //twinLeaves[1].corridors.Add(c);
            _leafA.parent.corridors.Add(c);
            //Debug.Log("left");

            //move one level up in the subdivision tree
            Debug.Log(_leafA.parent);
            CreateCorridors(_leafA.parent, _leafA.parent.sibling);
            //return;
        }
        else if (twinLeaves[1].x > twinLeaves[0].x)
        {
            // make sure the corridor stays within the edges of the two rooms
            
            int Ymin = (twinRooms[0].position.y > twinRooms[1].position.y ? twinRooms[0].position.y : twinRooms[1].position.y);
            int Ymax = (twinRooms[0].position.y + twinRooms[0].size.y > twinRooms[1].position.y + twinRooms[1].size.y ? twinRooms[1].position.y + twinRooms[1].size.y - twinLeaves[0].owner.corridorWidth : twinRooms[0].position.y + twinRooms[0].size.y - twinLeaves[0].owner.corridorWidth);

            //set corridor position and size
            corridorX = twinRooms[0].position.x + twinRooms[0].size.x;
            corridorY = Random.Range(Ymin, Ymax);
            corridorLength = twinRooms[1].position.x - (twinRooms[0].position.x + twinRooms[0].size.x);
            corridorPos = new Vector2Int(corridorX, corridorY);
            corridorSize = new Vector2Int(corridorLength, twinLeaves[0].owner.corridorWidth);

            //create exits at the two ends
            exit1 = new Vector2Int(corridorPos.x, corridorY + twinLeaves[0].owner.corridorWidth / 2);
            exit2 = new Vector2Int(corridorPos.x + corridorSize.x, corridorY + twinLeaves[0].owner.corridorWidth / 2);

            //create corridor with above parameters and add to this and sibling
            Corridor c = new Corridor(corridorPos, corridorSize, exit1, exit2);
            //twinLeaves[0].corridors.Add(c);
            //twinLeaves[1].corridors.Add(c);
            _leafA.parent.corridors.Add(c);
            //Debug.Log("right");

            //move one level up in the subdivision tree
            Debug.Log(_leafA.parent);
            CreateCorridors(_leafA.parent, _leafA.parent.sibling);
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

    public static bool RoomsAreConnected(BSPLeaf _leafA, BSPLeaf _leafB)
    {
        // if the two leaves are already connected, return
        var result = _leafA.corridors.Intersect(_leafB.corridors);
        if (result.Count() > 0)
        {
            Debug.Log("these corridors are already connected");
            return true;
        }
        return false;
    }

    //public bool HasCorridor(Cardinal _direction)
    //{
    //    if (corridors.ContainsKey(_direction))
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}

