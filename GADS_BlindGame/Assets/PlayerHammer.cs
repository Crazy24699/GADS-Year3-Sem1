using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    public Camera ViewCamera; // Assign the main camera in the inspector
    protected float rayLength = 100f; // Length of the ray

    protected GameObject SelectedNail;

    private void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            SelectObject();
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

            if (HitObject.name.Contains("Nail") && HitObject.CompareTag("Interactable") && SelectedNail == null) 
            {
                SelectedNail = HitObject;
            }
        }

    }
}
