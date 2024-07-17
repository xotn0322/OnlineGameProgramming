using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Enemy_move : MonoBehaviour
{
    NavMeshAgent nav;
    MeshRenderer self;
    private Color initColor;
    private int point;
    private float HP = 10.0f;
    private float initDetectionRange;
    private bool isTargetDetected = false;
    private bool isAlert = false;
    private GameObject target;

    public float detectionRange = 10.0f;  // Enemy�� ���� ����
    public float alertDetectionRange = 15.0f; //��� ���� ���� ���� ����
    public float detectionAngle = 45.0f;  // Enemy�� ���� ����
    public float attackRange = 2.0f;      // Enemy�ǰ��� ��Ÿ�
    public int rayCount = 10;             // ��ä���� ������ Ray�� ����
    public string targetTag = "Player";   // ������ ����� �±�
    [SerializeField] private GameObject[] wayPoints;

    [HideInInspector]
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        ALERT
    }

    State state;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        initDetectionRange = detectionRange;
        self = GetComponent<MeshRenderer>();
        initColor = self.material.color;

        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            DetectTarget();
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        self.material.color = initColor;
        if (!nav.pathPending && nav.remainingDistance < 0.5f)
        {
            nav.destination = distributeY(wayPoints[point]);

            point = (point + 1) % wayPoints.Length;

        }
        yield return null;
    }

    IEnumerator CHASE()
    {
        self.material.color = Color.red;
        if (IsTargetDetected())
        {
            nav.destination = distributeY(target);
            if (getDistanceToTarget() < attackRange && target != null)
            {
                // StateMachine �� �������� ����
                //ChangeState(State.ATTACK);
                UI_End.instance.Dead();
            }
        }
        else
        { 
            // ��ǥ���� �Ÿ��� �־��� ���
            if (nav.remainingDistance > detectionRange || nav.remainingDistance <= 0)
            {
                /*//��Ÿ����� ��� ��� ��� ����
                nav.destination = transform.position;
                yield return new WaitForSeconds(0.5f);*/

                // StateMachine �� ���� ����
                point = getShortestPoint(); //��ó���� ���� ����� ��������Ʈ �˻�
                //point = 0; //ó����������
                nav.destination = distributeY(wayPoints[point]);
                isAlert = true;
                ChangeState(State.ALERT);
                yield return null;
            }
        }

        yield return null;
    }

    IEnumerator ALERT()
    {
        detectionRange = alertDetectionRange;
        if (isAlert) { StopCoroutine(ALERT_TIME()); }
        StartCoroutine(ALERT_TIME());
        ChangeState(State.IDLE);
        yield return null;
    }

    IEnumerator ALERT_TIME()
    {
        yield return new WaitForSeconds(60);
        detectionRange = initDetectionRange;
        isAlert = false;
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

    public void ChangeState(State nowState)
    {
        state = nowState;
    }

    void DetectTarget()
    {
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
       // Debug.Log("Target Detected: " + isTargetDetected);
    }

    // ���� ����� ��ȯ�ϴ� �Լ�
    public bool IsTargetDetected()
    {
        return isTargetDetected;
    }

    public void setDestination(Transform target)
    {
        nav.destination = target.position;
    }

    public float getDistanceToTarget()
    {
        float distance;
        if(target != null)
        {
            distance = (target.transform.position - gameObject.transform.position).magnitude;
            return distance;
        }
        else
        {
            return attackRange += 1;
        }
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
