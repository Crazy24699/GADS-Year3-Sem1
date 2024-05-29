using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneStick : MonoBehaviour
{
    public Transform targetObject; // The object whose rotation you want to map
    public float minRotation; // Minimum rotation value (in degrees)
    public float maxRotation; // Maximum rotation value (in degrees)
    public float outputMin = 0f; // Minimum output value
    public float outputMax = 50f; // Maximum output value

    void Update()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }

        float currentRotation = targetObject.eulerAngles.x;
        currentRotation = NormalizeAngle(currentRotation);

        float ClampedX = Mathf.Clamp(currentRotation, minRotation, maxRotation);

        float mappedValue = MapRotationToRange(currentRotation, minRotation, maxRotation, outputMin, outputMax);

        //Debug.Log("Mapped Value: " + mappedValue);
        transform.eulerAngles = new Vector3(ClampedX, 0, 0);

    }

    float MapRotationToRange(float rotation, float minRotation, float maxRotation, float outputMin, float outputMax)
    {


        float clampedRotation = Mathf.Clamp(rotation, minRotation, maxRotation);

        float normalizedRotation = (clampedRotation - minRotation) / (maxRotation - minRotation);

        float mappedValue = Mathf.Lerp(outputMin, outputMax, normalizedRotation);

        return mappedValue;
    }


    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        return angle;
    }

}
