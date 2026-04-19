using UnityEngine;

public class Crystal : MonoBehaviour
{
    public HPBar hpbar;


    public int maxHP = 100;
    public int curHP = 100;

    public Animator attackedUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpbar.SetMaxHP(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(int damage)
    {
        curHP -= damage;

        if (curHP <= 0)
        {
            curHP = 0;
            PlayerController.playcon.GameLose();
        }
        else
        {
            attackedUI.SetTrigger("Hit");
        }


        hpbar.UpdateHP(curHP);
    }
}
