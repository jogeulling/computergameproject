using Unity.VisualScripting;
using System.Collections;
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
    bool Die = false;
    bool movePos = false;
    private float curTime = 0;
    public float coolTime = 0.5f;
    public LayerMask attack;
    Vector2 size;

    bool isHit = false;
    float HP = 100f;
    public float Damage = 10f;

    public bool isGuarding = false;
    float epsilon = 0.9f;

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
        // 좌, 우 움직임을 제어
        if (!this.isHit && !this.Die)
        {
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
            this.animator.SetTrigger("Attack");
            this.curTime = coolTime;
        }

        // Guard를 제어
        if (Input.GetKey(KeyCode.K) && !this.isGuarding)
        {
            this.animator.SetBool("IsGuarding", true);
            StartCoroutine(ReGuardAfterHit());
        }
    }

    void AttackStart()
    {
        Collider2D hit = Physics2D.OverlapBox(this.hitcheckTransform.position, this.size, 0, this.attack);
        GameObject player2 = GameObject.Find("RKnight");
        RKnight_Script RKnight = player2.GetComponent<RKnight_Script>();
        int dir = player2.transform.position.x - transform.position.x > 0 ? 1 : -1;
        bool enemyInfront = IsPlayer2InFront(RKnight.transform);

        if (hit && !RKnight.isGuarding)
        {
            RKnight.Hit(this.Damage, dir, 5f, enemyInfront);
        }
        else if (hit && RKnight.isGuarding && !enemyInfront)
        {
            RKnight.Hit(this.Damage, dir, 5f, enemyInfront);
        }
        else if (hit && RKnight.isGuarding && enemyInfront)
        {
            RKnight.GuardHit(this.Damage, dir, 3f);
        }
    }

    void AttackEnd()
    {
        this.isAttacked = false;
    }

    public void Hit(float damage, int dir, float knockbackPower, bool enemyInfront)
    {
        if (!this.isHit)
        {
            this.isHit = true;
            this.HP -= damage;

            if (this.HP <= 0)
            {
                this.Die = true;
                this.animator.SetTrigger("Death");
            }
            else
            {
                this.rigid.AddForce(new Vector2(dir, 1) * knockbackPower, ForceMode2D.Impulse);

                if (!this.isGuarding)
                {
                    this.animator.SetTrigger("Hit");
                }
                else if (this.isGuarding && !enemyInfront)
                {
                    this.animator.SetTrigger("Backhit");
                }
            }
        }
    }

    IEnumerator HitStart()
    {
        gameObject.layer = 9;

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
            this.rigid.AddForce(new Vector2(dir, 1) * knockbackPower, ForceMode2D.Impulse);

            this.HP -= damage;

            this.isHit = false;
        }
    }

    void HitEnd()
    {
        this.isHit = false;
        gameObject.layer = 7;
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

    bool IsPlayer2InFront(Transform other)
    {
        SpriteRenderer otherSprite = other.GetComponent<SpriteRenderer>();
        // flipX가 true면 왼쪽, false면 오른쪽
        Vector2 forward = otherSprite.flipX ? Vector2.left : Vector2.right;
        Vector2 toOther = (other.position - this.transform.position).normalized;
        float dot = Vector2.Dot(forward, toOther);
        return dot < 0.0f;
    }
    
    IEnumerator ReGuardAfterHit()
    {
        if (this.isHit)
        {
            yield return new WaitForSeconds(0.2f);
        } else
        {
            yield return new WaitForSeconds(0f);
            this.isGuarding = true;
        }
    }
}
