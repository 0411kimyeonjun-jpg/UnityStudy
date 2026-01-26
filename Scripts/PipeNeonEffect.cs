using UnityEngine;

public class PipeNeonEffect : MonoBehaviour
{
    private SpriteRenderer[] childSprites;

    private LogicManager logic;

    // 색상 설정
    public Color neonColor = new Color(0f, 1f, 1f, 1f);  // 밤/새벽에 적용할 형광색 (기본: 하늘색 네온)
    public Color originalColor = Color.white;           // 낮에 적용할 원래 색상 (기본: 흰색)
    public float transitionSpeed = 1.5f;               // 색상이 변하는 부드러운 속도

    // 규칙적인 네온 설정
    public float flickerSpeed = 4f;     // 깜빡이는 속도 (높을수록 빠름)
    public float minIntensity = 0.85f;  // 최소 밝기 (0.0 ~ 1.0)
    public float maxIntensity = 1.15f;  // 최대 밝기

    void Start()
    {
        childSprites = GetComponentsInChildren<SpriteRenderer>(); 
        logic = GameObject.FindObjectOfType<LogicManager>(); 
    }

    void Update()
    {
        if (logic != null)
        {
            int cycle = logic.GetCycle();
            Color targetColor = (cycle == 0) ? originalColor : neonColor;

            // 1. Sine 함수를 이용해 0~1 사이를 왕복하는 규칙적인 값 계산
            // Mathf.Sin은 -1~1을 반환하므로, 이를 0~1 범위로 변환
            float sineWave = (Mathf.Sin(Time.time * flickerSpeed) + 1f) / 2f;

            // 2. 설정한 최소/최대 밝기 사이를 부드럽게 왕복하도록 계산
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, sineWave);

            foreach (SpriteRenderer sr in childSprites) 
            {
                // 낮/밤 전환은 기존처럼 부드럽게 처리
                sr.color = Color.Lerp(sr.color, targetColor, transitionSpeed * Time.deltaTime);

                // 밤/새벽일 때만 규칙적인 깜빡임 적용
                if (cycle != 0)
                {
                    sr.color *= intensity;
                }
            }
        }
    }
}