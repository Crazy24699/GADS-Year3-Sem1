using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlankInteraction : MonoBehaviour
{

    public Vector3[] NailLocations;
    [SerializeField]protected int UsedPositions = 0;
    public GameObject[] NailPointVisualisers;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NailPointVisualisers.Length; i++)
        {
            NailPointVisualisers[i].transform.localPosition = NailLocations[UsedPositions];
            NailOutliner OutlinerScript = NailPointVisualisers[i].GetComponent<NailOutliner>();

            OutlinerScript.OutlinerIndexNum = i;
            OutlinerScript.PlankScript = this;
            OutlinerScript.Startup();

            UsedPositions++;
        }

        

    }

    public void UpdatePositions(int NailIndex, GameObject OutlinerRef)
    {
        if (UsedPositions < NailLocations.Length) 
        {
            NailPointVisualisers[NailIndex].transform.localPosition = NailLocations[UsedPositions];
            UsedPositions++;
        }
        else
        {
            OutlinerRef.SetActive(false);
        }

    }
}
