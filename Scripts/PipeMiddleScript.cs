using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    private LogicManager logic;

    void Start()
    {
        logic = GameObject.FindObjectOfType<LogicManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            logic.addScore(1);

        }
    }
}