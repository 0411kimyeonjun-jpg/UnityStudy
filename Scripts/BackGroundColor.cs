using UnityEngine;

public class BackGroundColor : MonoBehaviour
{
    private SpriteRenderer sr;

    private LogicManager logic; 

    // 시간대 별 배경 색상
    public Color dayTint = Color.white; // 낮
    public Color nightTint = new Color(0.15f, 0.15f, 0.3f); // 밤
    public Color dawnTint = new Color(0.4f, 0.35f, 0.5f); // 새벽

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        logic = GameObject.FindObjectOfType<LogicManager>(); 
    }

    void Update()
    {
        // LogicManager가 정상적으로 연결되어 있을 때만 실행
        if (logic != null)
        {
            int cycle = logic.GetCycle(); // LogicManager의 GetCycle() 함수를 호출하여 현재 주기 확인 (0:낮, 1:밤, 2:새벽)

            Color targetColor = dayTint; // 기본 목표 색상을 낮으로 설정

            // 주기에 따라 목표 색상을 밤이나 새벽으로 변경
            if (cycle == 1) targetColor = nightTint; // 밤
            else if (cycle == 2) targetColor = dawnTint; // 새벽

            // 배경 톤 부드럽게 전환
            sr.color = Color.Lerp(sr.color, targetColor, 0.5f * Time.deltaTime); 
        }
    }
}