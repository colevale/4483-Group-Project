using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{
    private float towerValue = 50;
    public float timeStuck;

    private float timer;
    private bool enemyStuck;
    public List<Collider> enemiesStuck = new List<Collider>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeStuck = 4;
        enemyStuck = false;
    }

    private void Update()
    {
        //clean up destroyed GameObjects
        enemiesStuck.RemoveAll(item => item == null);
        if (enemyStuck)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                foreach (var collider in enemiesStuck)
                {
                    collider.gameObject.GetComponent<NavMeshAgent>().speed = 3.5f; // can reduce if we want "sticky" effect
                }

                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesStuck.Add(other);
            other.gameObject.GetComponent<NavMeshAgent>().speed = 0f;

            if (!enemyStuck)
            {
                timer = timeStuck;
                enemyStuck = true;
            }
        }
    }
}
