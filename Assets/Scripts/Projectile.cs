using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    public float duration = 10;
    public int damage = 2;
    public float knockback = 50;
    float timer;

    private void Start()
    {
        timer = duration;
    }

    public void Shoot(Quaternion direction)
    {
        transform.rotation = direction;
    }


    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(this.gameObject);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, (collision.transform.position - transform.position).normalized * knockback);
            Destroy(this.gameObject);
        }
    }
}
