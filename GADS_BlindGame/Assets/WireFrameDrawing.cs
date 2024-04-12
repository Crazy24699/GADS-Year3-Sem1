using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireFrameDrawing : MonoBehaviour
{
    public MeshFilter meshFilter; // Reference to the mesh filter containing the mesh to draw
    public Bounds selectionBox; // The selection box defined by the player

    void Update()
    {
        // Check for user input to update the selection box
        UpdateSelectionBox();

        // Draw wireframe for selected vertices
        DrawWireframe();
    }

    void UpdateSelectionBox()
    {
        // Example: Use mouse input to define selection box
        // You can replace this with your own method to define the selection box
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectionBox.center = hit.point;
                selectionBox.size = new Vector3(1f, 1f, 1f); // Adjust size as needed
            }
        }
    }

    void DrawWireframe()
    {
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning("Mesh filter or shared mesh is not assigned!");
            return;
        }

        // Get vertices of the mesh
        Vector3[] vertices = meshFilter.sharedMesh.vertices;

        // Iterate through vertices and draw wireframe for selected vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPosition = meshFilter.transform.TransformPoint(vertices[i]);
            if (selectionBox.Contains(worldPosition))
            {
                // Draw wireframe for selected vertex
                Debug.DrawLine(worldPosition - Vector3.one * 0.1f, worldPosition + Vector3.one * 0.1f, Color.red);
            }
        }
    }
}
