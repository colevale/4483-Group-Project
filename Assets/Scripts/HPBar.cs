using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public float maxHP = 100;
    public float curHP = 100;

    public Transform hpbar;
    public Image image;


    public Color fullHPCol, lowHPCol;

    public float xOffset = 2.4f;

    public bool isUI;

    private void Update()
    {
        Vector3 playerPos = PlayerController.playcon.transform.position;

        Vector3 dir = playerPos - transform.position;
        if (!isUI)
        {
            transform.LookAt(dir);
        }
        

    }


    public void SetMaxHP(int hp)
    {
        maxHP = hp;
    }


    public void UpdateHP(int newHP)
    {
        curHP = newHP;

        float t = curHP / maxHP;
        if (t > 1)
            t = 1;
        if (t < 0)
            t = 0;

        hpbar.localScale = new Vector3(t, 1, 1);
        hpbar.localPosition = Vector3.right * (1 - t) * xOffset;
        image.color = t * fullHPCol + (1 - t) * lowHPCol;


        
    }
}
