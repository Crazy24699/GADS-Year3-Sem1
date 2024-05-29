using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookObject : MonoBehaviour
{

    public GameObject HookLocation;

    public bool Attatched;
    public bool Delivered;

    // Start is called before the first frame update
    void Start()
    {
        if (HookLocation == null)
        {
            HookLocation = transform.Find("Hook Point").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
