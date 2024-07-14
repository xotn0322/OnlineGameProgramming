using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Enemy_move : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField] private GameObject[] wayPoints;
    private int point;
    private float HP = 10.0f;

    public float detectionRange = 10.0f;  // Enemy�� ���� ����
    public float detectionAngle = 45.0f;  // Enemy�� ���� ����
    public float attackRange = 2.0f;      // Enemy�ǰ��� ��Ÿ�
    public int rayCount = 10;             // ��ä���� ������ Ray�� ����
    public string targetTag = "Player";   // ������ ����� �±�
    private bool isTargetDetected = false;
    private GameObject target;
    enum State
    {
        IDLE,
        CHASE,
        ATTACK
    }

    State state;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            DetectTarget();
            yield return StartCoroutine(state.ToString());
            Debug.Log(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        if (!nav.pathPending && nav.remainingDistance < 0.5f)
        {
            nav.destination = distributeY(wayPoints[point]);

            point = (point + 1) % wayPoints.Length;

            /*if (!isidle) { isidle = true; }*/
        }
        //DetectTarget();
        yield return null;
    }

    IEnumerator CHASE()
    {
        nav.destination = distributeY(target);
        //DetectTarget();
        if (IsTargetDetected())
        {
            if (nav.remainingDistance <= attackRange)
            {
                // StateMachine �� �������� ����
                //ChangeState(State.ATTACK);
            }
        }
        else
        {
            // ��ǥ���� �Ÿ��� �־��� ���
            if (nav.remainingDistance > detectionRange)
            {
                /*//��Ÿ����� ��� ��� ��� ����
                nav.destination = transform.position;
                yield return new WaitForSeconds(0.5f);*/
                
                // StateMachine �� ���� ����
                point = getShortestPoint(); //��ó���� ���� ����� ��������Ʈ �˻�
                ChangeState(State.IDLE);
                yield return null;
            }
        }

        yield return null;
    }

    private Vector3 distributeY(GameObject way)
    {
        Vector3 wayPoint = new Vector3();
        wayPoint.x = way.transform.position.x;
        wayPoint.y = gameObject.transform.position.y;
        wayPoint.z = way.transform.position.z;

        return wayPoint;
    }

    private int getShortestPoint()
    {
        Vector3 myPosition = transform.position;
        float shortDestination = (wayPoints[0].transform.position - myPosition).magnitude;
        int shortest = 0;

        for(int i = 1; i < wayPoints.Length; i++)
        {
            float destination = (wayPoints[i].transform.position - myPosition).magnitude;
            if (destination < shortDestination)
            {
                shortDestination = destination;
                shortest = i;
            }
        }

        return shortest;
    }

    private void ChangeState(State nowState)
    {
        state = nowState;
    }

    

    void DetectTarget()
    {
        Debug.Log(123);
        isTargetDetected = false;

        float angleStep = detectionAngle / rayCount;
        float startingAngle = -detectionAngle / 2;

        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = startingAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionRange))
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    isTargetDetected = true;
                    target = hit.transform.gameObject;
                    break;
                }
            }
        }

        if (IsTargetDetected())
        {
            nav.destination = distributeY(target);
            ChangeState(State.CHASE);
        }

        // ���� ����� �ֿܼ� ��� (����� ��)
        Debug.Log("Target Detected: " + isTargetDetected);
    }

    // ���� ����� ��ȯ�ϴ� �Լ�
    public bool IsTargetDetected()
    {
        return isTargetDetected;
    }

    // ���� ������ �ð������� ǥ�� (����� ��)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float angleStep = detectionAngle / rayCount;
        float startingAngle = -detectionAngle / 2;

        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = startingAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Gizmos.DrawRay(transform.position, direction * detectionRange);
        }
    }
}
