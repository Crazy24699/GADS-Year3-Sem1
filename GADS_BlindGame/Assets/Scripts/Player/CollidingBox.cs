using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode]
public class CollidingBox : MonoBehaviour
{


    public InteractedObjects CurrentObject;

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
        
    }

    public void RetrievePointData(Collider CollisionRef)
    {
        if (!CollisionRef.CompareTag("Interactable"))
        {
            return;
        }

        MeshFilter MeshRef = CollisionRef.GetComponent<MeshFilter>();

        CurrentObject = CollisionRef.TryGetComponent(out CurrentObject) ?
        CollisionRef.GetComponent<InteractedObjects>() :
        CollisionRef.AddComponent<InteractedObjects>();

        if((MeshRef != null && CurrentObject != null) && !CurrentObject.FoundAll)
        {
            Transform WorldTransform = CollisionRef.transform;
            Vector3[] AllVertices = MeshRef.sharedMesh.vertices.ToArrayPooled();

            for (int i = 0; i < AllVertices.Length; i++)
            {
                Vector3 VertWorldPosition = transform.TransformPoint(AllVertices[i]);
                if (this.GetComponent<Collider>().bounds.Contains(VertWorldPosition))
                {
                    CurrentObject.FoundVertices.Add(VertWorldPosition);
                }
            }

        }
        
    }


}
