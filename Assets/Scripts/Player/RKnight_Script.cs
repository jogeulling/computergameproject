using UnityEngine;
using System.Collections;

public class RKnight_Script : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float checkRadius = 0.45f;
    float moveSpeed = 4.0f;
    float jumpPower = 15.0f;
    bool isGrounded;

    bool isAttacked = false;
    bool movePos = false;
    private float curTime;
    public float coolTime = 0.5f;
    public LayerMask attack;
    Vector2 size;

    bool isHit = false;
    int HP = 100;
    int Damage = 10;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigid;
    BoxCollider2D hitcheck;
    Transform hitcheckTransform;
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

        if (!isHit)
        {
            // 좌, 우 움직임을 제어
            if (Input.GetKey(KeyCode.LeftArrow))
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
            else if (Input.GetKey(KeyCode.RightArrow))
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

        if (this.curTime <= 0 && Input.GetKey(KeyCode.Keypad1))
        {
            this.isAttacked = true;
        }

        if (this.curTime <= 0 && Input.GetKeyDown(KeyCode.Keypad1) && this.isAttacked)
        {
            this.animator.SetTrigger("Attack");
            this.curTime = coolTime;
        }
    }

    void AttackStart()
    {
        Collider2D hit = Physics2D.OverlapBox(this.hitcheckTransform.position, this.size, 0, this.attack);
        GameObject player1 = GameObject.Find("LKnight");
        Knight_Script RKnight = player1.GetComponent<Knight_Script>();
        int dir = player1.transform.position.x - transform.position.x > 0 ? 1 : -1;

        if (hit)
        {
            RKnight.Hit(Damage, dir, 7f);
        }
    }

    void AttackEnd()
    {
        this.isAttacked = false;
    }

    public void Hit(int damage, int dir, float knockbackPower)
    {
        if (!this.isHit)
        {
            this.HP -= damage;

            this.rigid.AddForce(new Vector2(dir * 7, 1) * knockbackPower, ForceMode2D.Impulse);

            if (this.HP <= 0)
            {
                this.animator.SetTrigger("Death");
            }
            else
            {
                this.animator.SetTrigger("Hit");
            }
        }
    }

    IEnumerator HitStart()
    {
        this.isHit = true;

        while (this.isHit)
        {
            this.spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.01f);
            this.spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void HitEnd()
    {
        this.isHit = false;
    }

    void DeathEnd()
    {
        Destroy(gameObject);
    }
}

