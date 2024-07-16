using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_CCTV_Controller : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float maxAngle = 45f;
    public GameObject rader;

    private float currentRotation = 0f;
    private Vector3 initRotaion;
    private bool rotatingClockwise = true;


    private void Start()
    {
        initRotaion = transform.localRotation.eulerAngles;
    }
    void Update()
    {
        if (!rader.GetComponent<CCTV_Collider>().IsDetected())
        {
            RotateCCTV();
        }
    }

    void RotateCCTV()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;

        if (rotatingClockwise)
        {
            currentRotation += rotationStep;
            if (currentRotation > maxAngle)
            {
                currentRotation = maxAngle;
                rotatingClockwise = false;
            }
        }
        else
        {
            currentRotation -= rotationStep;
            if (currentRotation < -maxAngle)
            {
                currentRotation = -maxAngle;
                rotatingClockwise = true;
            }
        }

        transform.localRotation = Quaternion.Euler(initRotaion.x, initRotaion.y + currentRotation, initRotaion.z);
    }
}
