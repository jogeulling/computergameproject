using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attack = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo attackinfo = this.attack.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.J))
        {
            this.attack.SetBool("IsAttack", true);
        }

        if (attackinfo.IsName("IsAttack") && attackinfo.normalizedTime >= 1.0f)
        {
            this.attack.SetBool("IsAttack", false);
        }
    }

    
}
