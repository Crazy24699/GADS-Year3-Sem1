using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankInteraction : MonoBehaviour
{

    public Vector3[] NailLocations;

    public GameObject[] NailPointVisualisers;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NailPointVisualisers.Length; i++)
        {
            NailPointVisualisers[i].transform.localPosition = NailLocations[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
