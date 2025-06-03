using UnityEngine;

public class Knight_Script : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float checkRadius = 0.45f;
    float moveSpeed = 4.0f;
    float jumpPower = 15.0f;
    bool isGrounded;

    private float curTime;
    public float coolTime = 0.5f;
    public LayerMask attack;
    Vector2 center;
    Vector2 size;


    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigid;
    BoxCollider2D hitcheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.rigid = GetComponent<Rigidbody2D>();
        this.hitcheck = GetComponentInChildren<BoxCollider2D>();
        this.hitcheck.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.isGrounded = Physics2D.OverlapCircle(this.groundCheck.position, this.checkRadius, this.whatIsGround);
        Collider2D hit = Physics2D.OverlapBox(this.center, this.size, 0, this.attack);

        // 좌, 우 움직임을 제어
        if (Input.GetKey(KeyCode.A))
        {
            this.spriteRenderer.flipX = true;
            this.rigid.linearVelocity = new Vector2(-this.moveSpeed, this.rigid.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (this.isGrounded == true)
            {
                this.rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        // Attack을 제어
        if (this.curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                AttackStart();

                if (hit)
                {
                    Debug.Log("피격함!");
                }

                this.animator.SetTrigger("Attack");
                this.curTime = coolTime;
            }
        }
        else
        {
            this.curTime -= Time.deltaTime;
        }
    }

    void AttackStart()
    {
        if (hitcheck != null)
        {
            this.center = this.hitcheck.transform.TransformPoint(this.hitcheck.offset);
            this.size = this.hitcheck.size;
        }    
    }

    void AttackEnd()
    {
        
    }
}
