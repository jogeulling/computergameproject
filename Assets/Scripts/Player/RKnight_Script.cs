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

    bool Die = false;
    bool isHit = false;
    float HP = 100f;
    float Damage = 10f;

    public bool isGuarding = false;
    float epsilon = 0.1f;

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
        if (Mathf.Abs(this.HP) < epsilon)
        {
            this.HP = 0f;
        }

        if (!this.isHit && !this.Die)
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

        // Guard를 제어
        if (Input.GetKey(KeyCode.Keypad2))
        {
            this.animator.SetBool("IsGuarding", true);
            this.isGuarding = true;
        }
    }

    void AttackStart()
    {
        Collider2D hit = Physics2D.OverlapBox(this.hitcheckTransform.position, this.size, 0, this.attack);
        GameObject player1 = GameObject.Find("LKnight");
        Knight_Script LKnight = player1.GetComponent<Knight_Script>();
        int dir = player1.transform.position.x - transform.position.x > 0 ? 1 : -1;

        if (hit && !LKnight.isGuarding)
        {
            LKnight.Hit(Damage, dir, 7f);
        }
        else if (hit && LKnight.isGuarding)
        {
            LKnight.GuardHit(this.Damage, dir, 5f);
        }
    }

    void AttackEnd()
    {
        this.isAttacked = false;
    }

    public void Hit(float damage, int dir, float knockbackPower)
    {
        if (!this.isHit)
        {
            this.HP -= damage;

            if (this.HP <= 0)
            {
                this.Die = true;
                this.animator.SetTrigger("Death");
            }
            else
            {
                this.rigid.AddForce(new Vector2(dir * 7, 1) * knockbackPower, ForceMode2D.Impulse);

                if (!this.isGuarding)
                {
                    this.animator.SetTrigger("Hit");
                }
                
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

    public void GuardHit(float damage, int dir, float knockbackPower)
    {
        if (this.HP > 0)
        {
            damage = damage * 0.2f;
            knockbackPower = 5f;
            this.rigid.AddForce(new Vector2(dir * 7, 1) * knockbackPower, ForceMode2D.Impulse);

            this.HP -= damage;
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

    void GuardEnd()
    {
        if (!Input.GetKey(KeyCode.K))
        {
            this.animator.SetBool("IsGuarding", false);
            this.isGuarding = false;
        }
    }

    public float HPValue()
    {
        return this.HP;
    }
}

