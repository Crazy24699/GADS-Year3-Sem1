using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    protected float rayLength = 100f; // Length of the ray

    protected int FingerHitNum = 0;

    public GameObject SelectedObject;
    protected GameObject HoverObject;       //Where the mouse cursor is hovering over
    public GameObject FeelingHandObject;
    public GameObject HammerObject;
    public GameObject LeftHandObject;
    [SerializeField] protected GameObject InstantFailButton;
    [SerializeField] protected GameObject NextLevelButton;
    [SerializeField] protected GameObject LevelFinishPanel;

    [Space(5), Header(" ")]
    public Camera ViewCamera; // Assign the main camera in the inspector
    [SerializeField] protected LayerMask HoverObjectMask;
    [SerializeField] protected LayerMask NonInteractable;

    
    [Space(5), Header(" ")]
    public bool HandActivated = false;
    public bool BracingNail = false;
    protected bool InteractionActive = true;
    public bool HandViewActive = false;

    [Space(5), Header(" ")]
    public Vector2 Position;
    public Vector3 MousePosition;
    public Vector3 HammerHitPosition = new Vector3(0, 14.64f, 0);

    [Space(5), Header(" ")]
    public Animator HammerAnimation;
    public Animator LeftHandAnim;
    public Animator RightHandAnim;

    [Space(5), Header(" ")]
    public TextMeshProUGUI FingerHitText;


    [SerializeField] protected TextMeshProUGUI EndScreenText;


    public List<Nail> AvailableNails = new List<Nail>();

    public enum PlayerState
    {
        SelectingNail,
        BracingNail,
    }

    public PlayerState CurrentState;

    protected ProgramManager ProgramManagerScript;

    private void Start()
    {
        LevelFinishPanel.SetActive(false);
        FingerHitNum -= 1;
        FingerHit();

        if (FindObjectOfType<ProgramManager>() != null)
        {
            ProgramManagerScript = FindObjectOfType<ProgramManager>();
        }

        AvailableNails = FindObjectsOfType<Nail>().ToList();
    }

    public void UpdateNailList(Nail NailRef)
    {
        AvailableNails.Remove(NailRef);
        if(AvailableNails.Count <= 0)
        {
            LevelFinishPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    void Update()
    {

        if(HandActivated )
        {
            ActivateHand();
        }

        if(Input.GetMouseButtonDown(0) && InteractionActive)
        {

            switch (CurrentState)
            {
                case PlayerState.SelectingNail:
                    SelectObject();
                    break;

                case PlayerState.BracingNail:
                    SwingHammer();
                    break;

                default:
                    break;
            }

        }

        MouseMovement();

        if (SelectedObject != null && SelectedObject.layer != NonInteractable) 
        {
            SelectedObject.transform.position = MousePosition;
            //SelectedObject.transform.position = new Vector3(MousePosition.x, 15, MousePosition.z);
        }

    }

    protected void ActivateHand()
    {
        FeelingHandObject.transform.position = MousePosition;
    }

    private void MouseMovement()
    {
        Vector3 MouseScreenPosition = Input.mousePosition;
        Ray MousePositionRay = ViewCamera.ScreenPointToRay(MouseScreenPosition);


        if (Physics.Raycast(MousePositionRay, out RaycastHit HitInfo, rayLength, HoverObjectMask))
        {
            HoverObject = HitInfo.collider.gameObject;
            MousePosition = HitInfo.point;

        }
    }

    void SelectObject()
    {
        if (ViewCamera == null)
        {
            Debug.LogError("Main Camera is not assigned.");
            return;
        }


        Vector3 CurrentMousePosition = Input.mousePosition;
        Ray RayData = ViewCamera.ScreenPointToRay(CurrentMousePosition);
        Debug.Log("for her");
        GameObject HitObject;
        if (Physics.Raycast(RayData, out RaycastHit HitInfo, rayLength))
        {
            HitObject = HitInfo.collider.gameObject;

            if (HitObject.CompareTag("Interactable") && SelectedObject == null) 
            {
                
                SelectedObject = HitObject;
                
            }
        }
        if (SelectedObject == null)
        {
            return;
        }
        SelectedObject.transform.parent = null;
        SelectedObject.layer = LayerMask.NameToLayer("Not Interactable");
    }

    public void SwingHammer()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray RayData = ViewCamera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(RayData, out RaycastHit HitInfo, rayLength))
        {
            HammerHitPosition = new Vector3(HitInfo.point.x, 14.64f, HitInfo.point.z);
        }
        HammerObject.transform.position = HammerHitPosition;


        HammerAnimation.SetTrigger("Swing Hammer");
    }

    public void NailBraceLogic(Vector3 NailPosition)
    {
        //NailPosition = new Vector3(NailPosition.x, NailPosition.y + 0.15f, NailPosition.z);
        LeftHandAnim.SetTrigger("Brace");
        LeftHandObject.transform.position=NailPosition;
    }

    public void FingerHit()
    {
        FingerHitNum++;
        FingerHitText.text = $"You have hit your fingers {FingerHitNum} times of 6 \n" +
            $"if you hit your finger 6 times you will be found out";

        if (FingerHitNum >= 6)
        {
            LevelFinishPanel.SetActive(true);
            NextLevelButton.SetActive(false);
            InstantFailButton.SetActive(true);
        }
    }

    public void NextLevel()
    {
        ProgramManagerScript.LoadNextLevel();
    }

    public void MainScreen()
    {
        ProgramManagerScript.ReturnToMenu();
    }

}

