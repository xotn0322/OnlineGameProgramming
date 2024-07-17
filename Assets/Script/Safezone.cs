using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safezone : MonoBehaviour
{
    public GameObject player;
    public Enemy_control e_Controller;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            other.tag = "Untagged";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged"))
            other.tag = "Player";
    }
}
