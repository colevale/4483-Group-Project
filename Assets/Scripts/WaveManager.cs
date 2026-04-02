using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject[] primarySpawners;
    public int[] waves;
    int level;
    int waveStage;
    bool waveInProgress;
    public GameObject player;
    PlayerController playerController;
    [SerializeField] private LayerMask enemyLayer;
    public GameObject spawn; //temporary simple proof of concept
    Spawner spawnScript;


    // i probably didn't have time before activity 5 to fully implement wave oh well
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = PlayerPrefs.GetInt("progress");
        if (level == 0) level = 1; //temporary please delete for final/demo builds
        waveStage = 0;
    }


    void FixedUpdate()
    {
        if (waveInProgress)
        {
            //proof of concept with single wave
       
            spawnScript = spawn.GetComponent<Spawner>(); 
            playerController = player.GetComponent<PlayerController>();
            //most of the componentry will be extended
            if (spawnScript.doneSpawning )
            {
                PlayerPrefs.SetInt("progress", level + 1);
                playerController.SaveGold();
                SceneManager.LoadScene("LevelRun");
            }
        }
    }

    public void StartWave()
    {
        waveInProgress = true;
        spawn.SetActive(true);
        //multi spawn
        for (int i = 0; i < primarySpawners.Length; i++)
        {
            primarySpawners[i].SetActive(true);
        }
    }

    
}
