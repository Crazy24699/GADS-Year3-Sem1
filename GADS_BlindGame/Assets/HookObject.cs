using UnityEngine;

public class HookObject : MonoBehaviour
{

    public GameObject HookLocation;

    public PlayerCrane CraneScript;

    public bool Attached;
    public bool Delivered;

    // Start is called before the first frame update
    void Start()
    {
        if (HookLocation == null)
        {
            HookLocation = transform.Find("Hook Point").gameObject;
        }
        CraneScript = FindObjectOfType<PlayerCrane>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider Collider)
    {
        if (Collider.CompareTag("Jib"))
        {
            transform.SetParent(Collider.transform);
            Attached = true;
        }

        if(Attached && Collider.CompareTag("Destination"))
        {
            Collider.transform.SetParent(null); 
            HookLocation.tag = "Non Interactable";
            this.transform.position = Collider.transform.position;
            CraneScript.EndGame(false);
            CraneScript.EndScreenText.text = "You successfully devlivered the materials to the workers that needed them";
            Attached = false;
        }

        if(Attached && Collider.CompareTag("workers"))
        {
            CraneScript.EndGame(true);
            CraneScript.EndScreenText.text = "You have hit one of your fellow workers and killed him, you are being tried for manslaughter";
        }

    }
}
