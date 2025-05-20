using UnityEngine;

public class Knight_Script : MonoBehaviour
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
        if (Input.GetKey(KeyCode.A))
        {
            this.spriteRenderer.flipX = true;
            this.rigid.velocity = new Vector2(-this.moveSpeed, this.rigid.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.spriteRenderer.flipX = false;
            this.rigid.velocity = new Vector2(this.moveSpeed, this.rigid.velocity.y);
        }
        else
        {
            this.rigid.velocity = new Vector2(0, this.rigid.velocity.y);
        }

        // 좌, 우 이동시 애니메이션 활상화, 비활성화
        if (this.rigid.velocity.normalized.x == 0)
        {
            this.animator.SetBool("IsWalk", false);
        }
        else
        {
            this.animator.SetBool("IsWalk", true);
        }

        // jump를 제어
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
