using UnityEngine;
using UnityEngine.UI;
public class HPBar_Script : MonoBehaviour
{
    Slider hpbar;
    public GameObject Knight;
    Knight_Script player1;
    RKnight_Script player2;
    float HP;
    float MaxHP = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Knight != null)
        {
            this.hpbar = GetComponent<Slider>();
            if (this.Knight.name == "LKnight")
            {
                this.player1 = this.Knight.GetComponent<Knight_Script>();
            } else if (this.Knight.name == "RKnight")
            {
                this.player2 = this.Knight.GetComponent<RKnight_Script>();
            }
            
        }
    }
        // Update is called once per frame
        void Update()
    {
        if (Knight!=null)
        {
            if (this.Knight.name == "LKnight")
            {
                this.HP = this.player1.HPValue();
                this.hpbar.value = this.HP / this.MaxHP;
            }
            else if (this.Knight.name == "RKnight")
            {
                this.HP = this.player2.HPValue();
                this.hpbar.value = this.HP / this.MaxHP;
            }

            if (this.hpbar.value == 0f)
            {
                this.hpbar.fillRect.gameObject.SetActive(false);
            }
        }
    }
}
