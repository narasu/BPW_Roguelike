using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCEL
{
    public List<Vertex> vertices = new List<Vertex>();
    public List<HalfEdge> halfEdges = new List<HalfEdge>();
    public List<Face> faces = new List<Face>();
    public Face outerFace;
    public Face rootFace; 

    public class HalfEdge
    {
        public HalfEdge twin, next, prev;
        public Vertex origin;
        public Face incidentFace;

        public HalfEdge(Vertex _origin, Face _incidentFace)
        {
            origin = _origin;
            incidentFace = _incidentFace;
            //halfEdges.Add(this);
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

        public void Split(Vector2Int _splitPosition)
        {
            Vertex midPoint = new Vertex(_splitPosition);
        }
    }

    public class Vertex
    {
        public Vector2Int coords;
        public HalfEdge outgoingEdge;
        public Vertex(Vector2Int _coords)
        {
            coords = _coords;
        }
    }
    public class Face
    {
        public HalfEdge incidentEdge;

        public static void ConnectInnerOuterFace(Face _outer, Face _inner)
        {
            HalfEdge.SetTwins(_outer.incidentEdge.prev, _inner.incidentEdge);
            HalfEdge.SetTwins(_outer.incidentEdge.prev.prev, _inner.incidentEdge.next);
            HalfEdge.SetTwins(_outer.incidentEdge.next, _inner.incidentEdge.next.next);
            HalfEdge.SetTwins(_outer.incidentEdge, _inner.incidentEdge.prev);
        }

        public static Face Create(Vector2Int _xy, Vector2Int _wh)
        {
            Face face = new Face();

            // create vertices in counterclockwise arrangement
            Vertex A = new Vertex(new Vector2Int(_xy.x, _xy.y));
            Vertex B = new Vertex(new Vector2Int(_xy.x, _wh.y));
            Vertex C = new Vertex(new Vector2Int(_wh.x, _wh.y));
            Vertex D = new Vertex(new Vector2Int(_wh.x, _xy.y));

            // create half edge for each vertex
            HalfEdge AB = new HalfEdge(A, face);
            HalfEdge BC = new HalfEdge(B, face);
            HalfEdge CD = new HalfEdge(C, face);
            HalfEdge DA = new HalfEdge(D, face);

            // store reference to adjacent edges
            AB.next = BC; BC.prev = AB;
            BC.next = CD; CD.prev = BC;
            CD.next = DA; DA.prev = CD;
            DA.next = AB; AB.prev = DA;

            face.incidentEdge = AB;

            return face;
        }
        public static Face CreateOuter(Vector2Int _xy, Vector2Int _wh)
        {
            Face face = new Face();

            // create vertices in clockwise order
            Vertex A = new Vertex(new Vector2Int(_xy.x, _xy.y));
            Vertex B = new Vertex(new Vector2Int(_wh.x, _xy.y));
            Vertex C = new Vertex(new Vector2Int(_wh.x, _wh.y));
            Vertex D = new Vertex(new Vector2Int(_xy.x, _wh.y));

            // create half edge for each vertex
            HalfEdge AB = new HalfEdge(A, face);
            HalfEdge BC = new HalfEdge(B, face);
            HalfEdge CD = new HalfEdge(C, face);
            HalfEdge DA = new HalfEdge(D, face);

            // store reference to adjacent edges
            AB.next = BC; BC.prev = AB;
            BC.next = CD; CD.prev = BC;
            CD.next = DA; DA.prev = CD;
            DA.next = AB; AB.prev = DA;

            face.incidentEdge = AB;

            return face;
        }
    }
}
