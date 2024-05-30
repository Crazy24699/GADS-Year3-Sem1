using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{

    public ProgramManager ProgramScript;
    public int BlindLevels = 3;

    // Start is called before the first frame update
    void Start()
    {
        ProgramScript = FindObjectOfType<ProgramManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        ProgramManager.ManagerInstance.LoadNextLevel();
    }

}
