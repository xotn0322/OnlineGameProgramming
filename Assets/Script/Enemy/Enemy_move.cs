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

    public float detectionRange = 10.0f;  // Enemy의 감지 범위
    public float detectionAngle = 45.0f;  // Enemy의 감지 각도
    public float attackRange = 2.0f;      // Enemy의공격 사거리
    public int rayCount = 10;             // 부채꼴을 구성할 Ray의 개수
    public string targetTag = "Player";   // 감지할 대상의 태그
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
                // StateMachine 을 공격으로 변경
                //ChangeState(State.ATTACK);
            }
        }
        else
        {
            // 목표와의 거리가 멀어진 경우
            if (nav.remainingDistance > detectionRange)
            {
                /*//사거리에서 벗어난 경우 즉시 멈춤
                nav.destination = transform.position;
                yield return new WaitForSeconds(0.5f);*/
                
                // StateMachine 을 대기로 변경
                point = getShortestPoint(); //근처에서 가장 가까운 웨이포인트 검색
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

        // 감지 결과를 콘솔에 출력 (디버깅 용)
        Debug.Log("Target Detected: " + isTargetDetected);
    }

    // 감지 결과를 반환하는 함수
    public bool IsTargetDetected()
    {
        return isTargetDetected;
    }

    // 감지 범위를 시각적으로 표시 (디버깅 용)
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
