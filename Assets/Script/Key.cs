using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Door_lock.instance.getKey += 1;
        Destroy(gameObject);
    }
}
