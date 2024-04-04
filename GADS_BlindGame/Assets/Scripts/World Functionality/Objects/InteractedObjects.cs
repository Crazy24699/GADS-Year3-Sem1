using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]
public class InteractedObjects : MonoBehaviour
{
    //public HashSet<Vector3> FoundVertices = new HashSet<Vector3>();
    public List<Vector3> FoundVertices = new List<Vector3>();
    protected HashSet<Vector3> AllVertices = new HashSet<Vector3>();

    [HideInInspector]public bool FoundAll = false;
    public bool UpdateVectorDraws;

    // Start is called before the first frame update
    void Start()
    {
        
        if(FoundAll)
        {
            FoundAll = false;
        }
        //FoundVertices.Add(Vector3.zero);
        //AllVertices = this.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.ToHashSet();
        //Debug.Log(AllVertices.Count);
    }


    protected void DrawObjectData()
    {

        foreach(var Vertex in FoundVertices)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vertex, 0.1f);
            Debug.Log("matter   "+ AllVertices.Count);
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

}
