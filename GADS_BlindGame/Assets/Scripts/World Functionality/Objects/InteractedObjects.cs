using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class InteractedObjects : MonoBehaviour
{
    public HashSet<Vector3> FoundVertices = new HashSet<Vector3>();
    protected HashSet<Vector3> AllVertices = new HashSet<Vector3>();

    [HideInInspector]public bool FoundAll = false;
    

    // Start is called before the first frame update
    void Start()
    {
        if(FoundAll)
        {
            FoundAll = false;
        }
        AllVertices = this.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.ToHashSet();
        //Debug.Log(AllVertices.Count);
    }


    protected void DrawObjectData()
    {

    }

    private void OnDrawGizmos()
    {
        if(FoundVertices.Count==AllVertices.Count)
        {
            return;
        }



    }

}
