using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class CementBags : MonoBehaviour
{
    public GameObject WeightTable;
    protected ComputerLogic ComputerLogicScript;

    public float ObjectWeight;
    public float MaxObjectWeight;
    public float MinObjectWeight;

    [SerializeField] private bool CanChangeWeight = true;
    public bool PositionedCorrectly = false;

    public Vector3 CurrentRotation;

    public string IngrediantName;

    public int MaterialIndex;

    private void Start()
    {
        ComputerLogicScript = FindObjectOfType<ComputerLogic>();
        
    }

    public void RunInteraction()
    {
        CementInteraction();
    }

    public void HandleRotation(float RotationChange)
    {
        float NewYRotation = CurrentRotation.y + RotationChange;
        this.gameObject.transform.rotation = Quaternion.Euler(CurrentRotation.x, NewYRotation, CurrentRotation.z);
    }

    private void Update()
    {
        CurrentRotation = this.gameObject.transform.rotation.eulerAngles;
    }

    public void HandleWeight(float WeightChange)
    {
        //Debug.Log("Blood");
        if (CanChangeWeight)
        {

            ObjectWeight += WeightChange;
            StartCoroutine(AddWeightCooldown());

            string TextUpdate = $"Current Bag: {IngrediantName} \n Weight: {ObjectWeight} " +
                $"/ {ComputerLogicScript.CorrectIngrediantWeight}";

            Debug.Log(TextUpdate);
            ComputerLogicScript.UpdateInfo(TextUpdate);
        }
    }

    public IEnumerator AddWeightCooldown()
    {
        CanChangeWeight = false;
        yield return new WaitForSeconds(0.5f);
        CanChangeWeight = true;
    }

    public void CementInteraction()
    {
        int RandomRotation = Random.Range(0, 4);
        int SetRotation = 0;
        switch (RandomRotation)
        {
            case 0:
                SetRotation = 0;
                break;

            case 1:
                SetRotation = 90;
                break;

            case 2:
                SetRotation = 180;
                break;

            case 3:
                SetRotation = 270;
                break;
        }
        this.transform.position = WeightTable.transform.position;
        this.transform.rotation = Quaternion.Euler(0, SetRotation, 90);

        //this.gameObject.tag = "Non Interactable";
    }

    public bool CheckIngredients(string ComparingName, float Weight)
    {
        bool Matches = false;

        if(ComparingName==IngrediantName && ObjectWeight == Weight)
        {

            Matches = true;
        }
        Debug.Log(Matches+"     "+ComparingName+"       "+Weight); 
        return Matches;
    }

    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.CompareTag("Interactable") && Collision.name.Contains("Mixer"))
        {
            PositionedCorrectly = true;
        }

    }
    private void OnTriggerExit(Collider Collision)
    {
        if (Collision.CompareTag("Interactable") && Collision.name.Contains("Mixer"))
        {
            PositionedCorrectly = false;
        }
    }
}
