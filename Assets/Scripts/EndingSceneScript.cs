using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneScript : MonoBehaviour
{
    public TMP_Text endingText;
    public List<RawImage> endingBG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int endingNum = PlayerPrefs.GetInt("end");
        //placeholder kept if invalid value as a way to debug
        if (endingNum == 0)
        {
            endingText.text = "You spared the Orcs.\nThe King will not be pleased.\nEnding 1/2";
        }
        else if (endingNum == 1)
        {
            endingText.text = "You massacred the Orcs.\nHow do you feel?\nEnding 2/2";
        }
        endingBG[endingNum].gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();

    }
}
