using System.Runtime.CompilerServices;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] primarySpawners;
    public int[] waves;
    int level;
    int waveStage;
    bool waveInProgress;
    [SerializeField] private LayerMask enemyLayer;
    public GameObject spawn; //temporary simple proof of concept

    // i probably didn't have time before activity 5 to fully implement wave oh well
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = PlayerPrefs.GetInt("level");
        if (level == 0) level = 1; //temporary please delete for final/demo builds
        waveStage = 0;
    }


    void FixedUpdate()
    {
        if (waveInProgress)
        {

        }
    }

    public void StartWave()
    {
        waveInProgress = true;
        spawn.SetActive(true);
    }
}
