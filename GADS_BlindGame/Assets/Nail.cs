using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nail : MonoBehaviour
{
    //public bool Interactable = true;
    //public Vector3 PermanentPosition;
    private NailOutliner NailOutliner;

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.gameObject.CompareTag("Hammer") && NailOutliner != null) 
        {
            this.transform.position = new Vector3(transform.position.x, 14.48f, transform.position.z);
            NailOutliner.ChangeOutliner();
            this.gameObject.tag = "Non Interactable";
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<BoxCollider>());
        }
        if(Collision.gameObject.CompareTag("Nail Point"))
        {
            NailOutliner = Collision.gameObject.GetComponent<NailOutliner>();
            Debug.Log("ran");
        }
    }

    private void OnTriggerExit(Collider Collision)
    {

    }
}
