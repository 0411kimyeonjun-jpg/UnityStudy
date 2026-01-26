using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogicManager : MonoBehaviour
{
    // 점수 및 UI
    public int score = 0; // 현재 점수
    public TextMeshProUGUI scoreText; // 실시간 점수
    public TextMeshProUGUI highScoreText; // 최고 점수
    public TextMeshProUGUI finalScoreText; 
    public GameObject gameOverScreen;

    // 난이도 관리
    public PipeSpawner pipeSpawner;
    private int difficultyLevel = 1;

    // 일시정지
    public GameObject PauseMenuScreen;
    private bool isPause = false;

    // 배경 순환
    public Camera mainCamera;
    public Color dayColor = new Color(0.45f, 0.75f, 1.0f);
    public Color nightColor = new Color(0.01f, 0.01f, 0.05f);
    public Color dawnColor = new Color(0.2f, 0.15f, 0.35f);
    public float transitionSpeed = 0.5f;

    private int scorePerCycle = 50;

    public int GetCycle()
    {
        return (score % (scorePerCycle * 3)) / scorePerCycle;
    }

    void Start()
    {
        Time.timeScale = 1f;
        PipeMove.speed = 3.5f;
        updateScoreText();

        // 저장된 최고 점수
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null) highScoreText.text = "Best: " + savedHighScore.ToString();

        // 카메라 설정 및 초기 낮 배경색 적용
        if (mainCamera == null) mainCamera = Camera.main;
        mainCamera.backgroundColor = dayColor;
    }

    private void Update()
    {
        // esc로 일시정지 / 재개 제어
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) ResumeGame();
            else PauseGame();
        }

        // 배경색 순환
        int cycle = GetCycle();
        Color targetColor = dayColor;
        if (cycle == 1) targetColor = nightColor;
        else if (cycle == 2) targetColor = dawnColor;

        // 카메라 배경색 부드럽게 변경
        mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, targetColor, transitionSpeed * Time.deltaTime);
    }
    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        updateScoreText();

        // 10점씩 레벨업 체크
        if (score >= difficultyLevel * 10) 
            LevelUp();
    }

    public IEnumerator ShakeCamera(float duration, float magnitude) // 카메라 흔드는 연출
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            mainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.unscaledDeltaTime; // 일시정지 중에도 작동하도록 처리
            yield return null;
        }
        mainCamera.transform.localPosition = originalPos; // 원래 위치로 복귀
    }

    // 일시정지 제어 함수
    public void PauseGame() { isPause = true; PauseMenuScreen.SetActive(true); Time.timeScale = 0f; }
    public void ResumeGame() { isPause = false; PauseMenuScreen.SetActive(false); Time.timeScale = 1f; }

    void LevelUp()
    {
        difficultyLevel++;

        if (PipeMove.speed < 6.5f) // 파이프 이동 속도 증가 (최대 6.5)
            PipeMove.speed += 0.5f; 

        if (pipeSpawner != null && pipeSpawner.spawnRate > 1.4f) // 파이프 생성 간격 단축 (최대 1.4초까지)
            pipeSpawner.spawnRate -= 0.15f; 
    }

    void updateScoreText() { if (scoreText != null) scoreText.text = "Score : " + score.ToString(); }

    public void gameOver() // 플레이어 사망 시 호출되는 게임 오버 처리
    {
        int CurrentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > CurrentHighScore) 
        { 
            CurrentHighScore = score; 
            PlayerPrefs.SetInt("HighScore", score); 
            PlayerPrefs.Save(); 
        }

        if (finalScoreText != null) 
            finalScoreText.text = "Score: " + score.ToString();

        if (highScoreText != null) 
            highScoreText.text = "Best: " + CurrentHighScore.ToString();

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence() 
    { 
        yield return new WaitForSecondsRealtime(0.15f); 
        gameOverScreen.SetActive(true); 
        Time.timeScale = 0f; 
    }
    public void MainMenu()
    {
        Time.timeScale = 1f; // 게임 속도를 정상화하고 이동
        SceneManager.LoadScene("Main"); // 메인 메뉴 씬 이름으로 변경
    }
    public void restartGame() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}