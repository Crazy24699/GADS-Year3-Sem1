using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrane : MonoBehaviour
{

    [SerializeField] private float YRotation;
    [SerializeField] private float YValueChange;
    [SerializeField] private float RayLength;
    //[SerializeField] private float XRotation;

    [SerializeField] protected GameObject InstantFailButton;
    [SerializeField] protected GameObject NextLevelButton;
    [SerializeField] protected GameObject LevelFinishPanel;
    [Space(5), Header("")]
    [SerializeField] protected GameObject JibObject;

    [SerializeField]private Vector3 initialMousePosition;
    [SerializeField] private Vector3 CurrentMousePosition;

    public CraneStick SelectedCraneStick;

    protected ProgramManager ProgramManagerScript;

    public bool Tracking;

    public Camera ViewingCamera;

    // Start is called before the first frame update
    void Start()
    {
        YRotation = transform.rotation.y;



        if (FindObjectOfType<ProgramManager>() != null)
        {
            ProgramManagerScript = FindObjectOfType<ProgramManager>();
        }

    }

    public void TrackMouseChange()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        CurrentMousePosition = currentMousePosition;
        float MouseYValue = (currentMousePosition.y - initialMousePosition.y) / 50;

        if (MouseYValue > 0)
        {
            YValueChange = MouseYValue;
        }
        else if (MouseYValue < 0)
        {
            YValueChange = MouseYValue;
        }

        if (Mathf.Abs(YValueChange) > 10)
        {
            YValueChange = (YValueChange * 10) / YValueChange;
        }

    }

    // Update is called once per frame
    void Update()
    {
        RotateCrane();

        if(Input.GetMouseButtonDown(0))
        {
            Ray RayData = ViewingCamera.ScreenPointToRay(Input.mousePosition);
            GameObject HitObject;
            if(Physics.Raycast(RayData,out RaycastHit HitInfo, RayLength))
            {
                HitObject = HitInfo.collider.gameObject;

                if (HitObject.CompareTag("Interactable"))
                {
                    SelectedCraneStick = HitObject.GetComponent<CraneStick>();
                    initialMousePosition = Input.mousePosition;
                    Tracking = true;
                }
            }


        }

        if(Tracking)
        {

            TrackMouseChange();
        }
    }

    protected void RotateCrane()
    {
        if (Input.GetKey(KeyCode.D))
        {
            YRotation += 0.045f;
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            YRotation -= 0.045f;
            
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, YRotation, transform.rotation.z);
    }

    public void MoveJib()
    {

    }

    public void NextLevel()
    {
        ProgramManagerScript.LoadNextLevel();
    }

    public void MainScreen()
    {
        ProgramManagerScript.ReturnToMenu();
    }

    private void OnCollisionEnter(Collision CollidedObject)
    {
        if (CollidedObject.collider.CompareTag("Environment"))
        {
            LevelFinishPanel.SetActive(true);
            Debug.Log("fuck");
        }
    }

}
