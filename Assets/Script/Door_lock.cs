using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_lock : MonoBehaviour
{
    public static Door_lock instance;

    public int getKey;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if(getKey == 4)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (getKey == 4)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    UI_End.instance.Clear();
                }
            }
        }
    }
}
