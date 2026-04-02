using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject checkpoints;
    public int numberToSpawn;
    private int counter;
    public GameObject player;
    public bool doneSpawning;
    List<GameObject> spawned;

    [SerializeField] private GameObject enemyPreFab;
    //[SerializeField] private LayerMask buildlayer;

    [SerializeField]
    private float spawnInterval = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
        spawned = new List<GameObject>();
        doneSpawning = false;
        StartCoroutine(spawnEnemy(spawnInterval, enemyPreFab));
    }

    private void FixedUpdate()
    {
        //removes all items destroyed in spawned objects
        spawned.RemoveAll(item => item == null);
        Debug.Log(spawned.Count);
        if (counter == numberToSpawn && spawned.Count <= 0)
        {
            doneSpawning = true;
        }
    }

    //following https://www.youtube.com/watch?v=SELTWo1XZ0c

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        
        if (counter < numberToSpawn || numberToSpawn < 0)
        {

        
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation);
            newEnemy.SetActive(false);
            
            newEnemy.layer = 8;
            EnemyWaypoints patrolScript = newEnemy.GetComponent<EnemyWaypoints>();
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();

            //sets player target
            enemyScript.player = player;
            enemyScript.setupPlayer();

            //adds checkpoints
            patrolScript.Setup(checkpoints);
            newEnemy.SetActive(true);
            spawned.Add(newEnemy);
            counter++;
            StartCoroutine(spawnEnemy(interval, enemy));

        }

        
    }
}
