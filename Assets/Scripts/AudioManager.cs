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

    private float timer;
    private float sound1Length;
    private float sound2Length;


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
        timer += Time.deltaTime;

        if (timer >= 15)
        {
            timer = 0;
        }

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
        }

        // NEED TO ADD FOR 2 CHANNELS OF SOUND, PLAY SOUND WON'T WORK WITH 2 CHANNELS IF THERE ONLY EXISTS 1 FUNCTION (UNLESS YOU DO SOMETHING CRACKED I DUNNO)
        /*
        if (sfx != null)
        {
            if (timer >= sound1Length)
            {
                sfx = null;
            }
        }

        if (sfx2 != null)
        {
            if (timer >= sound2Length)
            {
                sfx2 = null;
            }
        }
        */
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
        /*
        if (sfx exists)
        {
            PlaySfx1(sound);
        }
        else if (sfx2 exists
        {
            PlaySfx2(sound);
        }
        else
        {
            do nothing like a loser
        }
        */

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
                sfx.clip = waveEnd;
                break;
            case "wave_start":
                sfx.clip = waveStart;
                break;
            default:
                sfx.clip = null;
                break;
        }

        sfx.Play();
    }

    /*
    public void PlaySfx1(string sound)
    {

    }

    public void PlaySfx2(string sound)
    {

    }
    */

    public float GetSFXVolume()
    {
        return masterVolume * soundVolume;
    }
}
