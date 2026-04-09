using UnityEngine;

public class TowerElectric : Tower
{
    public new float shootTimer = 7;
    public new float towerValue = 500;
    new int[] cost = { 200, 400, 600 };

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
                    hit.gameObject.GetComponent<Enemy>().TakeDamage(damage, new Vector3(0, 0, 0));
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

    public override void Upgrade()
    {
        radius += 5; //hits at longer distance
        damage += 1; //deals more damage

        towerValue = towerValue + cost[level];
        level++;
    }
}


