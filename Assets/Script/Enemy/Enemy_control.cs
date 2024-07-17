using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_control : MonoBehaviour
{
    public GameObject[] Enemys;

    public void enemyAlert(Transform target)
    {
        if (Enemys.Length > 0)
        {
            foreach (GameObject enemy in Enemys)
            {
                enemy.GetComponent<Enemy_move>().setDestination(target);
                enemy.GetComponent<Enemy_move>().ChangeState(Enemy_move.State.ALERT);
            }
        }
    }

    public void safe()
    {
        if (Enemys.Length > 0)
        {
            foreach (GameObject enemy in Enemys)
            {
                enemy.GetComponent<Enemy_move>().ChangeState(Enemy_move.State.ALERT);
                enemy.GetComponent<Enemy_move>().setDestination(enemy.transform);
            }
        }
    }
}
