using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

//[ExecuteInEditMode]
public class InteractedObjects : MonoBehaviour
{
    public List<Vector3>  VerticeList = new List<Vector3>();
    public HashSet<Vector3> FoundVertices = new HashSet<Vector3>();

    public bool UpdateVectorDraws;

    public GameObject SpawnObject;
    public GameObject SphereOverlay;

    public MeshFilter MeshFilterRef;
    public Mesh MeshRef;

    public float VertexSize;

    public List<Vector3> LineLocations;

    protected int TriangleLimit;
    protected int VertexLimit;
    public int SpawnedVertices;

    public List<VerticePair> VerticePairClass=new List<VerticePair>();

    // Start is called before the first frame update
    void Start()
    {
        MeshFilterRef = GetComponent<MeshFilter>();
        MeshRef = MeshFilterRef.sharedMesh;

        TriangleLimit = MeshRef.triangles.Length;
        VertexLimit = MeshRef.vertices.Length;

    }

    protected void Update()
    {
        VerticeList = FoundVertices.ToList();
        if (VerticeList.Count() / 2 > VerticePairClass.Count())
        {
            VerticePair PairClass = new VerticePair();

            if (VerticePairClass.Count % 2 != 0)
            {
                //FoundVertices.Add(VerticeList[VerticeList.Count() - 1]);
            }
            PairClass.StartVertice = VerticeList[VerticeList.Count - 2];
            PairClass.EndVertice = VerticeList[VerticeList.Count - 1];
            VerticePairClass.Add(PairClass);

        }
    }

    public void SpawnVertexPoint(Vector3 WorldPosition)
    {
        
        GameObject ObjectRef=Instantiate(SpawnObject, WorldPosition, Quaternion.identity);
        ObjectRef.transform.SetParent(transform);
        SpawnedVertices++;
        ObjectRef.name = "Vertex " + SpawnedVertices;
    }

    private void OnDrawGizmos()
    {
        if (UpdateVectorDraws)
        {
            Gizmos.DrawRay(Vector3.zero, Vector3.one*5);
        }
        if (VerticePairClass.Count >= 1) 
        {
            foreach (var Pair in VerticePairClass) 
            {

                //Gizmos.DrawSphere(Pair.EndVertice, VertexSize);
                DrawLine(Pair.StartVertice, Pair.EndVertice);

            }
        }
        foreach (var Vertex in VerticeList)
        {
            //Gizmos.DrawSphere(Vertex, VertexSize);
        }
        //if(FoundVertices.Count==AllVertices.Count && !UpdateVectorDraws)
        //{
        //    return;
        //}

        //if (UpdateVectorDraws)
        //{
        //    DrawObjectData();

        //}

    }


    public void DrawLine(Vector3 StartPoint, Vector3 EndPoint)
    {

        Vector3 PotentialLocation = (StartPoint + EndPoint) / 2;

        if (LineLocations.Contains(PotentialLocation))
        {
            return; 
        }
        LineLocations.Add(PotentialLocation);
        // Create a new GameObject with a MeshFilter and MeshRenderer
        GameObject lineObject = new GameObject("LineMesh");
        MeshFilter meshFilter = lineObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = lineObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Create a new mesh
        Mesh lineMesh = new Mesh();

        // Define the vertices for the line
        Vector3[] vertices = new Vector3[2];
        vertices[0] = StartPoint;
        vertices[1] = EndPoint;



        // Define the indices for the line
        int[] indices = new int[2];
        indices[0] = 0;
        indices[1] = 1;

        // Assign the vertices and indices to the mesh
        lineMesh.vertices = vertices;
        lineMesh.SetIndices(indices, MeshTopology.Lines, 0);

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = lineMesh;

    }


}

[System.Serializable]
public class VerticePair
{
    public Vector3 StartVertice;
    public Vector3 EndVertice;

    public Vector3 Position;

    public void CheckVertexDistance()
    {

    }

    public bool VerticeInRange()
    {
        return false;
    }

    public Vector3 LineLocation(Vector3 StartPoint, Vector3 EndPoint)
    {
        Position = (StartPoint + EndPoint) / 2;
        return Position;
    }

}
