using UnityEngine;

public class TowerElectric : Tower
{
    private float electricShootTimer = 7;
    private float electricTowerValue = 700;
    private int[] electricCost = { 500, 700, 1000 };

    private float radius;
    private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = electricShootTimer;
        level = 0;
        radius = 15;
        damage = 2;
        rend = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = electricShootTimer;

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

        SetRendStatus();
    }

    private bool CheckEnemy(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            return true;
        }

        return false;
    }

    public override float GetValue()
    {
        return electricTowerValue;
    }

    public override int GetUpgradeCost()
    {
        if (electricCost.Length == level)
        {
            return 0;
        }
        else return electricCost[level];
    }

    public override void Upgrade()
    {
        radius += 5; //hits at longer distance
        damage += 1; //deals more damage

        electricTowerValue = electricTowerValue + electricCost[level];
        level++;
    }

    public override int GetSellPrice()
    {
        return (int)Mathf.Floor(electricTowerValue * 0.8f);
    }
}


