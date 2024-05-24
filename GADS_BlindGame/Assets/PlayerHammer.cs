using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    public Camera ViewCamera; // Assign the main camera in the inspector
    protected float rayLength = 100f; // Length of the ray

    protected GameObject SelectedObject;

    public bool HandActivated = false;
    public Vector2 Position;

    public Vector3 upperBound; // Upper bound position
    public Vector3 lowerBound; // Lower bound position

    public Vector3 UpperBound;
    public Vector3 LowerBound;

    public GameObject HandObject;

    public float ReallyDid;
    public float XVector;
    public float YVector;

    private void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        if(HandActivated)
        {
            Vector2 TranslatedMousePosition = GetMousePosition();
            Position = TranslatedMousePosition;

            // Ensure positionValue is clamped between 0 and 1
            //Position = Mathf.Clamp01(Position)

           XVector  = Mathf.Lerp(lowerBound.x, upperBound.x, Position.x);
           YVector  = Mathf.Lerp(lowerBound.z, upperBound.z, Position.y);

            // Calculate the new position using linear interpolation (lerp)
            //Vector3 newPosition = Vector3.Lerp(LowerBound, UpperBound, ReallyDid);
            Vector3 HandPosition=new Vector3 (XVector, upperBound.y, YVector);

            // Apply the new position to the object
            HandObject.transform.position = HandPosition;


        }

        if(SelectedObject == null)
        {
            HandActivated = true;
        }
    }

    void SelectObject()
    {
        if (ViewCamera == null)
        {
            Debug.LogError("Main Camera is not assigned.");
            return;
        }


        Vector3 mousePosition = Input.mousePosition;
        Ray RayData = ViewCamera.ScreenPointToRay(mousePosition);
        GameObject HitObject;
        if (Physics.Raycast(RayData, out RaycastHit HitInfo, rayLength))
        {
            HitObject = HitInfo.collider.gameObject;

            //Debug.Log("Ray hit at: " + HitInfo.point+"   "+ HitInfo.collider.gameObject.name);

            if (HitObject.CompareTag("Interactable") && SelectedObject == null) 
            {
                SelectedObject = HitObject;
            }
        }

    }

    protected Vector2 GetMousePosition()
    {
        Vector3 MousePosition = Input.mousePosition;
        float XNormalized = MousePosition.x / ViewCamera.pixelWidth;
        float YNormalize = MousePosition.y / ViewCamera.pixelHeight;

        return new Vector2(Mathf.Round(XNormalized * 100.0f) * 0.01f, Mathf.Round(YNormalize * 100.0f) * 0.01f);
    }

}

