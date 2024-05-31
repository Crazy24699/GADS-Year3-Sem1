using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NailOutliner : MonoBehaviour
{
    public int OutlinerIndexNum;

    public PlankInteraction PlankScript;
    protected PlayerHammer PlayerHammerScript;

    public void Startup()
    {
        PlayerHammerScript = FindObjectOfType<PlayerHammer>();
        Debug.Log(this.transform.TransformPoint(this.transform.position));
    }

    public bool HasNail;

    protected void OnTriggerEnter(Collider Collision)
    {
        if (Collision.CompareTag("Interactable") && Collision.name.Contains("Nail") && PlayerHammerScript.HitHand == false)  
        {
            PlayerHammerScript.SelectedObject = null;

            Collision.gameObject.layer = LayerMask.NameToLayer("Not Interactable");

            //Collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.parent = null;

            Collision.gameObject.transform.position = transform.position;
            this.transform.parent = PlankScript.gameObject.transform;
            PlayerHammerScript.CurrentState = PlayerHammer.PlayerState.BracingNail;
            PlayerHammerScript.NailBraceLogic(Collision.gameObject.transform.position);
            Debug.Log(Collision.name);

            Debug.Log(PlayerHammerScript.HitHand);

            //StartCoroutine(LogicDelay(Collision.gameObject));
        }

        if (Collision.CompareTag("Hammer"))
        {

        }

    }


    public void ChangeOutliner()
    {
        PlayerHammerScript.CurrentState = PlayerHammer.PlayerState.SelectingNail;
        PlankScript.UpdatePositions(OutlinerIndexNum, this.gameObject);
    }
}
