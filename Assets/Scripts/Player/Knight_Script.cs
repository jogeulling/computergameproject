using UnityEngine;

public class Knight_Script : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float checkRadius = 0.45f;
    float moveSpeed = 4.0f;
    float jumpPower = 15.0f;
    bool isGrounded;

    bool isAttacked = false;
    bool movePos = false;
    private float curTime = 0;
    public float coolTime = 0.5f;
    public LayerMask attack;
    Vector2 center;
    Vector2 size;


    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigid;
    BoxCollider2D hitcheck;
    Transform hitcheckTransform;

    Vector2 hitBoxOriginPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.rigid = GetComponent<Rigidbody2D>();
        this.hitcheck = GetComponentInChildren<BoxCollider2D>();
        this.hitcheckTransform = transform.Find("hitcheck");
        this.hitcheck.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.isGrounded = Physics2D.OverlapCircle(this.groundCheck.position, this.checkRadius, this.whatIsGround);

        // 좌, 우 움직임을 제어
        if (Input.GetKey(KeyCode.A))
        {
            this.spriteRenderer.flipX = true;
            this.rigid.linearVelocity = new Vector2(-this.moveSpeed, this.rigid.linearVelocity.y);
            if (this.movePos == false)
            {
                Vector3 localPos = this.hitcheckTransform.localPosition;
                localPos.x = -localPos.x;
                this.hitcheckTransform.localPosition = localPos;

                this.movePos = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.spriteRenderer.flipX = false;
            this.rigid.linearVelocity = new Vector2(this.moveSpeed, this.rigid.linearVelocity.y);
            if (this.movePos == true)
            {
                Vector3 localPos = this.hitcheckTransform.localPosition;
                localPos.x = -localPos.x;
                this.hitcheckTransform.localPosition = localPos;

                this.movePos = false;
            }
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
        if (this.curTime > 0)
        {
            this.curTime -= Time.deltaTime;
        }

        if (this.curTime <= 0 && Input.GetKey(KeyCode.J))
        {
            this.isAttacked = true;
        }

        if (this.curTime <= 0 && Input.GetKeyDown(KeyCode.J) && this.isAttacked)
        {
            Debug.Log("true");
            this.animator.SetTrigger("Attack");
            this.curTime = coolTime;
            this.isAttacked = false;
        }
    }

    void AttackStart()
    {
        Collider2D hit = Physics2D.OverlapBox(this.hitcheckTransform.position, this.size, 0, this.attack);

        if (hit)
        {
            Debug.Log("피격함!");
        }
    }

    void AttackEnd()
    {
        
    }
}
