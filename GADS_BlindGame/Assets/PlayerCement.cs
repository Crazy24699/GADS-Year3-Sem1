using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCement : MonoBehaviour
{

    public Camera ViewingCamera;

    private Vector3 CurrentCameraRotation;
    [SerializeField]protected Vector3 StartingCameraRotation;

    private Vector3 UpperLimit=new Vector3(15f, -60); //upper and left limit
    private Vector3 LowerLimit=new Vector3(45,55); //lower and right limit

    [SerializeField] private float YRotation;
    [SerializeField] private float XRotation;

    public float SpeedModifier = 0.0f;
    [Space(5)]
    [SerializeField] private float RayLength;

    public float RotationTime;

    public CementIngrediants CurrentChosenIngrediant;
    public CementIngrediants[] CementIngrediantsClass;
    protected ComputerLogic ComputerLogicScript;
    protected ProgramManager ProgramManagerScript;

    public GameObject PlacedObject;
    public GameObject LevelFinishPanel;
    [SerializeField] protected GameObject InstantFailButton;
    [SerializeField] protected GameObject NextLevelButton;
    [SerializeField] protected GameObject HandObject;
    [SerializeField] protected GameObject LoadingScreen;

    [SerializeField] protected TextMeshProUGUI EndScreenText;

    public bool Failed;
    protected bool InteractionActive = true;
    public bool HandViewActive = false;

    public int IngrediantIndex = 0;
    public int IncorrectBags = 0;

    public Vector2 ScreenMiddleCords;
    public float ScreenHeight;
    public float ScreenWidth;

    public LayerMask SelectableLayers;
    public LayerMask ViewableLayers;


    private void Start()
    {
        //
        YRotation = transform.rotation.y;

        ScreenHeight = Screen.height;
        ScreenWidth = Screen.width;
        ScreenMiddleCords = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
        //

        LevelFinishPanel.SetActive(false);
        ViewingCamera = FindObjectOfType<Camera>();
        ViewingCamera.transform.rotation = Quaternion.Euler(StartingCameraRotation);
        ComputerLogicScript = FindObjectOfType<ComputerLogic>();

        List<CementBags> AllBags = new List<CementBags>();
        AllBags = FindObjectsByType<CementBags>(FindObjectsSortMode.InstanceID).ToListPooled();

        for (int i = 0; i < AllBags.Count; i++)
        {
            foreach(var Ingrediant in CementIngrediantsClass)
            {
                if (Ingrediant.Name == AllBags[i].IngrediantName)
                {
                    AllBags[i].MaterialIndex = Ingrediant.IngrediantIndex;
                }
            }
            
        }
        string NextIngrediant = CementIngrediantsClass[IngrediantIndex].Name;
        ComputerLogicScript.UpdateInfo($"Place next bag: {NextIngrediant}");
        XRotation = StartingCameraRotation.x;
        YRotation = StartingCameraRotation.y;

        if (FindObjectOfType<ProgramManager>() != null)
        {
            ProgramManagerScript = FindObjectOfType<ProgramManager>();
        }

        StartCoroutine(LoadingScreenTimes());
    }

    public void RotateCamera()
    {
        if (Input.GetKey(KeyCode.W) && XRotation>UpperLimit.x)
        {
            XRotation = XRotation -= 0.035f -SpeedModifier*-1;

        }
        if (Input.GetKey(KeyCode.S) && XRotation<LowerLimit.x)
        {
            XRotation = XRotation += 0.035f + SpeedModifier;
        }
        if (Input.GetKey(KeyCode.A) && YRotation > UpperLimit.y) 
        {
            YRotation = YRotation -= 0.035f - SpeedModifier*-1;
        }
        if (Input.GetKey(KeyCode.D) && YRotation < LowerLimit.y) 
        {
            YRotation = YRotation += 0.035f + SpeedModifier;
        }
        ViewingCamera.gameObject.transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
    }

    private void Update()
    {
        ModifySpeed();
        RotateCamera();

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

        if (Input.GetMouseButton(0) && InteractionActive) 
        {
            MouseInteraction();
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

    protected void HandFunctionality()
    {
        // Create a ray from the camera through the mouse position
        Ray RayCast = ViewingCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;

        // Perform the raycast and check if it hits a collider on the specified layer
        if (Physics.Raycast(RayCast, out HitInfo, 100))
        {
            // Get the point in world space where the ray hit
            Vector3 HitPoint = HitInfo.point;

            HandObject.transform.position = HitPoint;
        }

        
    }



    public void MouseInteraction()
    {
        Vector3 MousePosition = Input.mousePosition;
        Ray RayData = ViewingCamera.ScreenPointToRay(MousePosition);

        GameObject HitObject;

        if(Physics.Raycast(RayData, out RaycastHit HitInfo, RayLength, SelectableLayers))
        {
            HitObject = HitInfo.collider.gameObject;

            if (HitObject.CompareTag("Interactable") && PlacedObject == null) 
            {
                Debug.Log(HitObject.name);
                HitObject.GetComponent<CementBags>().RunInteraction();
                PlacedObject = HitObject;
                return;
            }
            if(!HitObject.CompareTag("Interactable") && PlacedObject == null)
            {
                return;
            }
            bool CorrectValues;
            CementBags CementBags = PlacedObject.GetComponent<CementBags>();
            switch (HitObject.name)
            {
                case "Less Weight":
                    CementBags.HandleWeight(-0.5f);
                    break;

                case "More Weight":
                    CementBags.HandleWeight(0.5f);

                    break;

                case "Rotate Right":
                    CementBags.HandleRotation(+0.045f);
                    break;

                case "Rotate Left":
                    CementBags.HandleRotation(-0.045f);
                    break;

                case "Confirm Button":
                    foreach (var Bag in CementIngrediantsClass)
                    {
                        Debug.Log("take a stand"+Bag.Name);
                        if (Bag.Name == CementBags.IngrediantName)
                        {
                            CorrectValues = CementBags.CheckIngredients(Bag.Name, Bag.Weight);
                            HandleBagLogic(CorrectValues);
                            break;
                        }
                    }
                    break;
            }


        }

    }



    public void HandleBagLogic(bool CorrectValues)
    {
        Debug.Log("Run");
        IngrediantIndex++;
        if (IngrediantIndex >= 3)
        {
            EndGame();
            return;
        }

        switch (CorrectValues)
        {
            case true:
                Failed = false;
                break;

            case false:
                Failed = true;
                IncorrectBags++;
                break;

        }

        string NextIngrediant = CementIngrediantsClass[IngrediantIndex].Name;
        ComputerLogicScript.UpdateInfo($"Place next bag: {NextIngrediant}");

        ComputerLogicScript.CorrectIngrediantWeight = CementIngrediantsClass[IngrediantIndex].Weight;
        Destroy(PlacedObject);
        
    }

    public void EndGame()
    {
        InteractionActive = false;
        Time.timeScale = 0;
        LevelFinishPanel.SetActive(true);

        if(IncorrectBags>0 &&  IncorrectBags < 3)
        {
            EndScreenText.text = $"You used {IncorrectBags} Incorrect material or messurements, the next day the construction team comes on sight, as they are working" +
                $"a wall falls on them and kills 3, the fault found to be incorrectly mixed materials. \n You were not suspected";

        }
        else if (IncorrectBags == 3)
        {
            EndScreenText.text = "You incorrectly mixed every bag and the mistake was instantyl seen, as such have been fired and you are currently being sued for endagering life and neglagence ";
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

    public IEnumerator LoadingScreenTimes()
    {
        yield return new WaitForSeconds(1.65f);
        if (LoadingScreen.activeSelf)
        {
            LoadingScreen.SetActive(false);
        }
    }

}

[System.Serializable]
public class CementIngrediants
{
    public string Name;

    public int IngrediantIndex;

    public float Weight;
}
