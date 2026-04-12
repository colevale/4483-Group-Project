using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool startingInMenu;


    bool mainSongIs1 = true;
    public AudioSource song1;
    public AudioSource song2;
    public AudioSource sfx;
    public AudioSource sfx2;

    public AudioClip menuSong;
    public AudioClip buildSong;
    public AudioClip defendSong;
    public AudioClip defeatSong;
    public AudioClip victorySong;

    public AudioClip menuHover;
    public AudioClip menuBack;
    public AudioClip menuSelect;
    public AudioClip menuSelectFinal;

    public AudioClip playerJump;
    public AudioClip playerLand;
    public AudioClip playerShoot;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    public AudioClip waveStart;
    public AudioClip waveEnd;


    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float soundVolume = 1.0f;



    private float fadeInTimerCur = 0;
    private float fadeInTimerStart = 0;

    private float fadeOutTimerCur = 0;
    private float fadeOutTimerStart = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        if (startingInMenu)
            song1.clip = menuSong;
        else
            song1.clip = buildSong;
        song1.Play();
        song1.volume = masterVolume * musicVolume;

        sfx.volume = masterVolume * soundVolume;
    }

    // Update is called once per frame
    void Update()
    {

        if (fadeInTimerCur > 0)
        {
            fadeInTimerCur -= Time.deltaTime;
            if (fadeInTimerCur < 0)
                fadeInTimerCur = 0;

            if (mainSongIs1)
                song1.volume = (1 - fadeInTimerCur / fadeInTimerStart) * masterVolume * musicVolume;
            else
                song2.volume = (1 - fadeInTimerCur / fadeInTimerStart) * masterVolume * musicVolume;

        }


        if (fadeOutTimerCur > 0)
        {
            fadeOutTimerCur -= Time.deltaTime;
            if (fadeOutTimerCur < 0)
                fadeOutTimerCur = 0;


            if (!mainSongIs1)
                song1.volume = (fadeOutTimerCur / fadeOutTimerStart) * masterVolume * musicVolume;
            else
                song2.volume = (fadeOutTimerCur / fadeOutTimerStart) * masterVolume * musicVolume;

            Debug.Log(fadeOutTimerCur);
        }



    }

    public void TransitionSong(string nextSong, float curSongFadeOut, float nextSongFadeIn)
    {
        AudioClip clip = buildSong;
        switch (nextSong)
        {
            case "build":
                clip = buildSong;
                break;
            case "defend":
                clip = defendSong;
                break;
            case "defeat":
                clip = defeatSong;
                break;
            case "victory":
                clip = victorySong;
                break;
        }
        if (mainSongIs1)
        {
            song2.clip = clip;
            song2.volume = 0;
            song2.Play();
        }
        else
        {
            song1.clip = clip;
            song1.volume = 0;
            song1.Play();
        }

        mainSongIs1 = !mainSongIs1;




        fadeOutTimerStart = curSongFadeOut;
        fadeOutTimerCur = curSongFadeOut;

        fadeInTimerStart = nextSongFadeIn;
        fadeInTimerCur = nextSongFadeIn;
    }



    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "menu_back":
                sfx.clip = menuBack;
                break;
            case "menu_hover":
                sfx.clip = menuHover;
                break;
            case "menu_select":
                sfx.clip = menuSelect;
                break;
            case "menu_select_final":
                sfx.clip = menuSelectFinal;
                break;
            case "player_hurt":
                sfx.clip = playerHurt;
                break;
            case "player_die":
                sfx.clip = playerDie;
                break;
            case "player_shoot":
                sfx.clip = playerShoot;
                break;
            case "player_jump":
                sfx.clip = playerJump;
                break;
            case "player_land":
                sfx.clip = playerLand;
                break;
            case "wave_end":
                sfx2.clip = waveEnd;
                break;
            case "wave_start":
                sfx2.clip = waveStart;
                break;
        }

        sfx.Play();
    }
    


    public float GetSFXVolume()
    {
        return masterVolume * soundVolume;
    }
}
