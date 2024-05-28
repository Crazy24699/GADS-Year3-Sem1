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

    public float RotationTime;

    private void Start()
    {
        ViewingCamera = FindObjectOfType<Camera>();
        ViewingCamera.transform.rotation = Quaternion.Euler(StartingCameraRotation);


        XRotation = StartingCameraRotation.x;
        YRotation = StartingCameraRotation.y;
    }

    public void RotateCamera()
    {
        if (Input.GetKey(KeyCode.W) && XRotation>UpperLimit.x)
        {
            XRotation = XRotation -= 0.035f;
            //XRotation = Mathf.Lerp(XRotation, LowerLimit.x, 0.0005f);
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
    }

}
