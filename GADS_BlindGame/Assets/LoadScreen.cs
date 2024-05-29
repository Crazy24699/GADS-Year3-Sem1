using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public Material Blackout;

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
            Destroy(this.gameObject);
        }
        ChangeMaterials();
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

    public void ChangeMaterials()
    {
        List<MeshRenderer> AllRenderers = new List<MeshRenderer>();
        AllRenderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.InstanceID).ToList();
        foreach (var Renderer in AllRenderers)
        {
            Renderer.material = Blackout;
        }
    }

}
