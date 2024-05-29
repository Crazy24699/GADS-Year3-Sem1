using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerLogic : MonoBehaviour
{
    [SerializeField]protected TextMeshProUGUI WeightText;
    public GameObject CurrentPlacedObject;

    protected PlayerCement PlayerCementLogic;

    public float CorrectIngrediantWeight;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCementLogic = FindObjectOfType<PlayerCement>();

        WeightText.text = "No Material Detected";
    }

    public void UpdateInfo(string UpdateText)
    {
        WeightText.text = UpdateText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider Collision)
    {
        Debug.Log("The shadows");
        if (Collision.CompareTag("Interactable") && Collision.name.Contains("bag") && CurrentPlacedObject == null) 
        {
            CurrentPlacedObject = Collision.gameObject;
            PlayerCementLogic.PlacedObject = CurrentPlacedObject;

            CementBags CurrentBag = CurrentPlacedObject.GetComponent<CementBags>();
            string Name = CurrentBag.IngrediantName;
            float CurrentWeight = CurrentBag.ObjectWeight;
            CorrectIngrediantWeight = PlayerCementLogic.CementIngrediantsClass[PlayerCementLogic.IngrediantIndex].Weight;
            float DesiredWeight = CorrectIngrediantWeight;
            string TextUpdate = $"Current Bag: {Name} \n Weight: {CurrentWeight}/ {DesiredWeight}";

            UpdateInfo(TextUpdate);
        }
    }
    private void OnTriggerEnter(Collider Collision)
    {

    }

    private void OnTriggerExit(Collider Collision)
    {
        if (Collision.CompareTag("Interactable") && Collision.name.Contains("emen"))
        {
            CurrentPlacedObject = null;
            PlayerCementLogic.PlacedObject = null;
        }
    }
}
