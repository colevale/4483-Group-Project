using UnityEngine;

public class TowerElectric : MonoBehaviour
{
    public float shootTimer = 7;
    public int level = 0;
    float towerValue = 500;
    int[] cost = { 200, 400, 600 };
    float timer;

    private float radius;
    private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = shootTimer;
        radius = 15;
        damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = shootTimer;

            Collider[] whatWasHit = Physics.OverlapSphere(gameObject.transform.position, radius);

            foreach (var hit in whatWasHit)
            {
                if (CheckEnemy(hit))
                {
                    // Please update enemy script to not always take knockback...
                    hit.gameObject.GetComponent<Enemy>().TakeDamage(damage, new Vector3(0,0,0));
                }
            }
        }
    }

    private bool CheckEnemy(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            return true;
        }

        return false;
    }

    public void Upgrade()
    {
        radius += 5; //hits at longer distance
        damage += 1; //deals more damage

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
        if (cost.Length == level)
        {
            return 0;
        }
        else return cost[level];
    }

    public int GetSellPrice()
    {

        return (int)Mathf.Floor(towerValue * 0.8f);
    }
}


