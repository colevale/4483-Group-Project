using UnityEngine;

public class Settings : MonoBehaviour
{
    public void Unlock(int level)
    {
        AudioManager.instance.PlaySound("menu_select");
        PlayerPrefs.SetInt("progress", level);
    }
}
