using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    // 잔상 생성 설정
    public float ghostDelay = 0.05f; 
    private float ghostDelayTimer; 
    public GameObject ghostPrefab; 
    public LogicManager logic; 

    // 잔상 외형 설정
    public float ghostScaleOffset = 1.0f;
    public float positionOffsetLeft = 0.1f; 

    // 환경에 따라 잔상 색상 설정
    public Color dayGhostColor = new Color(1, 1, 1, 0.5f); // 반투명 흰색
    public Color nightGhostColor = new Color(1f, 1f, 0f, 0.85f); // 선명한 노란색
    public Color dawnGhostColor = new Color(1f, 0.4f, 0.9f, 0.75f); // 분홍 이지만 인게임 상 붉게 나옴

    void Start()
    {
        ghostDelayTimer = ghostDelay; // 시작할때 타이머 초기화

        if (logic == null) logic = GameObject.FindObjectOfType<LogicManager>(); 
    }

    void Update()
    {
        if (ghostDelayTimer > 0)
        {
            ghostDelayTimer -= Time.deltaTime; 
        }
        else
        {
            // 1. 생성 위치 계산
            Vector3 spawnPos = transform.position + (Vector3.left * positionOffsetLeft);

            // 2. 잔상 프리팹 생성
            GameObject currentGhost = Instantiate(ghostPrefab, spawnPos, transform.rotation);

            // 3. 잔상의 크기를 본체 크기에 맞춰 설정
            currentGhost.transform.localScale = transform.localScale * ghostScaleOffset;

            // 4. 컴포넌트 참조 (잔상과 본체의 이미지 정보를 다룸)
            SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>(); 
            SpriteRenderer birdSR = GetComponent<SpriteRenderer>();

            // 5. 본체의 현재 이미지를 잔상에 복사하고, 잔상이 본체 뒤에 그려지도록 정렬 순서 조정
            ghostSR.sprite = birdSR.sprite; 
            ghostSR.sortingOrder = birdSR.sortingOrder - 1;

            // 6. 현재 주기(Cycle)를 받아와 색상 결정
            int cycle = logic.GetCycle();
            if (cycle == 0) ghostSR.color = dayGhostColor;
            else if (cycle == 1) ghostSR.color = nightGhostColor;
            else ghostSR.color = dawnGhostColor;

            // 7. 타이머 재설정 및 일정 시간(0.3초) 후 잔상 오브젝트 파괴
            ghostDelayTimer = ghostDelay; 
            Destroy(currentGhost, 0.3f); 
        }
    }
}