using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContactPoints : MonoBehaviour
{
    [SerializeField]private FaceData FaceDataScript;
    private PlayerFunctionality PlayerScript;

    public GameObject MeshlocationBase;
    protected List<GameObject> VerticeContacts = new List<GameObject>();
    public GameObject VertexTrackers;

    public Vector3 DrawDirection;

    public float DrawLength;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindObjectOfType<PlayerFunctionality>();
        VerticeContacts = GameObject.FindGameObjectsWithTag("ContactPoint").ToList();
    }

    private void FixedUpdate()
    {
        DrawDirection = MeshlocationBase.transform.position - this.transform.position;

        RaycastHit HitObject;
        Debug.DrawRay(transform.position, DrawDirection.normalized * DrawLength, Color.blue);

        if(Physics.Raycast(transform.position,DrawDirection,out HitObject, DrawLength))
        {
            if (FaceDataScript == null && HitObject.collider.GetComponent<FaceData>() != null)
            {
                FaceDataScript = HitObject.collider.GetComponent<FaceData>();
            }
            if (HitObject.collider != null && HitObject.collider.CompareTag("Interactable") && FaceDataScript.name != HitObject.collider.name) 
            {
                FaceDataScript = HitObject.collider.GetComponent<FaceData>();
                PlayerScript.CurrentContactObject = FaceDataScript.gameObject;
            }
        }

    }

    public void SpawnTrackers()
    {

    }

    public void MeshUpdate()
    {
        FaceDataScript = null;
        foreach (var Vertex in VerticeContacts)
        {
            Vertex.GetComponent<VertexTracker>().ChangeTrackerState(false);
        }
    }

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.CompareTag("ContactPoint") && FaceDataScript != null) 
        {
            Debug.Log("Hit");
            Collision.GetComponent<VertexTracker>().ChangeTrackerState(true);
            FaceDataScript.FoundVertices.Add(Collision.GetComponent<VertexTracker>().WorldPosition);
            if(FaceDataScript.FoundVertices.Count == 3)
            {
                PlayerScript.CurrentContactObject = null;
            }
        }
    }

}
