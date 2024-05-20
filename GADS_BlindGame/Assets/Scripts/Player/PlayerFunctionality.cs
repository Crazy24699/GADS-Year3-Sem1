using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFunctionality : MonoBehaviour
{

    public GameObject CurrentContactObject;
    public List<Vector3> CurrentObjectVertices = new List<Vector3>();


    void Start()
    {
        
    }

    public List<Vector3> PopulateObjectVertices()
    {
        if(CurrentContactObject == null)
        {
            return null;
        }

        HashSet<Vector3> FoundVertices = new HashSet<Vector3>();
        FaceData FaceDataRef = CurrentContactObject.GetComponent<FaceData>();
        FoundVertices = FaceDataRef.FoundVertices.ToHashSet();


        return CurrentObjectVertices = FoundVertices.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
