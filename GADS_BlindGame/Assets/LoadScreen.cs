using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen LoadScreenScript;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (LoadScreenScript == null)  
        {
            LoadScreenScript = this.GetComponent<LoadScreen>();
            
        }
        else if(LoadScreenScript != null)
        {
            Debug.Log("Tearing me apart");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.L))
        {
            SceneManager.LoadScene("Cement Level");
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Main Menu");

        }
    }
}
