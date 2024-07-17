using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_control : MonoBehaviour
{
    public GameObject[] Enemys;

    public void enemyAlert(Transform target)
    {
        foreach(GameObject enemy in Enemys)
        {
            enemy.GetComponent<Enemy_move>().setDestination(target);
            enemy.GetComponent<Enemy_move>().ChangeState(Enemy_move.State.ALERT);
        }
    }
}
