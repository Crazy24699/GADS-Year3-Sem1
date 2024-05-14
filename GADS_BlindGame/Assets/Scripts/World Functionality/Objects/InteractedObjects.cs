using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]
public class InteractedObjects : MonoBehaviour
{
    //public HashSet<Vector3> FoundVertices = new HashSet<Vector3>();
    public List<Vector3>  VerticeList = new List<Vector3>();
    public HashSet<Vector3> FoundVertices = new HashSet<Vector3>();
    protected HashSet<Vector3> AllVertices = new HashSet<Vector3>();

    [HideInInspector]public bool FoundAll = false;
    public bool UpdateVectorDraws;

    public List<int> Triangles = new List<int>();
    public List<int> AllTriangles = new List<int>();

    public GameObject SpawnObject;
    public GameObject SphereOverlay;

    public MeshFilter MeshFilterRef;
    public Mesh MeshRef;

    int SpawnNum;

    [SerializeField]protected List<FaceData> Faces = new List<FaceData>();

    // Start is called before the first frame update
    void Start()
    {
        MeshFilterRef = GetComponent<MeshFilter>();
        //SphereOverlay.AddComponent<MeshFilter>();
        MeshRef = SphereOverlay.gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        AllTriangles = new List<int>(MeshRef.triangles) ;
        if (MeshRef == null)
        {
            Debug.Log("love bites");
        }
        //MeshRef = new Mesh();
        //MeshFilterRef.mesh = MeshRef;

        if(FoundAll)
        {
            FoundAll = false;
        }
        //FoundVertices.Add(Vector3.zero);
        //AllVertices = this.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.ToHashSet();
        //Debug.Log(AllVertices.Count);
    }


    public IEnumerator DrawObjectData()
    {

        foreach(var Vertex in FoundVertices)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateMesh();
            //GameObject Object = Instantiate(SpawnObject, Vertex, Quaternion.identity);
            //Object.name = "Object " + SpawnNum + "  ";

        }
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            UpdateMesh();
        }
    }

    public void FoundFacesUpdate(Vector3 VertexRef)
    {
        AddVertex(VertexRef);
        if (FoundVertices.Count >= 3)
        {
            AddFace(FoundVertices.Count - 3, FoundVertices.Count - 2, FoundVertices.Count - 1);
        }
    }

    private void OnDrawGizmos()
    {
        //if(FoundVertices.Count==AllVertices.Count && !UpdateVectorDraws)
        //{
        //    return;
        //}

        //if (UpdateVectorDraws)
        //{
        //    DrawObjectData();

        //}

    }

    protected void AddVertex(Vector3 VertexRef)
    {

        FoundVertices.Add(VertexRef);
        for (int i = 0; i < Faces.Count; i++)
        {
            if (Faces[i].HandleFaceVertex(FoundVertices.Count - 1, FoundVertices.ToList()))
            {
                Faces.RemoveAt(i);
                i--;
            }
        }

        UpdateMesh();
    }

    protected void UpdateMesh()
    {

        foreach (var Face in Faces)
        {
            foreach (var IndexValue in Face.VerticeIndex)
            {
                Triangles.Add(IndexValue);
                Debug.Log("curesed with you the things we do when love bites");
            }
        }
        
        VerticeList = FoundVertices.ToList();

        MeshRef.Clear();
        MeshRef.vertices = FoundVertices.ToArray();
        MeshRef.triangles = Triangles.ToArray();
        MeshRef.RecalculateNormals();

    }

    protected void AddFace(int VerticeIndex1, int VerticeIndex2, int VerticeIndex3)
    {
        FaceData CreatedFace = new FaceData();
        CreatedFace.FaceDimetions(VerticeIndex1, VerticeIndex2, VerticeIndex3);
        Faces.Add(CreatedFace);

        UpdateMesh();
    }

}

[Serializable]
public class FaceData
{
    public int[] VerticeIndex;

    public void FaceDimetions(int Vertex1, int Vertex2, int Vertex3)
    {
        VerticeIndex = new int[] { Vertex1, Vertex2, Vertex3 };
    }

    public bool HandleFaceVertex(int VertexIndex, List<Vector3> VerticeLocation)
    {
        if (VertexConntected(VertexIndex, VerticeLocation))
        {
            return true;
        }
        return false;
    }

    protected bool VertexConntected(int VertexIndex, List<Vector3> VerticeLocation)
    {
        foreach (int Index in VerticeIndex)
        {
            if (Vector3.Distance(VerticeLocation[Index], VerticeLocation[Index]) < 0.075f)
            {
                return true;
            }
        }
        return false;
    }

}
