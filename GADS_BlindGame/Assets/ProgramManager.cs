using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramManager : MonoBehaviour
{
    public LevelType CurrentType;
    public BlindLevel CurrentBlindness;
    public static ProgramManager ManagerInstance;

    public List<int> Sequence1;
    public List<int> Sequence2 = new List<int>();
    public List<int> Sequence3 = new List<int>();

    public List<int> CurrentSequence = new List<int>();
    [SerializeField]protected int CurrentSequenceList = 0;

    bool Finished = false;

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
        SetNewSequence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadNextLevel();
        }
        if(CurrentSequenceList>=3 && CurrentSequence.Count == 0)
        {
            Finished = true;
        }
    }

    public void SetNewSequence()
    {
        if (CurrentSequenceList >= 3)
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }
        switch (CurrentSequenceList)
        {
            case 0:
                CurrentSequence = Sequence1.ToList();
                break;

            case 1:
                CurrentSequence = Sequence2.ToList();
                break;

            case 2:
                CurrentSequence = Sequence3.ToList();
                break;
        }

        CurrentSequenceList++;
    }

    public void LoadNextLevel()
    {
        if (CurrentSequence.Count <= 0)
        {
            SetNewSequence();
        }
        if(CurrentSequenceList > 3 || Finished)
        {
            return;
        }
        int LevelNum = CurrentSequence[0];
        switch (LevelNum)
        {
            case 0:
                SceneManager.LoadScene("Cement Level");
                break;

            case 1:
                SceneManager.LoadScene("Hammer Level");
                break;

            case 2:
                SceneManager.LoadScene("Crane Level");
                break;
        }
        CurrentSequence.RemoveAt(0);
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
