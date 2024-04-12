using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode]
public class CollidingBox : MonoBehaviour
{
    public GameObject PointRef;

    public InteractedObjects CurrentObject;

    public GameObject CollidedObject;
    public Material CollidedMaterial;
    public MeshFilter MeshFilterRef;

    int DelayTime;
    int CurrentTime = 1;

    bool StartSpawning = false;

    int spawned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider ObjectCollision)
    {



        //RetrievePointData(ObjectCollision);
        PointDataTest(ObjectCollision);
        if (CurrentObject != null)
        {
            //Debug.Log(CurrentObject.FoundVertices.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentObject.UpdateVectorDraws = false;
    }

    public void RetrievePointData(Collider CollisionRef)
    {
        if (!CollisionRef.CompareTag("Interactable"))
        {
            Debug.Log("Clock");
            return;
        }


        Debug.Log("found    ");

        //CurrentObject = CollisionRef.TryGetComponent(out CurrentObject) ?
        //CollisionRef.GetComponent<InteractedObjects>() :
        //CurrentObject=CollisionRef.AddComponent<InteractedObjects>();

        CurrentObject = CollisionRef.gameObject.GetComponent<InteractedObjects>();
        Mesh MeshRef = CollisionRef.GetComponent<MeshFilter>().sharedMesh;
        HashSet<Vector3> Verts = new HashSet<Vector3>(MeshRef.vertices);


        if((MeshRef != null && CurrentObject != null))
        {
            Transform WorldTransform = CollisionRef.transform;
            //Vector3[] AllVertices = MeshRef.sharedMesh.vertices.ToArrayPooled();
            List<Vector3> Coords = new List<Vector3>(Verts);

            for (int i = 0; i < Coords.Count; i++)
            {
                Vector3 VertWorldPosition = Coords[i];
                if (this.GetComponent<Collider>().bounds.Contains(VertWorldPosition))
                {
                    CurrentObject.FoundVertices.Add(WorldTransform.position);
                }
            }

        }
        CurrentObject.UpdateVectorDraws = true;

    }


    public void PointDataTest(Collider CollisionRef)
    {
        if (!CollisionRef.CompareTag("Interactable"))
        {
            return;
        }

        CurrentObject = CollisionRef.TryGetComponent(out CurrentObject) ?
        CollisionRef.GetComponent<InteractedObjects>() :
        CurrentObject = CollisionRef.AddComponent<InteractedObjects>();

        Mesh MeshRef = CollisionRef.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] AllVerticies = MeshRef.vertices;

        Transform WorldVertexTransform = CollisionRef.GetComponent<MeshFilter>().transform;

        if (MeshRef != null)
        {

            for (int i = 0; i < AllVerticies.Length; i++)
            {
                Vector3 VertexWorldPosition = WorldVertexTransform.TransformPoint(AllVerticies[i]);

                bool HasFoundVerts = CurrentObject.FoundVertices.Contains(VertexWorldPosition);

                if(this.GetComponent<Collider>().bounds.Contains(VertexWorldPosition) && !HasFoundVerts)
                {
                    
                    CurrentObject.FoundVertices.Add(VertexWorldPosition);
                }

            }
            //DrawMeshFrame(MeshRef);
            StartCoroutine(CurrentObject.DrawObjectData());
        }


    }

    public IEnumerator SpawnDelay(Vector3 SpawnPosition)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject Object=Instantiate(PointRef, SpawnPosition, Quaternion.identity);
        Object.name = "Object " + spawned +"  ";
        spawned++;
        
    }

    protected void DrawMeshFrame(Mesh DrawMeshRef)
    {
        Graphics.DrawMesh(DrawMeshRef, Matrix4x4.identity, CollidedMaterial, gameObject.layer);
    }

}
