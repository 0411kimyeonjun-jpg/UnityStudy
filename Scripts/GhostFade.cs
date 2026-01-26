using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float fadeSpeed = 2f; // 잔상이 사라지는 속도를 조절하는 변수 (높을수록 빨리 사라짐)

    private SpriteRenderer sr; // 오브젝트의 이미지를 제어하기 위한 SpriteRenderer 컴포넌트 변수

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = sr.color;

        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        if (sr.color.a <= 0) // 알파 값이 0 이하가 되어 완전히 투명해졌다면
        {
            Destroy(gameObject); // 메모리 관리를 위해 해당 잔상 오브젝트를 게임상에서 파괴함
        }
    }
}