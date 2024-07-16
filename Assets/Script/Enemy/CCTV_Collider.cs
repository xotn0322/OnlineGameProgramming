using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Collider : MonoBehaviour
{
    private bool isPlayerDetected = false;
    public Renderer lightRenderer;
    private Color initColor;

    private void Start()
    {
        initColor = GetComponent<MeshRenderer>().material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
            // 자식 오브젝트의 색상을 빨간색으로 변경합니다.
            GetComponent<MeshRenderer>().material.color = Color.red;
            lightRenderer.material.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            // 자식 오브젝트의 색상을 원래대로 변경합니다.
            GetComponent<MeshRenderer>().material.color = initColor;
            lightRenderer.material.color = Color.white;
        }
    }

    public bool IsDetected()
    {
        return isPlayerDetected;
    }
}
