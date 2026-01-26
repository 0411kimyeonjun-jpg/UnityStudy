using UnityEngine;
using System.Collections;
public class PipeMove : MonoBehaviour
{
    // 기본 이동 설정
    public static float speed = 3.5f; // 모든 파이프가 공유하는 이동속도
    public float deadZone = -15f; // 파이프가 화면 왼쪽으로 벗어나서 파괴될 X 좌표

    // 새벽 / 고득점 흔들림 설정
    public float upDownSpeed = 1.5f; // 위아래 움직이는 속도
    public float upDownRange = 1.0f; // 위아래 움직이는 범위
    private float startY;
    private LogicManager logic;
    private bool isHit = false;

    void Start()
    {
        startY = transform.position.y;
        logic = GameObject.FindObjectOfType<LogicManager>();
    }
    void Update()
    {
        if (isHit)
            return;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (logic != null && logic.score >= 150) // 점수가 150점 이상이면 위 아래로 물결치듯이 이동
        {
            float newY = startY + Mathf.Sin(Time.time * upDownSpeed) * upDownRange;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        if (transform.position.x < deadZone) // 메모리 관리를 위해 파괴
        {
            Destroy(gameObject);
        }
    }

    public void OnShieldHit()
    {
        if (isHit) return;
        isHit = true;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // 카메라가 흔들리고, 파이프가 추락하는 코루틴 실행
        if (logic != null) logic.StartCoroutine(logic.ShakeCamera(0.15f, 0.25f));
        StartCoroutine(FallAndFade());
    }

    IEnumerator FallAndFade() // 파이프가 추락하면서 투명해지는 연출 코루틴
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        // 튕겨 나갈 랜덤한 방향과 힘 설정 (오른쪽 위 방향)
        Vector3 randomDir = new Vector3(Random.Range(3f, 6f), Random.Range(4f, 8f), 0);
        float timer = 0f;

        while (timer < 0.7f) // 0.7초 동안 연출 진행
        {
            timer += Time.deltaTime;

            randomDir += Vector3.down * 25f * Time.deltaTime;
            transform.position += randomDir * Time.deltaTime;

            transform.Rotate(0, 0, 400f * Time.deltaTime);

            foreach (var sr in srs)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, timer / 0.7f);
                sr.color = c;
            }
            yield return null;
        }
        Destroy(gameObject); // 연출이 끝나면 파이프 제거
    }
}