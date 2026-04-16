using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int damage;
    public float knockback;
    bool isActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage, (other.transform.position - transform.position).normalized * knockback);
            isActive = false;
        }

        else if (other.gameObject.tag == "Crystal")
        {
            other.gameObject.GetComponent<Crystal>().TakeDamage(damage);
            isActive = false;
        }
    }
}
