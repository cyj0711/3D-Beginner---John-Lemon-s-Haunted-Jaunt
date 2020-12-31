using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f; // 플레이어의 회전 속도(초당 회전하는 각도)
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // FixedUpdate()는 Update()와 달리 렌더링된 프레임에 맞춰 호출되는 것이 아니라 물리 시스템이 충돌 및 상호작용을 계산하기 전에 호출된다.
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize(); // 대각선으로 이동하면 더 빨라지는 오류를 수정함

        // 움직이는 상태라면 움직이는 애니메이션으로 전환
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        // 캐릭터의 전방벡터 계산
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        // deltaPosition : 루트 모션으로 인한 프레임당 포지션의 이동량
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
