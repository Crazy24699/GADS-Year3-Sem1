using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    public LevelType CurrentType;
    public BlindLevel CurrentBlindness;
    public static ProgramManager ManagerInstance;

    public List<int> Sequence1 = new List<int>();
    public List<int> Sequence2 = new List<int>();
    public List<int> Sequence3 = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if( ManagerInstance == null)
        {
            ManagerInstance = this;
        }
        else if( ManagerInstance != null)
        {
            Destroy(this.gameObject);
        }
    }



}

public enum LevelType
{
    Cement,
    Hammering,
    Crane
}

public enum BlindLevel
{
    Floaters,
    Fading,
    Complete
}
