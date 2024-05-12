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

    public GameObject SpawnObject;

    int SpawnNum;

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


    public IEnumerator DrawObjectData()
    {

        foreach(var Vertex in FoundVertices)
        {
            yield return new WaitForSeconds(0.5f);
            //GameObject Object = Instantiate(SpawnObject, Vertex, Quaternion.identity);
            //Object.name = "Object " + SpawnNum + "  ";

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
