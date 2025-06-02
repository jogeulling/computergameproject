using UnityEngine;

public class RKnight_Script : MonoBehaviour
{
    float moveSpeed = 4.0f;
    float jumpPower = 15.0f;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌, 우 움직임을 제어
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.spriteRenderer.flipX = true;
            this.rigid.linearVelocity = new Vector2(-this.moveSpeed, this.rigid.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.spriteRenderer.flipX = false;
            this.rigid.linearVelocity = new Vector2(this.moveSpeed, this.rigid.linearVelocity.y);
        }
        else
        {
            this.rigid.linearVelocity = new Vector2(0, this.rigid.linearVelocity.y);
        }

        // 좌, 우 이동시 애니메이션 활상화, 비활성화
        if (this.rigid.linearVelocity.normalized.x == 0)
        {
            this.animator.SetBool("IsWalk", false);
        }
        else
        {
            this.animator.SetBool("IsWalk", true);
        }

        // jump를 제어
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}

