using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    protected float rayLength = 100f; // Length of the ray

    public GameObject SelectedObject;
    protected GameObject HoverObject;       //Where the mouse cursor is hovering over
    public GameObject HandObject;
    public GameObject HammerObject;

    public Camera ViewCamera; // Assign the main camera in the inspector
    [SerializeField] protected LayerMask HoverObjectMask;
    [SerializeField] protected LayerMask NonInteractable;

    
    public bool HandActivated = false;
    public bool BracingNail = false;

    public Vector2 Position;
    public Vector3 MousePosition;
    public Vector3 HammerHitPosition = new Vector3(0, 14.64f, 0);

    public enum PlayerState
    {
        SelectingNail,
        BracingNail,
    }
    public PlayerState CurrentState;

    private void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {

            switch (CurrentState)
            {
                case PlayerState.SelectingNail:
                    SelectObject();
                    break;

                case PlayerState.BracingNail:
                    SwingHammer();
                    break;

                default:
                    break;
            }

        }

        MouseMovement();

        if(SelectedObject == null)
        {
            HandActivated = true;
        }

        if (SelectedObject != null && SelectedObject.layer != NonInteractable) 
        {
            SelectedObject.transform.position = MousePosition;
            //SelectedObject.transform.position = new Vector3(MousePosition.x, 15, MousePosition.z);
        }

    }

    private void MouseMovement()
    {
        Vector3 MouseScreenPosition = Input.mousePosition;
        Ray MousePositionRay = ViewCamera.ScreenPointToRay(MouseScreenPosition);


        if (Physics.Raycast(MousePositionRay, out RaycastHit HitInfo, rayLength, HoverObjectMask))
        {
            HoverObject = HitInfo.collider.gameObject;
            MousePosition = HitInfo.point;

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
        Debug.Log("for her");
        GameObject HitObject;
        if (Physics.Raycast(RayData, out RaycastHit HitInfo, rayLength))
        {
            HitObject = HitInfo.collider.gameObject;

            Debug.Log("Ray hit at: " + HitInfo.point + "   " + HitInfo.collider.gameObject.name);

            if (HitObject.CompareTag("Interactable") && SelectedObject == null) 
            {
                Debug.Log("for us");
                SelectedObject = HitObject;
                
            }
        }
        if (SelectedObject == null)
        {
            return;
        }
        SelectedObject.transform.parent = null;
        SelectedObject.layer = LayerMask.NameToLayer("Not Interactable");
    }

    public void SwingHammer()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray RayData = ViewCamera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(RayData, out RaycastHit HitInfo, rayLength))
        {
            HammerHitPosition = new Vector3(HitInfo.point.x, 14.64f, HitInfo.point.z);
        }
        HammerObject.transform.position = HammerHitPosition;

    }

}

