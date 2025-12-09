using UnityEngine;
using UnityEngine.EventSystems;

enum Dir
{
    Up,
    Down,
    Left,
    Right
}

enum State
{
    Idle,
    Move,
    Skill

}

public class Player : MonoBehaviour
{
    // 플래그 = 깃발 세우기

    Animator animator;
    SpriteRenderer spriterenderer;

    Dir dir;
    State state;
    public float Speed = 1.0f;
    Vector3 moveDirection = Vector3.zero;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateInput();
        UpdateAnimation();
        float moveDistance = Speed * Time.deltaTime;
        transform.position += moveDirection.normalized * moveDistance;

    }

    void UpdateInput()
    {
        moveDirection = Vector3.zero;

        if (isAttacking)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            state = State.Move;
            dir = Dir.Up;
            moveDirection += Vector3.up;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            state = State.Move;
            dir = Dir.Left;
            moveDirection += Vector3.left;

        }
        else if (Input.GetKey(KeyCode.S))
        {
            state = State.Move;
            dir = Dir.Down;
            moveDirection += Vector3.down;


        }
        else if (Input.GetKey(KeyCode.D))
        {
            state = State.Move;
            dir = Dir.Right;
            moveDirection += Vector3.right;

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.Skill;

            isAttacking = true;
        }
        else
        {
            if (state == State.Skill)

                return;

            state = State.Idle;

        }
    }

    void UpdateAnimation()
    {
        if (state == State.Idle)
        {
            switch (dir)
            {
                case Dir.Up:
                    animator.Play("Up_Idle");
                    break;
                case Dir.Down:
                    animator.Play("Down_Idle");
                    break;
                case Dir.Left:
                    animator.Play("Side_Idle");
                    spriterenderer.flipX = true;
                    break;
                case Dir.Right:
                    animator.Play("Side_Idle");
                    spriterenderer.flipX = false;
                    break;
                default:
                    break;
            }
        }
        if (state == State.Move)
        {
            switch (dir)
            {
                case Dir.Up:
                    animator.Play("Up_Walk");
                    break;
                case Dir.Down:
                    animator.Play("Down_Walk");
                    break;
                case Dir.Left:
                    animator.Play("Side_Walk");
                    spriterenderer.flipX = true;
                    break;
                case Dir.Right:
                    animator.Play("Side_Walk");
                    spriterenderer.flipX = false;
                    break;
                default:
                    break;
            }
        }
        if (state == State.Skill)
        {
            switch (dir)
            {
                case Dir.Up:
                    animator.Play("Up_Attack");
                    break;
                case Dir.Down:
                    animator.Play("Down_Attack");
                    break;
                case Dir.Left:
                    animator.Play("Side_Attack");
                    spriterenderer.flipX = true;
                    break;
                case Dir.Right:
                    animator.Play("Side_Attack");
                    spriterenderer.flipX = false;
                    break;
                default:
                    break;
            }
        }
    }
        public void OnAttackEnded()
        {
            state = State.Idle;

            isAttacking = false;
        }
    }



