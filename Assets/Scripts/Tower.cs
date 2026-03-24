using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootTimer = 5;
    float timer;

    public Transform gunBarrel;
    public Transform wholeTurret;
    
   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = shootTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = shootTimer;

            Projectile tempBullet = Instantiate<GameObject>(bulletPrefab).GetComponent<Projectile>();

            tempBullet.transform.position = gunBarrel.position;
            tempBullet.Shoot(gunBarrel.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        TargetEnemy(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TargetEnemy(other);
    }

    private void TargetEnemy(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            wholeTurret.LookAt(other.transform);
        }
    }
}
