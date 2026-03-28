using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootTimer = 5;
    public int level = 0;
    float towerValue = 200;
    int[] cost = {75, 150, 300};
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

    public void Upgrade()
    {
        //put any upgrades here before the level increments
        shootTimer = shootTimer / 2;
        towerValue = towerValue + cost[level];
        level++;
    }

    public bool CanUpgrade()
    {
        if (level < cost.Length)
        {
            return true;
        }
        else return false;
    }

    public int GetUpgradeCost()
    {
        return cost[level];
    }

    public int GetSellPrice()
    {

        
        return (int) Mathf.Floor(towerValue * 0.8f);
    }
}
