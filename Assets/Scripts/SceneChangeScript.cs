using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

//designed to be universal enough for both 4483 and 4480
//mainly used for navigating between UI windows
public class SceneChangeScript : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // changes scene with inputted name, using the desired scene name in project view
    public void ChangeScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }


    /**
     * underscore seperated parameters
     * scene name _ level
     * e.g. LevelRun_1
     */
    public void ChangeLevelScene(string dualString)
    {
        string[] parameters = dualString.Split('_');
        PlayerPrefs.SetInt("c_lvl", int.Parse(parameters[1]));
        SceneManager.LoadScene(parameters[0]);
    }

    
}
