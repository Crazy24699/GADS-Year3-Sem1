using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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
    [SerializeField] protected GameObject LoadingScreen;
    [Space(5), Header("")]
    [SerializeField] protected GameObject JibObject;

    [SerializeField]private Vector3 initialMousePosition;
    [SerializeField] private Vector3 CurrentMousePosition;
    public Vector3 LowerBounds;
    public Vector3 UpperBounds;

    public CraneStick SelectedCraneStick;

    public TextMeshProUGUI EndScreenText;
    protected ProgramManager ProgramManagerScript;

    public bool Tracking;

    public Camera ViewingCamera;

    public Vector2 ScreenMiddleCords;
    public float ScreenHeight;
    public float ScreenWidth;

    public float SpeedModifier = 0.0f;

    public LayerMask SelectableLayers;

    public bool HandViewActive = false;
    protected bool InteractionActive = true;
    [SerializeField] protected GameObject HandObject;

    // Start is called before the first frame update
    void Start()
    {
        YRotation = transform.rotation.y;



        if (FindObjectOfType<ProgramManager>() != null)
        {
            ProgramManagerScript = FindObjectOfType<ProgramManager>();
        }
        StartCoroutine(LoadingScreenTimes());
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
        ModifySpeed();
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            switch (HandViewActive)
            {
                case false:
                    HandViewActive = true;
                    InteractionActive = false;
                    break;

                case true:
                    HandViewActive = false;
                    InteractionActive = true;
                    break;
            }
        }

        if (HandViewActive)
        {
            HandFunctionality();
        }

        if (Input.GetMouseButtonDown(1) && SelectedCraneStick != null) 
        {
            Tracking = false;
            SelectedCraneStick = null;
        }

        if(Tracking && SelectedCraneStick != null) 
        {
            SelectedCraneStick.CurrentStickRotationX += YValueChange;
            SelectedCraneStick.gameObject.transform.rotation = Quaternion.Euler(SelectedCraneStick.CurrentStickRotationX,0,0);
            TrackMouseChange();
            MoveJib();
        }
    }

    public void ModifySpeed()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpeedModifier += 0.005f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpeedModifier -= 0.005f;
        }
    }

    protected void RotateCrane()
    {
        if (Input.GetKey(KeyCode.D))
        {
            YRotation += 0.025f + SpeedModifier;
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            YRotation -= 0.025f - SpeedModifier;
            
        }
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, YRotation, transform.localRotation.z);
    }

    protected void HandFunctionality()
    {
        // Create a ray from the camera through the mouse position
        Ray RayCast = ViewingCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;

        // Perform the raycast and check if it hits a collider on the specified layer
        if (Physics.Raycast(RayCast, out HitInfo, 100, SelectableLayers))
        {
            // Get the point in world space where the ray hit
            Vector3 HitPoint = HitInfo.point;

            // Log the world position
            HandObject.transform.position = HitPoint;
        }


    }

    public void MoveJib()
    {
        Vector3 JibLocalPosition = JibObject.transform.localPosition;
        if (JibLocalPosition.y > UpperBounds.y) 
        {
            JibObject.transform.localPosition = new Vector3(JibLocalPosition.x, UpperBounds.y, JibLocalPosition.z);
            return;
        }
        if (JibLocalPosition.y < LowerBounds.y)
        {
            JibObject.transform.localPosition = new Vector3(JibLocalPosition.x, LowerBounds.y, JibLocalPosition.z);
            return;
        }

        if (JibLocalPosition.z > UpperBounds.z)
        {
            JibObject.transform.localPosition = new Vector3(JibLocalPosition.x, JibLocalPosition.y, UpperBounds.z);
            return;
        }
        if (JibLocalPosition.z < LowerBounds.z)
        {
            JibObject.transform.localPosition = new Vector3(JibLocalPosition.x, JibLocalPosition.y, LowerBounds.z);
            return;
        }

        JibObject.transform.localPosition += (SelectedCraneStick.AffectedAxis * SelectedCraneStick.CurrentValue)*2 * Time.deltaTime ;
    }

    public void NextLevel()
    {
        ProgramManagerScript.LoadNextLevel();
    }

    public void MainScreen()
    {
        ProgramManagerScript.ReturnToMenu();
    }

    public void EndGame(bool Failed)
    {
        switch (Failed)
        {
            case true:
                NextLevelButton.SetActive(false);
                InstantFailButton.SetActive(true);
                break;

            case false:
                InstantFailButton.SetActive(false);
                break;
        }
        LevelFinishPanel.SetActive(true);
    }

    //private void OnCollisionEnter(Collision CollidedObject)
    //{
    //    Debug.Log(CollidedObject.gameObject.name);
    //    if (CollidedObject.collider.CompareTag("Environment"))
    //    {
    //        Debug.Log("restor");
    //        EndGame(true);
    //        EndScreenText.text = "You have hit the side of the construction zone and as such have caused major damage to the area, you are being sued for negligance";
    //    }
    //}

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.CompareTag("Environment"))
        {
            //Debug.Log("restor");
            EndGame(true);
            EndScreenText.text = "You have hit the side of the construction zone and as such have caused major damage to the area, you are being sued for negligance";
        }
    }
    public IEnumerator LoadingScreenTimes()
    {
        yield return new WaitForSeconds(1.65f);
        if (LoadingScreen.activeSelf)
        {
            LoadingScreen.SetActive(false);
        }
    }
}
