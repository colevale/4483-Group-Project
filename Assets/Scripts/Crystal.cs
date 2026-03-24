using UnityEngine;

public class Crystal : MonoBehaviour
{
    public HPBar hpbar;


    public int maxHP = 100;
    public int curHP = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpbar.SetMaxHP(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            curHP -= 10;
            collision.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - collision.transform.position).normalized * -100);
        }

        if (curHP <= 0)
            print("You lost!!!1!!");

        hpbar.UpdateHP(curHP);
    }
}
