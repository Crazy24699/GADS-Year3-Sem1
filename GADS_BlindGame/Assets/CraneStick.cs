using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneStick : MonoBehaviour
{
    public Transform RotationObject; 
    public float RotationMin; 
    public float RotationMax; 

    public float MinOutputValue = 0f; 
    public float MaxOutputValue = 50f; 
    public float CurrentValue;

    public Vector3 AffectedAxis;

    void Update()
    {
        if (RotationObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }

        float CurrentRotation = RotationObject.eulerAngles.x;
        CurrentRotation = NormalizeAngle(CurrentRotation);

        float ClampedX = Mathf.Clamp(CurrentRotation, RotationMin, RotationMax);
        CurrentValue = MapRotationToRange(CurrentRotation, RotationMin, RotationMax, MinOutputValue, MaxOutputValue);

        transform.eulerAngles = new Vector3(ClampedX, 0, 0);

    }

    float MapRotationToRange(float Rotation, float MinRotation, float MaxRotation, float OutputMin, float OutputMax)
    {

        float ClampedRotation = Mathf.Clamp(Rotation, MinRotation, MaxRotation);
        float RotationNormalised = (ClampedRotation - MinRotation) / (MaxRotation - MinRotation);
        float ReturnValue = Mathf.Lerp(OutputMin, OutputMax, RotationNormalised);

        return ReturnValue;
    }

    float NormalizeAngle(float Angle)
    {
        Angle = Angle % 360;
        if (Angle > 180) Angle -= 360;
        if (Angle < -180) Angle += 360;
        return Angle;
    }

}
