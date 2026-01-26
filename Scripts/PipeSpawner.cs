using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    // 프리팹 및 기본 설정
    public GameObject pipePrefab;
    public float spawnRate = 2.5f; // 파이프 생성되는 기본 간격
    public float heightOffset = 2.5f; // 파이프가 생성될 위 아래 랜덤 범위(중심 기준)
    private float timer = 0; // 다음 스폰까지 시간 계산 타이머

    // 쉴드 아이템 설정
    public GameObject ShieldPrefab;
    public float shiledSpawnChance = 0.10f; // 쉴드가 생성될 확률 (0.10 = 10%)

    // 난이도 조절 설정
    public LogicManager logic;         
    public float minSpawnRate = 1.5f; // 아무리 빨라져도 유지될 최소 생성 간격
    public float difficultyScale = 0.1f; // 점수에 따라 생성 간격이 줄어드는 가중치

    void Start()
    {
        if (logic == null) logic = GameObject.FindObjectOfType<LogicManager>();
        SpawnPipe(); // 게임 시작시 파이프 생성
    }

    void Update()
    {
        // 난이도 계산 로직
        // 점수가 높아질수록 생성 간격을 점점 줄임 (10점당 difficultyScale만큼)
        float currentRate = spawnRate - (logic.score / 10f * difficultyScale);

        // 계산된 간격이 너무 빨라지지 않도록 최솟값으로 고정
        float cappedRate = Mathf.Max(currentRate, minSpawnRate);

        // 타이머를 매 프레임 증가시켜 spawnRate에 도달할 때까지 대기
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            // 타이머가 다 차면 파이프 생성 함수 실행 후 타이머 리셋
            SpawnPipe();
            timer = 0;
        }
    }

    void SpawnPipe()
    {
        // 파이프가 생성될 수 있는 가장 낮은 위치와 높은 위치 계산
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        // 해당 범위 안에서 랜덤한 Y축 높이값을 결정하여 생성 위치 지정
        Vector3 spawnPos = new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0);

        // 파이프 프리팹을 해당 위치에 복제(생성)
        Instantiate(pipePrefab, spawnPos, transform.rotation);

        // 쉴드 프리팹이 존재하고, 랜덤 확률(0~1 사이 값)이 설정된 확률보다 작을 때 쉴드 생성
        if (ShieldPrefab != null && Random.value < shiledSpawnChance)
        {
            Instantiate(ShieldPrefab, spawnPos, Quaternion.identity); // 파이프와 같은 위치에 쉴드 아이템을 생성
        } 
    }
}