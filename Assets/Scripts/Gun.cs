using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem particles;
    Animator anim;

    public float timeBetweenShots = 1f;
    float shootCooldownCur = 0;

    public Transform bulletOffset;
    public GameObject bulletPrefab;

    bool readyToShoot = true;
    public bool canShoot = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        particles.Stop();
        particles.Clear();
    }

    public void Shoot(Quaternion direction)
    {
        if (!readyToShoot || !canShoot)
        {
            return;
        }

        readyToShoot = false;

        Projectile tempBullet = Instantiate<GameObject>(bulletPrefab).GetComponent<Projectile>();

        tempBullet.transform.position = bulletOffset.position;
        tempBullet.Shoot(direction);

        Invoke("ResetShot", timeBetweenShots);

        anim.SetTrigger("Shoot");
        anim.speed = 1;

        particles.Stop();
        particles.Play();

        AudioManager.instance.PlaySound("player_shoot");
    }


    public void UpdateSpeed(float speed)
    {
        speed /= 10;

        if (!readyToShoot) //IE, idling
            return;

        if (speed > 1)
            speed = 1;

        anim.speed = speed;
    }

    void ResetShot()
    {
        readyToShoot = true;
        anim.speed = 0;
    }

    public void SetShot(bool change)
    {
        canShoot = change;
    }
}
