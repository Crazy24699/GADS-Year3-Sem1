using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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

    [Space(5)]
    [SerializeField] private float RayLength;

    public float RotationTime;

    public CementIngrediants CurrentChosenIngrediant;
    public CementIngrediants[] CementIngrediantsClass;
    protected ComputerLogic ComputerLogicScript;

    public GameObject PlacedObject;

    public bool Failed;

    public int IngrediantIndex = 0;

    private void Start()
    {
        
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
    }

    public void RotateCamera()
    {
        if (Input.GetKey(KeyCode.W) && XRotation>UpperLimit.x)
        {
            XRotation = XRotation -= 0.035f;

        }
        if (Input.GetKey(KeyCode.S) && XRotation<LowerLimit.x)
        {
            XRotation = XRotation += 0.035f;
        }
        if (Input.GetKey(KeyCode.A) && YRotation > UpperLimit.y) 
        {
            YRotation = YRotation -= 0.035f;
        }
        if (Input.GetKey(KeyCode.D) && YRotation < LowerLimit.y) 
        {
            YRotation = YRotation += 0.035f;
        }
        ViewingCamera.gameObject.transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
    }

    private void Update()
    {
        RotateCamera();

        if(Input.GetMouseButton(0))
        {
            MouseInteraction();
        }
    }

    public void MouseInteraction()
    {
        Vector3 MousePosition = Input.mousePosition;
        Ray RayData = ViewingCamera.ScreenPointToRay(MousePosition);

        GameObject HitObject;

        if(Physics.Raycast(RayData, out RaycastHit HitInfo, RayLength))
        {
            HitObject = HitInfo.collider.gameObject;

            if (HitObject.CompareTag("Interactable") && PlacedObject == null) 
            {
                HitObject.GetComponent<CementBags>().RunInteraction();
                PlacedObject = HitObject;
                return;
            }
            if(!HitObject.CompareTag("Interactable") && PlacedObject == null)
            {
                return;
            }
            bool CorrectValues = false;
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
                        Debug.Log("take a stand");
                        if (Bag.Name == CementBags.IngrediantName)
                        {
                            CorrectValues = CementBags.CheckIngredients(Bag.Name, Bag.Weight);
                            HandleBagLogic(CorrectValues);
                        }
                    }
                    break;
            }


        }

    }


    public void HandleBagLogic(bool CorrectValues)
    {
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
                break;

        }

        string NextIngrediant = CementIngrediantsClass[IngrediantIndex].Name;
        ComputerLogicScript.UpdateInfo($"Place next bag: {NextIngrediant}");

        ComputerLogicScript.CorrectIngrediantWeight = CementIngrediantsClass[IngrediantIndex].Weight;
        Destroy(PlacedObject);
    }

    public void EndGame()
    {

    }

}

[System.Serializable]
public class CementIngrediants
{
    public string Name;

    public int IngrediantIndex;

    public float Weight;
}
