using System.Collections;
using UnityEngine;

public class GameOver : SceneChangeScript 
{

    void FixedUpdate()
    {
        
    }
    public void SpareEnding()
    {
        StartCoroutine(SceneChoice(0)); ;
        ChangeScene("End"); 
    }

    public void KillEnding()
    {
        StartCoroutine(SceneChoice(1));
        
        
        ChangeScene("End");
    }

    IEnumerator SceneChoice(int choice)
    {

        PlayerPrefs.SetInt("end", choice);
        
       
        yield return new WaitForSeconds(1f);
    }
}
