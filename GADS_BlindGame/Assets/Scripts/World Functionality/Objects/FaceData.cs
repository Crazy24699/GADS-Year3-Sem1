using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceData : MonoBehaviour
{
    public int Index;

    public Vector3[] VertexLocalLocations;
    public Vector3[] VertexWorldLocations;
    public List<Vector3> FoundVertices = new List<Vector3>();

    public MeshFilter FilterRef;
    public Mesh MeshRef;

    private InteractableObject InteractableScript;
    protected PlayerFunctionality PlayerScript;
    
    private bool StartupRan = false;
    
    public void PopulateFaceInfo(MeshFilter CreatedFilter, Mesh CreatedMesh)
    {
        FilterRef = CreatedFilter;
        MeshRef = CreatedMesh;
        InteractableScript = GameObject.FindObjectOfType<InteractableObject>();

        PopulateWorldCords();

        StartupRan = true;
    }

    private void Update()
    {
        if(!StartupRan)
        {
            return;
        }
        PopulateWorldCords();
        if (FoundVertices.Count == 3)
        {
            InteractableScript.MergeTriangles(new List<int> { 0, Index });
        }
    }

    public Vector3[] PopulateWorldCords()
    {
        VertexLocalLocations = MeshRef.vertices;
        VertexWorldLocations = new Vector3[VertexLocalLocations.Length];

        Transform MeshTransform = FilterRef.transform;

        for(int i = 0; i < VertexLocalLocations.Length; i++)
        {
            VertexWorldLocations[i] = MeshTransform.TransformPoint(VertexLocalLocations[i]);
        }
        return VertexWorldLocations;
    }

}
