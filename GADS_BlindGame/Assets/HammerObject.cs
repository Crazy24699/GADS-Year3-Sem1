using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerObject : MonoBehaviour
{
    private PlayerHammer PlayerHammerScript;

    private void Start()
    {
        PlayerHammerScript = FindObjectOfType<PlayerHammer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            Debug.Log("Hit the finger biiiiiitch");
            StartCoroutine(ColliderDisable());
        }
        
    }

    public IEnumerator ColliderDisable()
    {
        PlayerHammerScript.FingerHit();
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<Collider>().enabled = true;
    }
}
