using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nail : MonoBehaviour
{
    //public bool Interactable = true;
    //public Vector3 PermanentPosition;
    private NailOutliner NailOutliner;

    PlayerHammer PlayerHammerScript;

    private void Start()
    {
        PlayerHammerScript = FindObjectOfType<PlayerHammer>();
    }

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.gameObject.CompareTag("Hammer") && NailOutliner != null && !PlayerHammerScript.HitHand) 
        {
            StartCoroutine(Delay(Collision));
        }
        if(Collision.gameObject.CompareTag("Nail Point"))
        {
            NailOutliner = Collision.gameObject.GetComponent<NailOutliner>();
            Debug.Log("ran");
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            
        }
    }

    public IEnumerator Delay(Collider Collision)
    {
        yield return new WaitForSeconds(0.025f);
        if (!PlayerHammerScript.HitHand)
        {
            this.transform.position = new Vector3(transform.position.x, 14.48f, transform.position.z);
            NailOutliner.ChangeOutliner();
            this.gameObject.tag = "Non Interactable";
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<BoxCollider>());
            PlayerHammer HammerScript = FindObjectOfType<PlayerHammer>();
            HammerScript.UpdateNailList(this);
        }

    }

    private void OnTriggerExit(Collider Collision)
    {

    }
}
