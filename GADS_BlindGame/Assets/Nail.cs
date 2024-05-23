using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public bool Interactable = true;
    public Vector3 PermanentPosition;

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.gameObject.CompareTag("Nail Point"))
        {
            PermanentPosition = Collision.transform.TransformPoint(Collision.transform.position);
        }
    }

    private void OnTriggerExit(Collider Collision)
    {
        
    }
}
