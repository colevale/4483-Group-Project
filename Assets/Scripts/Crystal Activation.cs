using UnityEngine;

public class CrystalActivation : MonoBehaviour
{
    public Animator anim;

    bool inAWave = false;
    public Light crystalLight;
    public AudioSource audio;
    public float audioVolumeMax = 0.5f;

    public Gun gunReference;
    public WaveManager wave;

    int nearCrystal;

    float volumeTimer = 0;
    public float volumeTransitionSpeed = 0.2f;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !inAWave)
        {
            gunReference.SetShot(false);

            nearCrystal = 1;
            if (inAWave)
                return;

            anim.SetBool("OnOff", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !inAWave)
        {
            gunReference.SetShot(true);

            nearCrystal = -1;
            anim.SetBool("OnOff", false);
        }
    }

    private void Update()
    {
        volumeTimer += Time.deltaTime * nearCrystal * volumeTransitionSpeed;

        if (volumeTimer > audioVolumeMax)
            volumeTimer = audioVolumeMax;
        if (volumeTimer < 0)
            volumeTimer = 0;


        audio.volume = volumeTimer/audioVolumeMax;

        if (Input.GetKeyDown(KeyCode.E) && nearCrystal == 1 && !inAWave)
        {
            WaveStart();
            Debug.Log("Wave has started");
        }
    }


    public void WaveStart()
    {
        AudioManager.instance.TransitionSong("defend", 5, 10);
        AudioManager.instance.PlaySound("wave_start");
        wave.StartWave();
        anim.SetBool("OnOff", false);
        crystalLight.intensity = 500;
        inAWave = true;
        gunReference.SetShot(true);
    }

    public void EndWave()
    {
        AudioManager.instance.TransitionSong("build", 5, 10);
        AudioManager.instance.PlaySound("wave_end");
        crystalLight.intensity = 0;
        inAWave = false;
    }
}
