using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public List<MeshFilter> ChunkMeshFilters = new List<MeshFilter>();
    public List<Mesh> ChunkMeshes = new List<Mesh>();

    public Mesh OriginalMesh;
    public Mesh MeshCollisionRef;
    private Mesh MeshMergeTarget;

    public Material MergedMaterial;

    public int Count;

    protected int VertexOffset;
    protected GameObject SingleMesh;

    // Start is called before the first frame update
    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshCollisionRef = OriginalMesh;
        GenerateMeshChunks(OriginalMesh, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Count = ChunkMeshFilters.Count;

        if (Input.GetKeyDown(KeyCode.L))
        {
            MergeTriangles(new List<int> { 0, 2 });
        }

    }

    public void GenerateMeshChunks(Mesh MeshRef, int TrianglesPerChunk)
    {
        Vector3[] MeshVertices = MeshRef.vertices;
        int[] MeshTriangles = MeshRef.triangles;
        int TriangleNum = MeshTriangles.Length / 3;

        int ChunkCount = Mathf.CeilToInt((float)TriangleNum / TrianglesPerChunk);

        for (int ChunkIndex = 0; ChunkIndex < ChunkCount; ChunkIndex++) 
        {

            int StartingTri = ChunkIndex * TrianglesPerChunk;
            int EndingTri = Mathf.Min(StartingTri+TrianglesPerChunk,TriangleNum);

            List<Vector3> ChunkVertices = new List<Vector3>();
            List<int> ChunkIndices = new List<int>();

            Dictionary<int, int> VertexMap = new Dictionary<int, int>();

            for (int i = StartingTri; i < EndingTri; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int VertexIndex = MeshTriangles[i * 3 + j];

                    if(!VertexMap.ContainsKey(VertexIndex))
                    {
                        VertexMap[VertexIndex] = ChunkVertices.Count;
                        ChunkVertices.Add(MeshVertices[VertexIndex]);
                    }

                    ChunkIndices.Add(VertexMap[VertexIndex]);
                }
            }
            
            Mesh MeshChunk = new Mesh();
            GameObject TriangleChunkObject = new GameObject("Mesh Chunk: " + ChunkIndex);
            FaceData TriangleFaceData = TriangleChunkObject.AddComponent<FaceData>();
            MeshFilter NewMeshFilter = TriangleChunkObject.AddComponent<MeshFilter>();

            MeshChunk.vertices = ChunkVertices.ToArray();
            MeshChunk.triangles = ChunkIndices.ToArray();
            MeshChunk.RecalculateBounds();
            MeshChunk.RecalculateNormals();
            ChunkMeshes.Add(MeshChunk);

            TriangleFaceData.Index = ChunkIndex;
            TriangleFaceData.VertexLocalLocations = ChunkVertices.ToArray();

            TriangleChunkObject.transform.parent = transform;
            TriangleChunkObject.transform.localPosition = Vector3.zero;
            TriangleChunkObject.gameObject.tag = "Interactable";

            NewMeshFilter.mesh = MeshChunk;
            ChunkMeshFilters.Add(NewMeshFilter);

            TriangleChunkObject.AddComponent<MeshCollider>();

            TriangleChunkObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            TriangleFaceData.PopulateFaceInfo(NewMeshFilter, MeshChunk);
            TriangleFaceData.PopulateWorldCords();

        }


    }


    public void MergeTriangles(List<int>ChunkIndices)
    {
        foreach (var Mesh in ChunkMeshFilters)
        {
            Debug.Log(Mesh.name);
        }
        Debug.Log(ChunkMeshFilters.Count);
        if (ChunkIndices.Count < 2) 
        {
            return;
        }
        Debug.Log(ChunkMeshFilters.Count);
        if (MeshMergeTarget == null)
        {
            CreateMergeMesh();
        }
        Debug.Log(ChunkMeshFilters.Count);
        foreach (var Chunk in ChunkIndices)
        {
            
            if (Chunk >= ChunkMeshFilters.Count)
            {
                Debug.LogError("Chunk index is invalid      "+Chunk+"   "+ChunkMeshFilters.Count);
                continue;
            }

            Mesh ChunkMesh = ChunkMeshFilters[Chunk].mesh;

            List<Vector3> MergedVertices = new List<Vector3>(MeshMergeTarget.vertices);
            List<int> MergedTriangles = new List<int>(MeshMergeTarget.triangles);

            foreach (var Vertex in ChunkMesh.vertices)
            {
                MergedVertices.Add(Vertex);
            }
            foreach (var Triangle in ChunkMesh.triangles)
            {
                MergedTriangles.Add(Triangle);
            }

            MeshMergeTarget.vertices = MergedVertices.ToArray();
            MeshMergeTarget.triangles = MergedTriangles.ToArray();

            MeshMergeTarget.RecalculateNormals();
            MeshMergeTarget.RecalculateBounds();

            VertexOffset += ChunkMesh.vertexCount;
            Destroy(ChunkMeshFilters[Chunk].gameObject);

        }

    }

    private void CreateMergeMesh()
    {
        MeshMergeTarget = new Mesh();
        MeshMergeTarget.name = "Single Mesh";

        SingleMesh = new GameObject("Merged Meshes");

        MeshFilter MergedFilter = SingleMesh.AddComponent<MeshFilter>();
        MergedFilter.mesh = MeshMergeTarget;
        MeshRenderer MergedMeshRenderer = SingleMesh.AddComponent<MeshRenderer>();
        MergedMeshRenderer.material = MergedMaterial;
        
    }

}
