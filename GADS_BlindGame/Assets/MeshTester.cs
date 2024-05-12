using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshTester : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();

    public List<Face> faces = new List<Face>();

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    // Function to add a vertex
    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);

        // Check if the new vertex connects to any face
        for (int i = 0; i < faces.Count; i++)
        {
            if (faces[i].CheckAndAddVertex(vertices.Count - 1, vertices))
            {
                // Remove the face if it's completed
                faces.RemoveAt(i);
                i--;
            }
        }

        UpdateMesh();
    }

    void Update()
    {
        // Example: Add a vertex every second
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Whenevern it rains");
            int XVert = Mathf.RoundToInt(Random.Range(-5f, 5f));
            int YVert = Mathf.RoundToInt(Random.Range(-5f, 5f));
            int ZVert = Mathf.RoundToInt(Random.Range(-5f, 5f));

            Vector3 randomPoint = new Vector3(XVert, YVert, ZVert);
            AddVertex(randomPoint);
            if (vertices.Count >= 3)
            {
                // Add a face with the last three vertices added
                AddFace(vertices.Count - 3, vertices.Count - 2, vertices.Count - 1);
            }
        }
    }

    // Function to add a face
    public void AddFace(int vertexIndex1, int vertexIndex2, int vertexIndex3)
    {
        faces.Add(new Face(vertexIndex1, vertexIndex2, vertexIndex3));

        UpdateMesh();
    }

    // Function to update the mesh with the current vertices and triangles
    private void UpdateMesh()
    {
        triangles.Clear();

        // Add vertices to the triangle list for all faces
        foreach (var face in faces)
        {
            foreach (var index in face.vertexIndices)
            {
                triangles.Add(index);
            }
        }

        // Update mesh data
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    // Helper class to represent a face
    public class Face
    {
        public int[] vertexIndices;

        public Face(int vertexIndex1, int vertexIndex2, int vertexIndex3)
        {
            vertexIndices = new int[] { vertexIndex1, vertexIndex2, vertexIndex3 };
        }

        // Function to check if a vertex connects to this face, and add it if it does
        public bool CheckAndAddVertex(int vertexIndex, List<Vector3> vertices)
        {
            if (IsVertexConnected(vertexIndex, vertices))
            {
                return true;
            }
            return false;
        }

        // Function to check if a vertex is connected to this face
        private bool IsVertexConnected(int vertexIndex, List<Vector3> vertices)
        {
            foreach (int index in vertexIndices)
            {
                if (Vector3.Distance(vertices[index], vertices[vertexIndex]) < 0.1f)
                    return true;
            }
            return false;
        }
    }
}
