using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_controller : MonoBehaviour
{
    /*public float detectionRange = 10.0f;  // CCTV 감지 범위
    public float detectionAngle = 45.0f;  // CCTV 감지 각도
    public string targetTag = "Player";   // 감지할 대상의 태그
    private bool isTargetDetected = false;

    void Update()
    {
        DetectTarget();
    }

    void DetectTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        isTargetDetected = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget <= detectionAngle / 2)
                {
                    isTargetDetected = true;
                    break;
                }
            }
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
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }*/

    public float detectionRange = 10.0f;  // CCTV 감지 범위
    public float detectionAngle = 45.0f;  // CCTV 감지 각도
    public int rayCount = 10;             // 부채꼴을 구성할 Ray의 개수
    public string targetTag = "Player";   // 감지할 대상의 태그
    private bool isTargetDetected = false;

    void Update()
    {
        DetectTarget();
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
                    break;
                }
            }
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
