using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexTracker : MonoBehaviour
{
    public int Index;

    protected MeshRenderer RenderRef;
    protected PlayerFunctionality PlayerScript;

    public Vector3 WorldPosition;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindObjectOfType<PlayerFunctionality>();
        RenderRef = GetComponent<MeshRenderer>();
        ChangeTrackerState(false);
    }

    public void ChangeTrackerState(bool NewState)
    {
        switch (NewState)
        {
            case true:
                break;

            case false:
                break;
        }

        RenderRef.enabled = NewState;
    }

    // Update is called once per frame
    void Update()
    {

        //change this to a partial update type of thing, when the contact object is changed then it updates 
        if(PlayerScript.CurrentContactObject != null)
        {
            FaceData DataRef = PlayerScript.CurrentContactObject.GetComponent<FaceData>();
            this.transform.position = DataRef.VertexWorldLocations[Index];
            WorldPosition = DataRef.VertexWorldLocations[Index];
        }
    }


}
