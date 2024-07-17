using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Collider : MonoBehaviour
{
    private bool isPlayerDetected = false;
    public Renderer lightRenderer;
    public Enemy_control e_control;
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
            e_control.enemyAlert(other.transform);
            // �ڽ� ������Ʈ�� ������ ���������� �����մϴ�.
            GetComponent<MeshRenderer>().material.color = Color.red;
            lightRenderer.material.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            // �ڽ� ������Ʈ�� ������ ������� �����մϴ�.
            GetComponent<MeshRenderer>().material.color = initColor;
            lightRenderer.material.color = Color.white;
        }
    }

    public bool IsDetected()
    {
        return isPlayerDetected;
    }
}
