using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject LoadingScreen;
    public GameObject BlindSpotRef;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(true);
        }
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
        VisualiseBlindness();
        StartCoroutine(LoadingScreenDelay());
        CurrentSequence.RemoveAt(0);
    }

    public void VisualiseBlindness()
    {
        switch (CurrentSequenceList)
        {
            default:
            case 1:
                break;

            case 2:
                StartCoroutine(ActivateColour());
                break;

            case 3:
                StartCoroutine(LoadingScreenDelay());
                break;
        }
    }

    public IEnumerator ActivateColour()
    {
        yield return new WaitForSeconds(0.25f);
        GameObject BlindSpotObject = GameObject.FindGameObjectWithTag("Player Canvas").transform.Find("Blind Spot").gameObject;
        Debug.Log(BlindSpotObject.name);
        BlindSpotRef = BlindSpotObject;
        Image BlindSpotColor = BlindSpotObject.GetComponent<Image>();
        BlindSpotColor.enabled = true;
    }

    public IEnumerator LoadingScreenDelay()
    {
        yield return new WaitForSeconds(0.125f);
        LoadingScreen = GameObject.FindGameObjectWithTag("Player Canvas").transform.Find("LoadingScreenPanel").gameObject;
        LoadingScreen.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        if (CurrentSequenceList==3)
        {
            LoadScreen.LoadScreenScript.ChangeMaterials();
        }
        yield return new WaitForSeconds(1.5f);
        LoadingScreen.SetActive(false);
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
