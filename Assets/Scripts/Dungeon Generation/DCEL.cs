﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCEL
{
    public DCEL()
    {
        instance = this;
    }
    
    public List<Vertex> vertices = new List<Vertex>();
    public List<HalfEdge> halfEdges = new List<HalfEdge>();
    public List<Face> faces = new List<Face>();
    public Face outerFace;
    public Face rootFace;

    private static DCEL instance;
    public static DCEL Instance
    {
        get { return instance; }
    }

    public class HalfEdge
    {
        public HalfEdge twin, next, prev;
        public Vertex origin;
        public Face incidentFace;

        public HalfEdge(Vertex _origin, Face _incidentFace)
        {
            origin = _origin;
            incidentFace = _incidentFace;
            Instance.halfEdges.Add(this);
        }

        public Vertex GetDestination()
        {
            return twin.origin;
        }

        public static void SetTwins(HalfEdge e1, HalfEdge e2)
        {
            e1.twin = e2;
            e2.twin = e1;
        }

        public void Split(Vector2Int _midPoint)
        {
            Vertex midPoint = new Vertex(_midPoint);
            HalfEdge e1 = new HalfEdge(midPoint, incidentFace);
            HalfEdge e2 = new HalfEdge(next.origin, next.incidentFace);

            SetTwins(e1, e2);

            e1.prev = this;
            e1.next = next;
            next = e1;
            e2.prev = twin.prev;
            twin.prev = e2;
            twin.origin = midPoint;
            e2.next = twin;
        }
    }

    public class Vertex
    {
        public Vector2Int coords;
        public HalfEdge outgoingEdge;
        public Vertex(Vector2Int _coords)
        {
            coords = _coords;
            Instance.vertices.Add(this);
        }
    }
    public class Face
    {
        public HalfEdge incidentEdge;

        public Face(Vector2Int _xy, Vector2Int _wh)
        {
            
            // create vertices in counterclockwise arrangement
            Vertex A = new Vertex(new Vector2Int(_xy.x, _xy.y));
            Vertex B = new Vertex(new Vector2Int(_xy.x, _xy.y + _wh.y));
            Vertex C = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y + _wh.y));
            Vertex D = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y));

            // create half edge for each vertex
            HalfEdge AB = new HalfEdge(A, this);
            HalfEdge BC = new HalfEdge(B, this);
            HalfEdge CD = new HalfEdge(C, this);
            HalfEdge DA = new HalfEdge(D, this);

            // store reference to adjacent edges
            AB.next = BC; BC.prev = AB;
            BC.next = CD; CD.prev = BC;
            CD.next = DA; DA.prev = CD;
            DA.next = AB; AB.prev = DA;

            incidentEdge = AB;

        }

        public Face(Vector2Int _xy, Vector2Int _wh, bool outer)
        {
            
            Vertex A, B, C, D;

            if (outer)
            {
                // create vertices in clockwise order
                A = new Vertex(new Vector2Int(_xy.x, _xy.y));
                B = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y));
                C = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y + _wh.y));
                D = new Vertex(new Vector2Int(_xy.x, _xy.y + _wh.y));
            }
            else
            {
                // create vertices in counterclockwise arrangement
                A = new Vertex(new Vector2Int(_xy.x, _xy.y));
                B = new Vertex(new Vector2Int(_xy.x, _xy.y + _wh.y));
                C = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y + _wh.y));
                D = new Vertex(new Vector2Int(_xy.x + _wh.x, _xy.y));
            }
            

            // create half edge for each vertex
            HalfEdge AB = new HalfEdge(A, this);
            HalfEdge BC = new HalfEdge(B, this);
            HalfEdge CD = new HalfEdge(C, this);
            HalfEdge DA = new HalfEdge(D, this);

            // store reference to adjacent edges
            AB.next = BC; BC.prev = AB;
            BC.next = CD; CD.prev = BC;
            CD.next = DA; DA.prev = CD;
            DA.next = AB; AB.prev = DA;

            incidentEdge = AB;

        }

        public void Split()
        {

        }

        public static void ConnectInnerOuterFace(Face _outer, Face _inner)
        {
            HalfEdge.SetTwins(_outer.incidentEdge.prev, _inner.incidentEdge);
            HalfEdge.SetTwins(_outer.incidentEdge.prev.prev, _inner.incidentEdge.next);
            HalfEdge.SetTwins(_outer.incidentEdge.next, _inner.incidentEdge.next.next);
            HalfEdge.SetTwins(_outer.incidentEdge, _inner.incidentEdge.prev);
        }

        //public static Face Create(Vector2Int _xy, Vector2Int _wh)
        //{
        //    Face face = new Face();

        //    // create vertices in counterclockwise arrangement
        //    Vertex A = new Vertex(new Vector2Int(_xy.x, _xy.y));
        //    Vertex B = new Vertex(new Vector2Int(_xy.x, _wh.y));
        //    Vertex C = new Vertex(new Vector2Int(_wh.x, _wh.y));
        //    Vertex D = new Vertex(new Vector2Int(_wh.x, _xy.y));

        //    // create half edge for each vertex
        //    HalfEdge AB = new HalfEdge(A, face);
        //    HalfEdge BC = new HalfEdge(B, face);
        //    HalfEdge CD = new HalfEdge(C, face);
        //    HalfEdge DA = new HalfEdge(D, face);

        //    // store reference to adjacent edges
        //    AB.next = BC; BC.prev = AB;
        //    BC.next = CD; CD.prev = BC;
        //    CD.next = DA; DA.prev = CD;
        //    DA.next = AB; AB.prev = DA;

        //    face.incidentEdge = AB;

        //    return face;
        //}
        //public static Face CreateOuter(Vector2Int _xy, Vector2Int _wh)
        //{
        //    Face face = new Face();

        //    // create vertices in clockwise order
        //    Vertex A = new Vertex(new Vector2Int(_xy.x, _xy.y));
        //    Vertex B = new Vertex(new Vector2Int(_wh.x, _xy.y));
        //    Vertex C = new Vertex(new Vector2Int(_wh.x, _wh.y));
        //    Vertex D = new Vertex(new Vector2Int(_xy.x, _wh.y));

        //    // create half edge for each vertex
        //    HalfEdge AB = new HalfEdge(A, face);
        //    HalfEdge BC = new HalfEdge(B, face);
        //    HalfEdge CD = new HalfEdge(C, face);
        //    HalfEdge DA = new HalfEdge(D, face);

        //    // store reference to adjacent edges
        //    AB.next = BC; BC.prev = AB;
        //    BC.next = CD; CD.prev = BC;
        //    CD.next = DA; DA.prev = CD;
        //    DA.next = AB; AB.prev = DA;

        //    face.incidentEdge = AB;

        //    return face;
        //}
    }
}
