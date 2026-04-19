//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;

    public bool isSelected = false;
    protected Renderer[] rend;

    private float shootTimer = 5;
    public int level = 0;
    private float towerValue = 400;
    private int[] cost = { 200, 400, 600 };
    protected float timer;

    public LayerMask whatIsEnemy;
    public float sightRange;
    public bool enemyInSightRange;
    private Transform targetedEnemy = null;

    public Transform gunBarrel;
    public Transform wholeTurret;

    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = shootTimer;
        audioSource = GetComponent<AudioSource>();
        rend = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        
        timer -= Time.deltaTime;

        if (enemyInSightRange)
        {
            TargetEnemy();
        }

        SetRendStatus();
    }

    protected void SetRendStatus()
    {
        foreach (Renderer childRends in rend)
        {
            childRends.material.SetInt("_IsSelected", isSelected ? 1 : 0);
        }
    }

    public void ChangeRendStatus(bool value)
    {
        isSelected = value;
    }

    private void TargetEnemy()
    {
        GetEnemy();

        if (timer <= 0)
        {
            timer = shootTimer;

            TurretBullet tempBullet = Instantiate<GameObject>(bulletPrefab).GetComponent<TurretBullet>();

            tempBullet.transform.position = gunBarrel.position;
            tempBullet.Shoot(gunBarrel.rotation);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }

    private void GetEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, whatIsEnemy);

        if (targetedEnemy == null)
        {
            targetedEnemy = enemies[enemies.Length-1].transform;
        }

        wholeTurret.LookAt(targetedEnemy);

        foreach (Collider target in enemies)
        {
            if (target.transform == targetedEnemy)
            {
                return;
            }
        }

        targetedEnemy = null;
    }

    public virtual void Upgrade()
    {
        //put any upgrades here before the level increments
        shootTimer = shootTimer / 2; //shoot faster

        towerValue = towerValue + cost[level];
        level++;
    }

    public virtual float GetValue()
    {
        return towerValue;
    }

    public virtual bool CanUpgrade()
    {
        if (level < cost.Length)
        {
            return true;
        }
        else return false;
    }

    public virtual int GetUpgradeCost()
    {
        if (cost.Length == level)
        {
            return 0;
        }
        else return cost[level];
    }

    public virtual int GetSellPrice()
    {

        return (int)Mathf.Floor(towerValue * 0.8f);
    }
}

