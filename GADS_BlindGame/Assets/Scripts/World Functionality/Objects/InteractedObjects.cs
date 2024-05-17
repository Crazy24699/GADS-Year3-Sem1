using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public List<VerticePair> VerticePairClass=new List<VerticePair>();

    // Start is called before the first frame update
    void Start()
    {
        MeshFilterRef = GetComponent<MeshFilter>();
        MeshRef = MeshFilterRef.sharedMesh;
        
    }

    protected void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (UpdateVectorDraws)
        {
            Gizmos.DrawRay(Vector3.zero, Vector3.one*5);
        }
        if (FoundVertices.Count() >= 2) 
        {
            foreach (var Pair in VerticePairClass) 
            {

            }
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



}

public class VerticePair
{
    public Vector3 StartVertice;
    public Vector3 EndVertice;

    public void CheckVertexDistance()
    {

    }

    public bool VerticeInRange()
    {
        return false;
    }

}
