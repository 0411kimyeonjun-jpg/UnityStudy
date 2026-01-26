using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    // 물리 및 이동
    public float jumpForce = 5f; // 점프
    private Rigidbody2D rb;
    private bool isDead = false; // 사망 상태 확인
    public LogicManager logic;

    // 회전 연출
    public float rotationSpeed = 5f; // 회전하는 속도
    public float amount = 5f;        // 속도에 따른 회전 민감도   
    public float maxUpsideAngle = 25f; // 위로 회전할 때 최대 각도
    public float maxDownsideAngle = -90f; // 아래로 회전할 때 최대 각도

    // 쉴드 시스템
    public bool hasShield = false; // 쉴드 보유 여부
    public GameObject ShieldVisual; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (logic == null) logic = GameObject.FindObjectOfType<LogicManager>();

        // 시작 시 쉴드 상태 및 비주얼 초기화
        hasShield = false;
        if (ShieldVisual != null)
        {
            ShieldVisual.SetActive(false);
            ShieldVisual.transform.localPosition = Vector3.zero;
        }
    }

    void Update()
    {
        if (isDead) return;

        // 입력 처리 (마우스 왼쪽 || 스페이스 바 )
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        // 캐릭터 현재속도에 맞춰서 부드러운 회전 적용
        ApplySmoothRotation();

        // 쉴드 활성화 중이면 비주얼 오브젝트를 플레이어 위치에 계속 유지
        if (hasShield && ShieldVisual != null)
        {
            ShieldVisual.transform.localPosition = Vector3.zero;
        }
    }

    // 아이템 습득 시 호출되는 쉴드 활성화 함수
    public void ActivateShield()
    {
        hasShield = true;
        if (ShieldVisual != null)
        {
            ShieldVisual.SetActive(false); // 재활성화 시 깜빡임 방지를 위해 껐다 켬
            ShieldVisual.SetActive(true);
            ShieldVisual.transform.localPosition = Vector3.zero;
        }
    }

    void ApplySmoothRotation() // 새의 속도에 따라 고개를 들거나 숙이는 연출 함수
    {
        // y축 속도에 비례하여 각도를 계산하고, 설정한 최소/최대 범위 내로 고정(Clamp)
        float targetZAngle = Mathf.Clamp(rb.linearVelocity.y * amount, maxDownsideAngle, maxUpsideAngle);

        // 목표 각도를 쿼터니언 값으로 변환
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZAngle);

        // 현재 각도에서 목표 각도까지 부드럽게 보간(Lerp)하여 회전 적용
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) // 다른 물체(장애물)와 충돌했을 때 실행되는 함수
    {
        if (isDead)
            return;

        if (hasShield) // 쉴드 활성화 됐을때
        {
            hasShield = false; // 쉴드 소모
            if (ShieldVisual != null)
            {
                ShieldVisual.SetActive(false); // 쉴드 비주얼 끄기
            }

            // 부딪힌 대상이 파이프라면 파이프 추락 연출 실행
            PipeMove pipe = collision.gameObject.GetComponentInParent<PipeMove>();
            if (pipe != null)
            {
                pipe.OnShieldHit(); // 파이프의 콜라이더를 끄고 날려버리는 함수 호출
            }
            return; // 죽지 않고 함수 탈출
        }
        isDead = true; // 사망처리

        if (logic != null)
        {
            // 카메라 흔들림 효과 시작
            StartCoroutine(logic.ShakeCamera(0.2f, 0.3f));

            // 게임오버 UI 실행
            logic.gameOver();
        }
    }
}