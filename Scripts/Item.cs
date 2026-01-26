using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 1. 부딪힌 오브젝트의 태그가 "Player"인지 확인
        {
            // 2. 부딪힌 오브젝트(플레이어)에서 BirdController 스크립트 컴포넌트를 가져옴
            BirdController bird = collision.GetComponent<BirdController>();

            // 3. BirdController 컴포넌트가 존재한다면 (정상적으로 가져왔다면)
            if (bird != null)
            {
                {
                    bird.ActivateShield(); // 4. 플레이어의 쉴드 기능을 활성화 시킴
                }
                // 5. 아이템 습득이 완료되었으므로 아이템 오브젝트를 씬에서 삭제
                Destroy(gameObject);
            }
        }

    }
}
