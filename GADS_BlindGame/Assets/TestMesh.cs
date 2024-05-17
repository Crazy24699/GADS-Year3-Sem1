using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    
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
